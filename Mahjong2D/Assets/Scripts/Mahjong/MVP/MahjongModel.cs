using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MahjongModel
{
    private readonly MahjongBoardGenerator _generator;
    private readonly IMovesProvider _movesProvider;
    private readonly ISoundProvider _soundProvider;

    private readonly List<MahjongTileData> tiles = new();

    private MahjongTileData firstSelectedTile;

    private CancellationTokenSource generateCancellation;

    public IReadOnlyList<MahjongTileData> Tiles => tiles;

    public MahjongModel(
        MahjongBoardGenerator generator,
        IMovesProvider movesProvider,
        ISoundProvider soundProvider)
    {
        _generator = generator;
        _movesProvider = movesProvider;
        _soundProvider = soundProvider;
    }


    public void Initialize()
    {
    }


    public void Dispose()
    {
        generateCancellation?.Cancel();
        generateCancellation?.Dispose();
        generateCancellation = null;

        tiles.Clear();

        firstSelectedTile = null;
    }


    // =========================================================
    // GENERATE
    // =========================================================

    public async UniTask GenerateBoard(
        List<Sprite> sprites,
        float tileDelay)
    {
        // Отменяем предыдущую генерацию,
        // если она ещё выполняется.
        generateCancellation?.Cancel();
        generateCancellation?.Dispose();

        generateCancellation =
            new CancellationTokenSource();

        CancellationToken cancellationToken =
            generateCancellation.Token;


        ClearBoard();


        if (sprites == null || sprites.Count == 0)
            return;


        List<MahjongTilePosition> positions =
            _generator.Generate(
                sprites.Count
            );


        if (positions.Count != sprites.Count * 2)
            return;


        // =====================================================
        // GENERATE SMART PAIRS
        // =====================================================

        List<int> pairIds;

        if (!TryGeneratePlayablePairs(
                positions,
                out pairIds))
        {
            Debug.LogWarning(
                "[Mahjong] Could not create a playable pair distribution."
            );

            return;
        }


        // =====================================================
        // CREATE TILE DATA
        // =====================================================

        List<MahjongTileData> generatedTiles =
            new List<MahjongTileData>();


        for (
            int id = 0;
            id < positions.Count;
            id++)
        {
            int pairId =
                pairIds[id];


            Sprite sprite =
                sprites[pairId];


            MahjongTilePosition position =
                positions[id];


            MahjongTileData tile =
                new MahjongTileData(
                    id,
                    pairId,
                    sprite,
                    position.Layer,
                    position.GridX,
                    position.GridY
                );


            generatedTiles.Add(
                tile
            );


            // Все данные добавляем в Model сразу.
            // Это важно для правильного расчёта Active.
            tiles.Add(
                tile
            );
        }


        // =====================================================
        // UPDATE ACTIVE STATES
        // =====================================================

        UpdateActiveStates();


        // =====================================================
        // START GENERATE
        // =====================================================

        OnStartGenerate?.Invoke();


        // =====================================================
        // CREATE TILES ONE BY ONE
        // =====================================================

        for (
            int i = 0;
            i < generatedTiles.Count;
            i++)
        {
            cancellationToken.ThrowIfCancellationRequested();


            OnTileCreated?.Invoke(
                generatedTiles[i]
            );


            // Не ждём после последнего тайла.
            if (i >= generatedTiles.Count - 1)
                continue;


            if (tileDelay > 0f)
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(tileDelay),
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                await UniTask.Yield(
                    cancellationToken
                );
            }
        }


        // =====================================================
        // STOP GENERATE
        // =====================================================

        OnStopGenerate?.Invoke();
    }


    private void ClearBoard()
    {
        if (tiles.Count == 0)
            return;


        tiles.Clear();

        firstSelectedTile = null;


        OnBoardCleared?.Invoke();
    }


    // =========================================================
    // SMART PAIR GENERATION
    // =========================================================

    private bool TryGeneratePlayablePairs(
        List<MahjongTilePosition> positions,
        out List<int> pairIds)
    {
        pairIds = null;

        const int maxAttempts = 100;

        for (
            int attempt = 0;
            attempt < maxAttempts;
            attempt++)
        {
            List<int> remainingIndices =
                new List<int>();


            for (
                int i = 0;
                i < positions.Count;
                i++)
            {
                remainingIndices.Add(i);
            }


            List<int> result =
                new List<int>();


            bool failed = false;


            // -------------------------------------------------
            // Снимаем виртуально пары одну за другой.
            //
            // На каждом шаге выбираем две позиции,
            // которые в данный момент доступны.
            // -------------------------------------------------

            while (remainingIndices.Count > 0)
            {
                List<int> activeIndices =
                    GetActivePositionIndices(
                        positions,
                        remainingIndices
                    );


                if (activeIndices.Count < 2)
                {
                    failed = true;
                    break;
                }


                // -------------------------------------------------
                // Стараемся выбирать разные слои,
                // чтобы пары не концентрировались только внизу.
                // -------------------------------------------------

                List<int> shuffledActive =
                    new List<int>(
                        activeIndices
                    );

                Shuffle(
                    shuffledActive
                );


                int firstIndex =
                    shuffledActive[0];


                int secondIndex =
                    -1;


                MahjongTilePosition firstPosition =
                    positions[firstIndex];


                // Сначала ищем пару на другом слое.
                for (
                    int i = 1;
                    i < shuffledActive.Count;
                    i++)
                {
                    int candidateIndex =
                        shuffledActive[i];

                    MahjongTilePosition candidate =
                        positions[candidateIndex];


                    if (
                        candidate.Layer !=
                        firstPosition.Layer)
                    {
                        secondIndex =
                            candidateIndex;

                        break;
                    }
                }


                // Если другого слоя нет —
                // берём любую другую доступную позицию.
                if (secondIndex == -1)
                {
                    for (
                        int i = 1;
                        i < shuffledActive.Count;
                        i++)
                    {
                        if (
                            shuffledActive[i] !=
                            firstIndex)
                        {
                            secondIndex =
                                shuffledActive[i];

                            break;
                        }
                    }
                }


                if (secondIndex == -1)
                {
                    failed = true;
                    break;
                }


                result.Add(
                    firstIndex
                );

                result.Add(
                    secondIndex
                );


                remainingIndices.Remove(
                    firstIndex
                );

                remainingIndices.Remove(
                    secondIndex
                );
            }


            if (failed)
                continue;


            // -------------------------------------------------
            // Преобразуем последовательность пар
            // в pairId для каждой позиции.
            // -------------------------------------------------

            pairIds =
                new List<int>(
                    new int[positions.Count]
                );


            for (
                int i = 0;
                i < result.Count;
                i += 2)
            {
                int firstIndex =
                    result[i];

                int secondIndex =
                    result[i + 1];


                int pairId =
                    i / 2;


                pairIds[firstIndex] =
                    pairId;

                pairIds[secondIndex] =
                    pairId;
            }


            return true;
        }


        return false;
    }


    // =========================================================
    // GET ACTIVE POSITIONS
    // =========================================================

    private List<int> GetActivePositionIndices(
        List<MahjongTilePosition> positions,
        List<int> remainingIndices)
    {
        List<int> activeIndices =
            new List<int>();


        foreach (
            int index
            in remainingIndices)
        {
            MahjongTilePosition tile =
                positions[index];


            if (
                IsPositionActive(
                    tile,
                    positions,
                    remainingIndices))
            {
                activeIndices.Add(
                    index
                );
            }
        }


        return activeIndices;
    }


    // =========================================================
    // CHECK POSITION ACTIVE
    // =========================================================

    private bool IsPositionActive(
        MahjongTilePosition tile,
        List<MahjongTilePosition> positions,
        List<int> remainingIndices)
    {
        // -----------------------------------------------------
        // TILE ABOVE
        // -----------------------------------------------------

        foreach (
            int index
            in remainingIndices)
        {
            MahjongTilePosition other =
                positions[index];


            if (other.Equals(tile))
                continue;


            if (other.Layer <= tile.Layer)
                continue;


            if (
                IsPositionOverlapping(
                    tile,
                    other))
            {
                return false;
            }
        }


        // -----------------------------------------------------
        // LEFT / RIGHT
        // -----------------------------------------------------

        bool leftBlocked =
            HasPositionOnSide(
                tile,
                -1,
                positions,
                remainingIndices
            );


        bool rightBlocked =
            HasPositionOnSide(
                tile,
                1,
                positions,
                remainingIndices
            );


        return
            !leftBlocked ||
            !rightBlocked;
    }


    // =========================================================
    // SIDE BLOCK
    // =========================================================

    private bool HasPositionOnSide(
        MahjongTilePosition tile,
        int direction,
        List<MahjongTilePosition> positions,
        List<int> remainingIndices)
    {
        foreach (
            int index
            in remainingIndices)
        {
            MahjongTilePosition other =
                positions[index];


            if (other.Equals(tile))
                continue;


            if (other.Layer != tile.Layer)
                continue;


            if (other.GridY != tile.GridY)
                continue;


            if (
                other.GridX !=
                tile.GridX + direction)
            {
                continue;
            }


            return true;
        }


        return false;
    }


    // =========================================================
    // OVERLAP
    // =========================================================

    private bool IsPositionOverlapping(
        MahjongTilePosition first,
        MahjongTilePosition second)
    {
        int deltaX =
            Mathf.Abs(
                first.GridX -
                second.GridX
            );


        int deltaY =
            Mathf.Abs(
                first.GridY -
                second.GridY
            );


        return
            deltaX <= 1 &&
            deltaY <= 1;
    }


    // =========================================================
    // HINT
    // =========================================================

    public void Hint()
    {
        if (firstSelectedTile != null)
            OnTileUnselected?.Invoke(
                firstSelectedTile.Id
            );


        firstSelectedTile = null;


        List<(int firstId, int secondId)>
            availablePairs = new();


        for (
            int i = 0;
            i < tiles.Count;
            i++)
        {
            MahjongTileData firstTile =
                tiles[i];


            if (
                firstTile.IsRemoved ||
                !firstTile.IsActive)
            {
                continue;
            }


            for (
                int j = i + 1;
                j < tiles.Count;
                j++)
            {
                MahjongTileData secondTile =
                    tiles[j];


                if (
                    secondTile.IsRemoved ||
                    !secondTile.IsActive)
                {
                    continue;
                }


                if (
                    firstTile.PairId !=
                    secondTile.PairId)
                {
                    continue;
                }


                availablePairs.Add(
                    (
                        firstTile.Id,
                        secondTile.Id
                    )
                );
            }
        }


        if (availablePairs.Count == 0)
            return;


        var (
            firstId,
            secondId
        ) =
            availablePairs[
                UnityEngine.Random.Range(
                    0,
                    availablePairs.Count
                )
            ];


        _movesProvider.Increase();


        OnTileHintSelected?.Invoke(
            firstId,
            secondId
        );
    }


    // =========================================================
    // SELECT
    // =========================================================

    public void SelectTile(int tileId)
    {
        MahjongTileData tile =
            GetTile(tileId);


        if (tile == null)
            return;


        if (tile.IsRemoved)
            return;


        if (!tile.IsActive)
            return;


        _movesProvider.Increase();


        // =====================================================
        // FIRST CLICK
        // =====================================================

        if (firstSelectedTile == null)
        {
            firstSelectedTile =
                tile;


            OnTileSelected?.Invoke(
                tile.Id
            );


            _soundProvider.PlayOneShot(
                "Choose"
            );


            return;
        }


        // =====================================================
        // CLICK ON SAME TILE AGAIN
        // =====================================================

        if (firstSelectedTile == tile)
        {
            OnTileUnselected?.Invoke(
                firstSelectedTile.Id
            );


            _soundProvider.PlayOneShot(
                "Choose"
            );


            firstSelectedTile = null;


            return;
        }


        // =====================================================
        // SECOND TILE
        // =====================================================

        MahjongTileData firstTile =
            firstSelectedTile;


        MahjongTileData secondTile =
            tile;


        // =====================================================
        // PAIR CHECK
        // =====================================================

        if (
            firstTile.PairId !=
            secondTile.PairId)
        {
            OnTileUnselected?.Invoke(
                firstTile.Id
            );


            firstSelectedTile = null;


            _soundProvider.PlayOneShot(
                "Incorrect"
            );


            return;
        }


        // =====================================================
        // CORRECT PAIR
        // =====================================================

        OnTileUnselected?.Invoke(
            firstTile.Id
        );


        firstSelectedTile = null;


        OnPairTileRemoved?.Invoke(
            firstTile.Id,
            secondTile.Id
        );


        RemoveTile(
            firstTile
        );


        RemoveTile(
            secondTile
        );


        _soundProvider.PlayOneShot(
            "Choose"
        );


        UpdateActiveStates();
    }


    // =========================================================
    // REMOVE
    // =========================================================

    private void RemoveTile(
        MahjongTileData tile)
    {
        if (tile == null)
            return;


        if (tile.IsRemoved)
            return;


        tile.Remove();


        OnTileRemoved?.Invoke(
            tile.Id
        );
    }


    // =========================================================
    // ACTIVE
    // =========================================================

    private void UpdateActiveStates()
    {
        foreach (
            MahjongTileData tile
            in tiles)
        {
            if (tile.IsRemoved)
                continue;


            bool active =
                IsTileActive(
                    tile
                );


            if (tile.IsActive == active)
                continue;


            tile.SetActive(
                active
            );


            OnTileActiveChanged?.Invoke(
                tile.Id,
                active
            );
        }
    }


    private bool IsTileActive(
        MahjongTileData tile)
    {
        if (HasTileAbove(tile))
            return false;


        bool leftBlocked =
            HasTileOnSide(
                tile,
                -1
            );


        bool rightBlocked =
            HasTileOnSide(
                tile,
                1
            );


        return
            !leftBlocked ||
            !rightBlocked;
    }


    private bool HasTileAbove(
        MahjongTileData tile)
    {
        foreach (
            MahjongTileData other
            in tiles)
        {
            if (other == tile)
                continue;


            if (other.IsRemoved)
                continue;


            if (other.Layer <= tile.Layer)
                continue;


            if (
                IsTileOverlapping(
                    tile,
                    other))
            {
                return true;
            }
        }


        return false;
    }


    private bool HasTileOnSide(
        MahjongTileData tile,
        int direction)
    {
        foreach (
            MahjongTileData other
            in tiles)
        {
            if (other == tile)
                continue;


            if (other.IsRemoved)
                continue;


            if (
                other.Layer !=
                tile.Layer)
            {
                continue;
            }


            if (
                other.GridY !=
                tile.GridY)
            {
                continue;
            }


            if (
                other.GridX !=
                tile.GridX + direction)
            {
                continue;
            }


            return true;
        }


        return false;
    }


    private bool IsTileOverlapping(
        MahjongTileData first,
        MahjongTileData second)
    {
        int deltaX =
            Mathf.Abs(
                first.GridX -
                second.GridX
            );


        int deltaY =
            Mathf.Abs(
                first.GridY -
                second.GridY
            );


        return
            deltaX <= 1 &&
            deltaY <= 1;
    }


    // =========================================================
    // MIX
    // =========================================================

    public void Mix()
    {
        if (firstSelectedTile != null)
        {
            OnTileUnselected?.Invoke(
                firstSelectedTile.Id
            );
        }


        firstSelectedTile = null;


        int firstLayerCount = 0;
        int secondLayerCount = 0;
        int thirdLayerCount = 0;


        List<MahjongTileData> availableTiles =
            new List<MahjongTileData>();


        foreach (
            MahjongTileData tile
            in tiles)
        {
            if (tile.IsRemoved)
                continue;


            availableTiles.Add(
                tile
            );


            switch (tile.Layer)
            {
                case 0:
                    firstLayerCount++;
                    break;


                case 1:
                    secondLayerCount++;
                    break;


                case 2:
                    thirdLayerCount++;
                    break;
            }
        }


        List<MahjongTilePosition> positions =
            _generator.Generate(
                firstLayerCount,
                secondLayerCount,
                thirdLayerCount
            );


        if (
            positions.Count !=
            availableTiles.Count)
        {
            return;
        }


        Shuffle(
            positions
        );


        for (
            int i = 0;
            i < availableTiles.Count;
            i++)
        {
            availableTiles[i].SetPosition(
                positions[i].Layer,
                positions[i].GridX,
                positions[i].GridY
            );
        }


        firstSelectedTile = null;


        UpdateActiveStates();


        _movesProvider.Increase();


        OnMix?.Invoke();
    }


    // =========================================================
    // HELPERS
    // =========================================================

    private MahjongTileData GetTile(
        int tileId)
    {
        foreach (
            MahjongTileData tile
            in tiles)
        {
            if (tile.Id == tileId)
                return tile;
        }


        return null;
    }


    private void Shuffle<T>(
        List<T> list)
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--)
        {
            int randomIndex =
                UnityEngine.Random.Range(
                    0,
                    i + 1
                );


            T temp =
                list[i];


            list[i] =
                list[randomIndex];


            list[randomIndex] =
                temp;
        }
    }


    // =========================================================
    // BOOLS
    // =========================================================

    public bool HasAvailableMoves()
    {
        for (
            int i = 0;
            i < tiles.Count;
            i++)
        {
            MahjongTileData firstTile =
                tiles[i];


            if (firstTile.IsRemoved)
                continue;


            if (!firstTile.IsActive)
                continue;


            for (
                int j = i + 1;
                j < tiles.Count;
                j++)
            {
                MahjongTileData secondTile =
                    tiles[j];


                if (secondTile.IsRemoved)
                    continue;


                if (!secondTile.IsActive)
                    continue;


                if (
                    firstTile.PairId !=
                    secondTile.PairId)
                {
                    continue;
                }


                return true;
            }
        }


        return false;
    }


    public bool HasRemainingTiles()
    {
        foreach (
            MahjongTileData tile
            in tiles)
        {
            if (tile.IsRemoved)
                continue;


            return true;
        }


        return false;
    }


    // =========================================================
    // OUTPUT
    // =========================================================

    public event Action<MahjongTileData>
        OnTileCreated;


    public event Action<int>
        OnTileRemoved;


    public event Action<int, int>
        OnPairTileRemoved;


    public event Action<int, bool>
        OnTileActiveChanged;


    public event Action<int>
        OnTileSelected;


    public event Action<int>
        OnTileUnselected;


    public event Action<int, int>
        OnTileHintSelected;


    public event Action
        OnBoardCleared;


    public event Action
        OnMix;


    public event Action
        OnStartGenerate;


    public event Action
        OnStopGenerate;
}
