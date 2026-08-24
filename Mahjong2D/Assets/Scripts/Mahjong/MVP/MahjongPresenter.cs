using System;
using System.Collections.Generic;
using UnityEngine;

public class MahjongPresenter
{
    private readonly MahjongModel model;
    private readonly MahjongView view;


    public MahjongPresenter(
        MahjongModel model,
        MahjongView view)
    {
        this.model = model;
        this.view = view;
    }


    public void Initialize()
    {
        view.OnClickTile +=
            HandleClickTile;


        model.OnTileCreated +=
            HandleTileCreated;

        model.OnTileRemoved +=
            HandleTileRemoved;

        model.OnTileActiveChanged +=
            HandleTileActiveChanged;

        model.OnTileSelected +=
            HandleTileSelected;

        model.OnTileUnselected +=
            HandleTileUnselected;

        model.OnBoardCleared +=
            HandleBoardCleared;

        model.OnMix +=
            HandleMix;


        view.Initialize();
        model.Initialize();
    }


    public void Dispose()
    {
        view.OnClickTile -=
            HandleClickTile;


        model.OnTileCreated -=
            HandleTileCreated;

        model.OnTileRemoved -=
            HandleTileRemoved;

        model.OnTileActiveChanged -=
            HandleTileActiveChanged;

        model.OnTileSelected -=
            HandleTileSelected;

        model.OnTileUnselected -=
            HandleTileUnselected;

        model.OnBoardCleared -=
            HandleBoardCleared;

        model.OnMix -=
            HandleMix;


        view.Dispose();
        model.Dispose();
    }


    // =========================================================
    // PUBLIC API
    // =========================================================

    public void GenerateBoard(
        List<Sprite> sprites)
    {
        model.GenerateBoard(
            sprites
        );

        view.UpdateDrawingOrder(
        model.Tiles
    );
    }


    public void Mix()
    {
        model.Mix();
    }


    // =========================================================
    // INPUT
    // =========================================================

    private void HandleClickTile(
        int tileId)
    {
        model.SelectTile(
            tileId
        );
    }


    // =========================================================
    // MODEL -> VIEW
    // =========================================================

    private void HandleTileCreated(
        MahjongTileData data)
    {
        view.CreateTile(
            data
        );
    }


    private void HandleTileRemoved(
        int tileId)
    {
        view.RemoveTile(
            tileId
        );
    }


    private void HandleTileActiveChanged(
        int tileId,
        bool isActive)
    {
        view.SetTileActive(
            tileId,
            isActive
        );
    }


    private void HandleTileSelected(
        int tileId)
    {
        view.SelectTile(
            tileId
        );
    }


    private void HandleTileUnselected(
        int tileId)
    {
        view.UnselectTile(
            tileId
        );
    }


    private void HandleBoardCleared()
    {
        view.ClearBoard();
    }


    private void HandleMix()
    {
        view.Mix(
            model.Tiles
        );
    }
}