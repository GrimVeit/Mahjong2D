using System;
using System.Collections.Generic;
using UnityEngine;

public class MahjongPresenter : IMahjongListener, IMahjongProvider, IMahjongInfo
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

        model.OnTileHintSelected += view.HintTile;

        model.OnPairTileRemoved +=
            HandlePairRemoved;

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

        model.OnTileHintSelected -= view.HintTile;

        model.OnPairTileRemoved -=
            HandlePairRemoved;

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

    private void HandlePairRemoved(
        int tileIdFirst,
        int tileIdSecond)
    {
        view.RemovePair(
            tileIdFirst, 
            tileIdSecond
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

    #region Info

    public bool HasAvailableMoves() => model.HasAvailableMoves();
    public bool HasRemainingTiles() => model.HasRemainingTiles();

    #endregion

    #region Output

    public event Action<MahjongPairRemovedData> OnPairRemoved
    {
        add => view.OnPairRemoved += value;
        remove => view.OnPairRemoved -= value;
    }

    #endregion

    #region Input

    public void GenerateBoard(List<Sprite> sprites)
    {
        model.GenerateBoard(sprites);

        view.UpdateDrawingOrder(model.Tiles);
    }


    public void Mix()
    {
        model.Mix();
    }

    public void Hint()
    {
        model.Hint();
    }

    #endregion
}

public interface IMahjongInfo
{
    public bool HasAvailableMoves();
    public bool HasRemainingTiles();
}

public interface IMahjongProvider
{
    public void GenerateBoard(List<Sprite> sprites);
    public void Mix();
    public void Hint();
}

public interface IMahjongListener
{
    public event Action<MahjongPairRemovedData> OnPairRemoved;
}