using System;
using System.Collections.Generic;
using UnityEngine;

public class MahjongModel
{
    private readonly MahjongBoardGenerator _generator;

    private readonly List<MahjongTileData> tiles = new();

    private MahjongTileData firstSelectedTile;


    public IReadOnlyList<MahjongTileData> Tiles =>
        tiles;


    public MahjongModel(
        MahjongBoardGenerator generator)
    {
        _generator = generator;
    }


    public void Initialize()
    {
    }


    public void Dispose()
    {
        tiles.Clear();

        firstSelectedTile = null;
    }


    // =========================================================
    // GENERATE
    // =========================================================

    public void GenerateBoard(
    List<Sprite> sprites)
    {
        ClearBoard();


        if (sprites == null)
            return;


        List<MahjongTilePosition> positions =
            _generator.Generate(
                sprites.Count
            );


        if (positions.Count != sprites.Count * 2)
            return;


        // =====================================================
        // CREATE PAIRS
        // =====================================================

        List<int> pairIds =
            new List<int>();


        for (
            int pairId = 0;
            pairId < sprites.Count;
            pairId++)
        {
            //  аждый PairId должен встретитьс€ ровно 2 раза.

            pairIds.Add(pairId);
            pairIds.Add(pairId);
        }


        // =====================================================
        // SHUFFLE PAIRS
        // =====================================================

        Shuffle(
            pairIds
        );


        // =====================================================
        // CREATE TILES
        // =====================================================

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


            tiles.Add(
                tile
            );


            OnTileCreated?.Invoke(
                tile
            );
        }


        UpdateActiveStates();
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
    // SELECT
    // =========================================================

    public void SelectTile(
        int tileId)
    {
        MahjongTileData tile =
            GetTile(tileId);


        if (tile == null)
            return;


        if (tile.IsRemoved)
            return;


        if (!tile.IsActive)
            return;


        // =====================================================
        // FIRST CLICK
        // =====================================================

        if (firstSelectedTile == null)
        {
            firstSelectedTile = tile;


            OnTileSelected?.Invoke(
                tile.Id
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

        if (firstTile.PairId != secondTile.PairId)
        {
            OnTileUnselected?.Invoke(
                firstTile.Id
            );


            firstSelectedTile = null;


            return;
        }


        // =====================================================
        // CORRECT PAIR
        // =====================================================

        OnTileUnselected?.Invoke(
            firstTile.Id
        );


        firstSelectedTile = null;


        RemoveTile(firstTile);
        RemoveTile(secondTile);


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
                IsTileActive(tile);


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
        int firstLayerCount = 0;
        int secondLayerCount = 0;
        int thirdLayerCount = 0;


        List<MahjongTileData> availableTiles =
            new List<MahjongTileData>();


        foreach (MahjongTileData tile in tiles)
        {
            if (tile.IsRemoved)
                continue;


            availableTiles.Add(tile);


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


        if (positions.Count != availableTiles.Count)
            return;


        Shuffle(positions);


        for (int i = 0; i < availableTiles.Count; i++)
        {
            availableTiles[i].SetPosition(
                positions[i].Layer,
                positions[i].GridX,
                positions[i].GridY
            );
        }


        firstSelectedTile = null;


        UpdateActiveStates();


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


            T temp = list[i];


            list[i] =
                list[randomIndex];


            list[randomIndex] =
                temp;
        }
    }


    // =========================================================
    // OUTPUT
    // =========================================================

    public event Action<MahjongTileData>
        OnTileCreated;


    public event Action<int>
        OnTileRemoved;


    public event Action<int, bool>
        OnTileActiveChanged;


    public event Action<int>
        OnTileSelected;


    public event Action<int>
        OnTileUnselected;


    public event Action
        OnBoardCleared;


    public event Action
        OnMix;
}