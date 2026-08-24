using System;

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

    public void GenerateBoard()
    {
        model.GenerateBoard();
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