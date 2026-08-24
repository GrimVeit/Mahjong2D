using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MahjongView : View
{
    [Header("Board")]

    [SerializeField]
    private RectTransform boardRoot;

    [SerializeField]
    private MahjongTile tilePrefab;


    [Header("Tile Size")]

    [SerializeField]
    private Vector2 tileSize =
        new Vector2(
            235f,
            290f
        );


    [Header("Layer Offset")]

    [SerializeField]
    private float layerOffsetX = 18f;

    [SerializeField]
    private float layerOffsetY = 22f;


    [Header("Mix")]

    [SerializeField]
    private float mixDuration = 0.35f;

    [SerializeField]
    private Ease mixEase =
        Ease.InOutQuad;


    private readonly Dictionary<int, MahjongTile> tileViews =
        new Dictionary<int, MahjongTile>();


    // =========================================================
    // LIFETIME
    // =========================================================

    public void Initialize()
    {
    }


    public void Dispose()
    {
        foreach (MahjongTile tile in tileViews.Values)
        {
            if (tile == null)
                continue;

            tile.OnClick -= HandleTileClick;
        }

        tileViews.Clear();
    }


    // =========================================================
    // CREATE
    // =========================================================

    public void CreateTile(
        MahjongTileData data)
    {
        MahjongTile tile =
            Instantiate(
                tilePrefab,
                boardRoot
            );

        tile.Initialize(
            data.Id
        );

        tile.OnClick +=
            HandleTileClick;

        tileViews.Add(
            data.Id,
            tile
        );


        RectTransform rect =
            tile.transform as RectTransform;

        rect.anchoredPosition =
            CalculatePosition(data);


        tile.SetActiveVisual(
            data.IsActive
        );
    }


    // =========================================================
    // REMOVE
    // =========================================================

    public void RemoveTile(
        int tileId)
    {
        if (
            !tileViews.TryGetValue(
                tileId,
                out MahjongTile tile
            )
        )
        {
            return;
        }


        tile.OnClick -=
            HandleTileClick;

        tileViews.Remove(
            tileId
        );

        Destroy(
            tile.gameObject
        );
    }


    // =========================================================
    // CLEAR
    // =========================================================

    public void ClearBoard()
    {
        foreach (
            MahjongTile tile
            in tileViews.Values)
        {
            if (tile == null)
                continue;


            tile.OnClick -=
                HandleTileClick;


            Destroy(
                tile.gameObject
            );
        }


        tileViews.Clear();
    }


    // =========================================================
    // ACTIVE
    // =========================================================

    public void SetTileActive(
        int tileId,
        bool isActive)
    {
        if (
            !tileViews.TryGetValue(
                tileId,
                out MahjongTile tile
            )
        )
        {
            return;
        }


        tile.SetActiveVisual(
            isActive
        );
    }


    // =========================================================
    // MIX
    // =========================================================

    public void Mix(
        IReadOnlyList<MahjongTileData> tiles)
    {
        UpdateDrawingOrder(
            tiles
        );


        foreach (
            MahjongTileData data
            in tiles)
        {
            if (data.IsRemoved)
                continue;


            if (
                !tileViews.TryGetValue(
                    data.Id,
                    out MahjongTile tile
                )
            )
            {
                continue;
            }


            RectTransform rect =
                tile.transform as RectTransform;


            Vector2 targetPosition =
                CalculatePosition(
                    data
                );


            rect.DOKill();


            rect
                .DOAnchorPos(
                    targetPosition,
                    mixDuration
                )
                .SetEase(
                    mixEase
                );
        }
    }


    // =========================================================
    // DRAWING ORDER
    // =========================================================

    private void UpdateDrawingOrder(
        IReadOnlyList<MahjongTileData> tiles)
    {
        List<MahjongTileData> sortedTiles =
            new List<MahjongTileData>();


        foreach (
            MahjongTileData tile
            in tiles)
        {
            if (tile.IsRemoved)
                continue;


            sortedTiles.Add(
                tile
            );
        }


        /*
         * Ќижние €русы должны быть раньше
         * в Canvas hierarchy.
         *
         * „ем больше Layer Ч
         * тем позже объект будет в hierarchy
         * и тем выше он будет отрисован.
         */

        sortedTiles.Sort(
            CompareTilesDrawingOrder
        );


        foreach (
            MahjongTileData data
            in sortedTiles)
        {
            if (
                !tileViews.TryGetValue(
                    data.Id,
                    out MahjongTile tile
                )
            )
            {
                continue;
            }


            tile.transform.SetAsLastSibling();
        }
    }


    private int CompareTilesDrawingOrder(
        MahjongTileData first,
        MahjongTileData second)
    {
        int layerCompare =
            first.Layer.CompareTo(
                second.Layer
            );


        if (layerCompare != 0)
            return layerCompare;


        /*
         * ¬нутри одного €руса
         * сохран€ем стабильный пор€док
         * по Y, затем по X.
         */

        int yCompare =
            first.GridY.CompareTo(
                second.GridY
            );


        if (yCompare != 0)
            return yCompare;


        return
            first.GridX.CompareTo(
                second.GridX
            );
    }


    // =========================================================
    // POSITION
    // =========================================================

    private Vector2 CalculatePosition(
        MahjongTileData data)
    {
        /*
         * Layer 1 + Layer 2
         */

        if (data.Layer < 2)
        {
            float x =
                data.GridX *
                tileSize.x;

            float y =
                data.GridY *
                tileSize.y;


            float boardWidth =
                4f *
                tileSize.x;

            float boardHeight =
                5f *
                tileSize.y;


            x -=
                (boardWidth - tileSize.x) / 2f;

            y -=
                (boardHeight - tileSize.y) / 2f;


            x +=
                data.Layer *
                layerOffsetX;

            y +=
                data.Layer *
                layerOffsetY;


            return new Vector2(
                x,
                y
            );
        }


        /*
         * Layer 3
         *
         * Ќаходитс€ между тайлами
         * второго €руса.
         */

        float leftX =
            data.GridX *
            tileSize.x;

        float rightX =
            (data.GridX + 1) *
            tileSize.x;


        float middleX =
            (leftX + rightX) / 2f;


        float boardWidthThird =
            4f *
            tileSize.x;


        middleX -=
            (boardWidthThird - tileSize.x) / 2f;


        float xPosition =
            middleX +
            2f *
            layerOffsetX;


        float yPosition =
            data.GridY *
            tileSize.y;


        float boardHeightThird =
            5f *
            tileSize.y;


        yPosition -=
            (boardHeightThird - tileSize.y) / 2f;


        yPosition +=
            2f *
            layerOffsetY;


        return new Vector2(
            xPosition,
            yPosition
        );
    }


    // =========================================================
    // INPUT
    // =========================================================

    private void HandleTileClick(
        int tileId)
    {
        OnClickTile?.Invoke(
            tileId
        );
    }


    // =========================================================
    // OUTPUT
    // =========================================================

    public event Action<int>
        OnClickTile;
}