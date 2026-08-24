using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MahjongBoard : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private RectTransform boardRect;

    [Header("Tile Prefab")]
    [SerializeField] private MahjongTile tilePrefab;

    [Header("Tile Size")]
    [SerializeField]
    private Vector2 tileSize =
        new Vector2(235f, 290f);

    [Header("Layer Offset")]
    [SerializeField] private float layerOffsetX = 18f;
    [SerializeField] private float layerOffsetY = 22f;


    // =========================================================
    // GENERATION SETTINGS
    // =========================================================

    [Header("Generation")]

    [SerializeField]
    [Range(1, 20)]
    private int layer0TileCount = 20;

    [SerializeField]
    [Range(1, 20)]
    private int layer1TileCount = 12;

    [SerializeField]
    [Range(1, 15)]
    private int layer2TileCount = 5;

    [SerializeField]
    private int randomSeed = 0;

    [SerializeField]
    private bool useRandomSeed = true;


    // =========================================================
    // MIX SETTINGS
    // =========================================================

    [Header("Mix")]

    [SerializeField]
    private float mixDuration = 0.35f;

    [SerializeField]
    private Ease mixEase = Ease.InOutQuad;


    // =========================================================
    // INTERNAL DATA
    // =========================================================

    private readonly List<SpawnedTileData> spawnedTiles =
        new List<SpawnedTileData>();


    private readonly List<GridPosition> layer0Positions =
        new List<GridPosition>();

    private readonly List<GridPosition> layer1Positions =
        new List<GridPosition>();

    private readonly List<GridPosition> layer2Positions =
        new List<GridPosition>();


    // =========================================================
    // SELECTION
    // =========================================================

    private MahjongTile firstSelectedTile;


    // =========================================================
    // MIX STATE
    // =========================================================

    private bool isMixing;


    // =========================================================
    // DATA CLASSES
    // =========================================================

    private class SpawnedTileData
    {
        public MahjongTile tile;

        public int layer;

        public int gridX;

        public int gridY;
    }


    private class GridPosition
    {
        public int x;

        public int y;


        public GridPosition(
            int x,
            int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        GenerateBoard();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            GenerateBoard();
        }

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            Mix();
        }
    }


    // =========================================================
    // GENERATE BOARD
    // =========================================================

    [ContextMenu("Generate Board")]
    public void GenerateBoard()
    {
        /*
         * Если вдруг GenerateBoard вызвали
         * во время Mix — останавливаем DOTween.
         */

        DOTween.Kill(boardRect);


        isMixing = false;

        firstSelectedTile = null;


        ClearBoard();

        InitializeRandom();

        GeneratePositions();

        SpawnGeneratedTiles();

        SortTiles();

        UpdateTileStates();
    }


    // =========================================================
    // RANDOM
    // =========================================================

    private void InitializeRandom()
    {
        if (useRandomSeed)
        {
            Random.InitState(
                System.Environment.TickCount
            );
        }
        else
        {
            Random.InitState(randomSeed);
        }
    }


    // =========================================================
    // GENERATE POSITIONS
    // =========================================================

    private void GeneratePositions()
    {
        layer0Positions.Clear();

        layer1Positions.Clear();

        layer2Positions.Clear();


        GenerateLayer0();

        GenerateLayer1();

        GenerateLayer2();
    }


    // =========================================================
    // LAYER 0
    // =========================================================

    private void GenerateLayer0()
    {
        List<GridPosition> available =
            CreateAllPositions(4, 5);


        GridPosition start =
            GetCenterPosition(4, 5);


        layer0Positions.Add(start);


        available.RemoveAll(
            p =>
                p.x == start.x &&
                p.y == start.y
        );


        while (
            layer0Positions.Count <
            layer0TileCount)
        {
            List<GridPosition> candidates =
                GetNeighbourCandidates(
                    layer0Positions,
                    available
                );


            if (candidates.Count == 0)
                break;


            GridPosition selected =
                candidates[
                    Random.Range(
                        0,
                        candidates.Count
                    )
                ];


            layer0Positions.Add(
                selected
            );


            available.Remove(
                selected
            );
        }
    }


    // =========================================================
    // LAYER 1
    // =========================================================

    private void GenerateLayer1()
    {
        List<GridPosition> available =
            CreateAllPositions(4, 5);


        List<GridPosition> candidates =
            new List<GridPosition>();


        foreach (GridPosition position in available)
        {
            if (
                HasPosition(
                    layer0Positions,
                    position.x,
                    position.y
                )
            )
            {
                candidates.Add(position);
            }
        }


        if (candidates.Count == 0)
            return;


        GridPosition start =
            candidates[
                Random.Range(
                    0,
                    candidates.Count
                )
            ];


        layer1Positions.Add(start);

        candidates.Remove(start);


        while (
            layer1Positions.Count <
            layer1TileCount)
        {
            List<GridPosition> localCandidates =
                GetNeighbourCandidates(
                    layer1Positions,
                    candidates
                );


            if (localCandidates.Count == 0)
                break;


            GridPosition selected =
                localCandidates[
                    Random.Range(
                        0,
                        localCandidates.Count
                    )
                ];


            layer1Positions.Add(
                selected
            );


            candidates.Remove(
                selected
            );
        }
    }


    // =========================================================
    // LAYER 2
    // =========================================================

    private void GenerateLayer2()
    {
        List<GridPosition> available =
            CreateAllPositions(3, 5);


        List<GridPosition> candidates =
            new List<GridPosition>();


        foreach (GridPosition position in available)
        {
            int leftX =
                position.x;


            int rightX =
                position.x + 1;


            bool hasLeftSupport =
                HasPosition(
                    layer1Positions,
                    leftX,
                    position.y
                );


            bool hasRightSupport =
                HasPosition(
                    layer1Positions,
                    rightX,
                    position.y
                );


            /*
             * Для третьего яруса
             * обязательны ОБЕ опоры.
             */

            if (
                hasLeftSupport &&
                hasRightSupport
            )
            {
                candidates.Add(position);
            }
        }


        if (candidates.Count == 0)
            return;


        GridPosition start =
            candidates[
                Random.Range(
                    0,
                    candidates.Count
                )
            ];


        layer2Positions.Add(start);

        candidates.Remove(start);


        while (
            layer2Positions.Count <
            layer2TileCount)
        {
            List<GridPosition> localCandidates =
                GetNeighbourCandidates(
                    layer2Positions,
                    candidates
                );


            if (localCandidates.Count == 0)
                break;


            GridPosition selected =
                localCandidates[
                    Random.Range(
                        0,
                        localCandidates.Count
                    )
                ];


            layer2Positions.Add(
                selected
            );


            candidates.Remove(
                selected
            );
        }
    }


    // =========================================================
    // CREATE GRID
    // =========================================================

    private List<GridPosition> CreateAllPositions(
        int columns,
        int rows)
    {
        List<GridPosition> result =
            new List<GridPosition>();


        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                result.Add(
                    new GridPosition(
                        x,
                        y
                    )
                );
            }
        }


        return result;
    }


    // =========================================================
    // CENTER
    // =========================================================

    private GridPosition GetCenterPosition(
        int columns,
        int rows)
    {
        return new GridPosition(
            (columns - 1) / 2,
            (rows - 1) / 2
        );
    }


    // =========================================================
    // NEIGHBOURS
    // =========================================================

    private List<GridPosition> GetNeighbourCandidates(
        List<GridPosition> current,
        List<GridPosition> available)
    {
        List<GridPosition> result =
            new List<GridPosition>();


        foreach (GridPosition candidate in available)
        {
            foreach (GridPosition existing in current)
            {
                int dx =
                    Mathf.Abs(
                        candidate.x -
                        existing.x
                    );


                int dy =
                    Mathf.Abs(
                        candidate.y -
                        existing.y
                    );


                if (
                    dx <= 1 &&
                    dy <= 1 &&
                    (dx + dy) > 0
                )
                {
                    result.Add(candidate);

                    break;
                }
            }
        }


        return result;
    }


    // =========================================================
    // POSITION CHECK
    // =========================================================

    private bool HasPosition(
        List<GridPosition> positions,
        int x,
        int y)
    {
        foreach (GridPosition position in positions)
        {
            if (
                position.x == x &&
                position.y == y
            )
            {
                return true;
            }
        }


        return false;
    }


    // =========================================================
    // SPAWN GENERATED TILES
    // =========================================================

    private void SpawnGeneratedTiles()
    {
        foreach (GridPosition position in layer0Positions)
        {
            SpawnLayer0Tile(position);
        }


        foreach (GridPosition position in layer1Positions)
        {
            SpawnLayer1Tile(position);
        }


        foreach (GridPosition position in layer2Positions)
        {
            SpawnLayer2Tile(position);
        }
    }


    // =========================================================
    // LAYER 0 POSITION
    // =========================================================

    private void SpawnLayer0Tile(
        GridPosition position)
    {
        const int columns = 4;

        const int rows = 5;


        float startX =
            -(columns - 1) *
            tileSize.x *
            0.5f;


        float startY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        Vector2 tilePosition =
            new Vector2(
                startX +
                position.x *
                tileSize.x,

                startY +
                position.y *
                tileSize.y
            );


        SpawnTile(
            tilePosition,
            0,
            position.x,
            position.y
        );
    }


    // =========================================================
    // LAYER 1 POSITION
    // =========================================================

    private void SpawnLayer1Tile(
        GridPosition position)
    {
        const int columns = 4;

        const int rows = 5;


        float startX =
            -(columns - 1) *
            tileSize.x *
            0.5f;


        float startY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        Vector2 tilePosition =
            new Vector2(
                startX +
                position.x *
                tileSize.x +
                layerOffsetX,

                startY +
                position.y *
                tileSize.y +
                layerOffsetY
            );


        SpawnTile(
            tilePosition,
            1,
            position.x,
            position.y
        );
    }


    // =========================================================
    // LAYER 2 POSITION
    // =========================================================

    private void SpawnLayer2Tile(
        GridPosition position)
    {
        const int rows = 5;


        float layer1StartX =
            -(4 - 1) *
            tileSize.x *
            0.5f;


        float layer1X0 =
            layer1StartX +
            0 * tileSize.x +
            layerOffsetX;


        float layer1X1 =
            layer1StartX +
            1 * tileSize.x +
            layerOffsetX;


        float layer1X2 =
            layer1StartX +
            2 * tileSize.x +
            layerOffsetX;


        float layer1X3 =
            layer1StartX +
            3 * tileSize.x +
            layerOffsetX;


        float layer2X0 =
            (layer1X0 + layer1X1) *
            0.5f +
            layerOffsetX;


        float layer2X1 =
            (layer1X1 + layer1X2) *
            0.5f +
            layerOffsetX;


        float layer2X2 =
            (layer1X2 + layer1X3) *
            0.5f +
            layerOffsetX;


        float layer1StartY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        float layer1Y =
            layer1StartY +
            position.y *
            tileSize.y +
            layerOffsetY;


        float layer2Y =
            layer1Y +
            layerOffsetY;


        float x;


        if (position.x == 0)
        {
            x = layer2X0;
        }
        else if (position.x == 1)
        {
            x = layer2X1;
        }
        else
        {
            x = layer2X2;
        }


        SpawnTile(
            new Vector2(
                x,
                layer2Y
            ),
            2,
            position.x,
            position.y
        );
    }


    // =========================================================
    // SPAWN TILE
    // =========================================================

    private void SpawnTile(
        Vector2 position,
        int layer,
        int gridX,
        int gridY)
    {
        MahjongTile tile =
            Instantiate(
                tilePrefab,
                boardRect
            );


        RectTransform rect =
            tile.GetComponent<RectTransform>();


        rect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );


        rect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );


        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );


        rect.sizeDelta =
            tileSize;


        rect.anchoredPosition =
            position;


        /*
         * Сообщаем тайлу его Board.
         */

        tile.Initialize(this);


        spawnedTiles.Add(
            new SpawnedTileData
            {
                tile = tile,

                layer = layer,

                gridX = gridX,

                gridY = gridY
            }
        );
    }


    // =========================================================
    // SORT
    // =========================================================

    private void SortTiles()
    {
        spawnedTiles.Sort(
            (a, b) =>
            {
                int layerCompare =
                    a.layer.CompareTo(
                        b.layer
                    );


                if (layerCompare != 0)
                    return layerCompare;


                int yCompare =
                    a.gridY.CompareTo(
                        b.gridY
                    );


                if (yCompare != 0)
                    return yCompare;


                return a.gridX.CompareTo(
                    b.gridX
                );
            }
        );


        for (
            int i = 0;
            i < spawnedTiles.Count;
            i++)
        {
            spawnedTiles[i]
                .tile
                .transform
                .SetSiblingIndex(i);
        }
    }


    // =========================================================
    // TILE CLICK
    // =========================================================

    public void OnTileClicked(
        MahjongTile clickedTile)
    {
        /*
         * Во время Mix клики запрещены.
         */

        if (isMixing)
            return;


        /*
         * Первый тайл.
         */

        if (firstSelectedTile == null)
        {
            firstSelectedTile =
                clickedTile;

            return;
        }


        /*
         * Повторный клик по тому же тайлу.
         */

        if (
            firstSelectedTile ==
            clickedTile
        )
        {
            return;
        }


        /*
         * Пока просто удаляем
         * любые два активных тайла.
         */

        RemoveTile(
            firstSelectedTile
        );


        RemoveTile(
            clickedTile
        );


        firstSelectedTile = null;


        /*
         * Пересчитываем доступность
         * оставшихся тайлов.
         */

        UpdateTileStates();
    }


    // =========================================================
    // REMOVE TILE
    // =========================================================

    private void RemoveTile(
        MahjongTile tile)
    {
        for (
            int i =
                spawnedTiles.Count - 1;

            i >= 0;

            i--)
        {
            if (
                spawnedTiles[i].tile ==
                tile
            )
            {
                spawnedTiles.RemoveAt(i);

                break;
            }
        }


        if (tile != null)
        {
            Destroy(
                tile.gameObject
            );
        }
    }


    // =========================================================
    // MIX
    // =========================================================

    public void Mix()
    {
        /*
         * Нельзя запустить Mix повторно,
         * пока предыдущий ещё идёт.
         */

        if (isMixing)
            return;


        /*
         * Если осталось меньше двух тайлов,
         * перемешивать нечего.
         */

        if (spawnedTiles.Count <= 1)
            return;


        isMixing = true;


        /*
         * Сбрасываем первый выбранный тайл.
         */

        firstSelectedTile = null;


        /*
         * На всякий случай убиваем старые
         * Tween'ы позиций тайлов.
         */

        foreach (SpawnedTileData data in spawnedTiles)
        {
            RectTransform rect =
                data.tile.GetComponent<RectTransform>();


            rect.DOKill();
        }


        /*
         * Собираем позиции отдельно
         * для каждого яруса.
         */

        List<Vector2> layer0TargetPositions =
            GetCurrentLayerPositions(0);


        List<Vector2> layer1TargetPositions =
            GetCurrentLayerPositions(1);


        List<Vector2> layer2TargetPositions =
            GetCurrentLayerPositions(2);


        /*
         * Перемешиваем позиции.
         */

        Shuffle(
            layer0TargetPositions
        );


        Shuffle(
            layer1TargetPositions
        );


        Shuffle(
            layer2TargetPositions
        );


        /*
         * Индексы назначения.
         */

        int layer0Index = 0;

        int layer1Index = 0;

        int layer2Index = 0;


        /*
         * Запускаем перемещение.
         */

        foreach (SpawnedTileData data in spawnedTiles)
        {
            RectTransform rect =
                data.tile.GetComponent<RectTransform>();


            Vector2 targetPosition;


            if (data.layer == 0)
            {
                targetPosition =
                    layer0TargetPositions[
                        layer0Index
                    ];


                layer0Index++;
            }
            else if (data.layer == 1)
            {
                targetPosition =
                    layer1TargetPositions[
                        layer1Index
                    ];


                layer1Index++;
            }
            else
            {
                targetPosition =
                    layer2TargetPositions[
                        layer2Index
                    ];


                layer2Index++;
            }


            /*
             * Плавное движение.
             */

            rect
                .DOAnchorPos(
                    targetPosition,
                    mixDuration
                )
                .SetEase(
                    mixEase
                );
        }


        /*
         * После окончания Mix:
         *
         * 1. Синхронизируем gridX/gridY.
         * 2. Пересчитываем доступность.
         * 3. Разрешаем клики.
         */

        DOVirtual.DelayedCall(
            mixDuration,
            FinishMix
        );
    }


    // =========================================================
    // GET CURRENT POSITIONS
    // =========================================================

    private List<Vector2> GetCurrentLayerPositions(
        int targetLayer)
    {
        List<Vector2> positions =
            new List<Vector2>();


        foreach (SpawnedTileData data in spawnedTiles)
        {
            if (
                data.layer !=
                targetLayer
            )
            {
                continue;
            }


            RectTransform rect =
                data.tile.GetComponent<RectTransform>();


            positions.Add(
                rect.anchoredPosition
            );
        }


        return positions;
    }


    // =========================================================
    // FINISH MIX
    // =========================================================

    private void FinishMix()
    {
        /*
         * Сначала обновляем логические координаты
         * согласно новым позициям.
         */

        UpdateLogicalPositionsAfterMix();


        /*
         * Затем сортируем Canvas.
         */

        SortTiles();


        /*
         * Пересчитываем активность.
         */

        UpdateTileStates();


        /*
         * Теперь снова разрешаем клики.
         */

        isMixing = false;
    }


    // =========================================================
    // UPDATE LOGICAL POSITIONS
    // =========================================================

    private void UpdateLogicalPositionsAfterMix()
    {
        /*
         * После Mix позиции визуально уже поменялись.
         *
         * Теперь каждому тайлу нужно назначить
         * соответствующий gridX/gridY.
         */


        foreach (SpawnedTileData data in spawnedTiles)
        {
            RectTransform rect =
                data.tile.GetComponent<RectTransform>();


            Vector2 position =
                rect.anchoredPosition;


            if (data.layer == 0)
            {
                UpdateLayer0LogicalPosition(
                    data,
                    position
                );
            }
            else if (data.layer == 1)
            {
                UpdateLayer1LogicalPosition(
                    data,
                    position
                );
            }
            else
            {
                UpdateLayer2LogicalPosition(
                    data,
                    position
                );
            }
        }
    }


    // =========================================================
    // UPDATE LAYER 0 LOGIC
    // =========================================================

    private void UpdateLayer0LogicalPosition(
        SpawnedTileData data,
        Vector2 position)
    {
        const int columns = 4;

        const int rows = 5;


        float startX =
            -(columns - 1) *
            tileSize.x *
            0.5f;


        float startY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        int x =
            Mathf.RoundToInt(
                (position.x - startX) /
                tileSize.x
            );


        int y =
            Mathf.RoundToInt(
                (position.y - startY) /
                tileSize.y
            );


        data.gridX = x;

        data.gridY = y;
    }


    // =========================================================
    // UPDATE LAYER 1 LOGIC
    // =========================================================

    private void UpdateLayer1LogicalPosition(
        SpawnedTileData data,
        Vector2 position)
    {
        const int columns = 4;

        const int rows = 5;


        float startX =
            -(columns - 1) *
            tileSize.x *
            0.5f;


        float startY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        int x =
            Mathf.RoundToInt(
                (
                    position.x -
                    layerOffsetX -
                    startX
                ) /
                tileSize.x
            );


        int y =
            Mathf.RoundToInt(
                (
                    position.y -
                    layerOffsetY -
                    startY
                ) /
                tileSize.y
            );


        data.gridX = x;

        data.gridY = y;
    }


    // =========================================================
    // UPDATE LAYER 2 LOGIC
    // =========================================================

    private void UpdateLayer2LogicalPosition(
        SpawnedTileData data,
        Vector2 position)
    {
        const int rows = 5;


        float layer1StartX =
            -(4 - 1) *
            tileSize.x *
            0.5f;


        float layer1X0 =
            layer1StartX +
            0 * tileSize.x +
            layerOffsetX;


        float layer1X1 =
            layer1StartX +
            1 * tileSize.x +
            layerOffsetX;


        float layer1X2 =
            layer1StartX +
            2 * tileSize.x +
            layerOffsetX;


        float layer1X3 =
            layer1StartX +
            3 * tileSize.x +
            layerOffsetX;


        float layer2X0 =
            (layer1X0 + layer1X1) *
            0.5f +
            layerOffsetX;


        float layer2X1 =
            (layer1X1 + layer1X2) *
            0.5f +
            layerOffsetX;


        float layer2X2 =
            (layer1X2 + layer1X3) *
            0.5f +
            layerOffsetX;


        float[] possibleX =
        {
            layer2X0,
            layer2X1,
            layer2X2
        };


        int closestX = 0;


        float closestDistance =
            Mathf.Abs(
                position.x -
                possibleX[0]
            );


        for (
            int i = 1;
            i < possibleX.Length;
            i++)
        {
            float distance =
                Mathf.Abs(
                    position.x -
                    possibleX[i]
                );


            if (
                distance <
                closestDistance
            )
            {
                closestDistance =
                    distance;


                closestX = i;
            }
        }


        float layer1StartY =
            -(rows - 1) *
            tileSize.y *
            0.5f;


        int y =
            Mathf.RoundToInt(
                (
                    position.y -
                    layerOffsetY -
                    layerOffsetY -
                    layer1StartY
                ) /
                tileSize.y
            );


        data.gridX =
            closestX;


        data.gridY =
            y;
    }


    // =========================================================
    // SHUFFLE
    // =========================================================

    private void Shuffle<T>(
        List<T> list)
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--)
        {
            int randomIndex =
                Random.Range(
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
    // UPDATE TILE STATES
    // =========================================================

    private void UpdateTileStates()
    {
        foreach (SpawnedTileData data in spawnedTiles)
        {
            bool isFree =
                IsTileFree(data);


            data.tile.SetActiveVisual(
                isFree
            );
        }
    }


    // =========================================================
    // CHECK TILE FREE
    // =========================================================

    private bool IsTileFree(
        SpawnedTileData tileData)
    {
        /*
         * Сверху ничего не должно быть.
         */

        if (
            HasTileAbove(tileData)
        )
        {
            return false;
        }


        /*
         * Хотя бы одна сторона
         * должна быть свободна.
         */

        bool leftBlocked =
            HasTileOnSide(
                tileData,
                -1
            );


        bool rightBlocked =
            HasTileOnSide(
                tileData,
                1
            );


        return !leftBlocked ||
               !rightBlocked;
    }


    // =========================================================
    // TILE ABOVE
    // =========================================================

    private bool HasTileAbove(
        SpawnedTileData current)
    {
        foreach (SpawnedTileData other in spawnedTiles)
        {
            if (other == current)
                continue;


            if (
                other.layer <=
                current.layer
            )
            {
                continue;
            }


            if (
                DoTilesOverlap(
                    current.tile,
                    other.tile
                )
            )
            {
                return true;
            }
        }


        return false;
    }


    // =========================================================
    // SIDE TILE
    // =========================================================

    private bool HasTileOnSide(
        SpawnedTileData current,
        int direction)
    {
        foreach (SpawnedTileData other in spawnedTiles)
        {
            if (other == current)
                continue;


            if (
                other.layer !=
                current.layer
            )
            {
                continue;
            }


            if (direction < 0)
            {
                if (
                    other.gridX !=
                    current.gridX - 1
                )
                {
                    continue;
                }
            }
            else
            {
                if (
                    other.gridX !=
                    current.gridX + 1
                )
                {
                    continue;
                }
            }


            if (
                other.gridY !=
                current.gridY
            )
            {
                continue;
            }


            return true;
        }


        return false;
    }


    // =========================================================
    // RECT OVERLAP
    // =========================================================

    private bool DoTilesOverlap(
        MahjongTile first,
        MahjongTile second)
    {
        RectTransform firstRect =
            first.GetComponent<RectTransform>();


        RectTransform secondRect =
            second.GetComponent<RectTransform>();


        Rect firstWorldRect =
            GetWorldRect(firstRect);


        Rect secondWorldRect =
            GetWorldRect(secondRect);


        return firstWorldRect.Overlaps(
            secondWorldRect
        );
    }


    // =========================================================
    // WORLD RECT
    // =========================================================

    private Rect GetWorldRect(
        RectTransform rectTransform)
    {
        Vector3[] corners =
            new Vector3[4];


        rectTransform.GetWorldCorners(
            corners
        );


        float minX =
            corners[0].x;

        float maxX =
            corners[0].x;

        float minY =
            corners[0].y;

        float maxY =
            corners[0].y;


        for (int i = 1; i < 4; i++)
        {
            minX =
                Mathf.Min(
                    minX,
                    corners[i].x
                );


            maxX =
                Mathf.Max(
                    maxX,
                    corners[i].x
                );


            minY =
                Mathf.Min(
                    minY,
                    corners[i].y
                );


            maxY =
                Mathf.Max(
                    maxY,
                    corners[i].y
                );
        }


        return Rect.MinMaxRect(
            minX,
            minY,
            maxX,
            maxY
        );
    }


    // =========================================================
    // CLEAR
    // =========================================================

    private void ClearBoard()
    {
        spawnedTiles.Clear();


        layer0Positions.Clear();

        layer1Positions.Clear();

        layer2Positions.Clear();


        if (boardRect == null)
            return;


        for (
            int i =
                boardRect.childCount - 1;

            i >= 0;

            i--)
        {
            Destroy(
                boardRect.GetChild(i).gameObject
            );
        }
    }
}