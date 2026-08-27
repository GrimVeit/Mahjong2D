using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MahjongPresenter :
    IMahjongListener,
    IMahjongProvider,
    IMahjongInfo
{
    private readonly MahjongModel model;
    private readonly MahjongView view;


    private const float GenerateTileDelay = 0.05f;


    public MahjongPresenter(
        MahjongModel model,
        MahjongView view)
    {
        this.model = model;
        this.view = view;
    }


    // =========================================================
    // LIFETIME
    // =========================================================

    public void Initialize()
    {
        view.OnClickTile +=
            HandleClickTile;


        model.OnTileCreated +=
            HandleTileCreated;

        model.OnTileRemoved +=
            HandleTileRemoved;

        model.OnTileHintSelected +=
            view.HintTile;

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

        model.OnStartGenerate +=
            HandleStartGenerate;

        model.OnStopGenerate +=
            HandleStopGenerate;


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

        model.OnTileHintSelected -=
            view.HintTile;

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

        model.OnStartGenerate -=
            HandleStartGenerate;

        model.OnStopGenerate -=
            HandleStopGenerate;


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


    private void HandleStartGenerate()
    {
        
    }


    private void HandleStopGenerate()
    {
        view.UpdateDrawingOrder(
            model.Tiles
        );
    }


    // =========================================================
    // INFO
    // =========================================================

    public bool HasAvailableMoves() =>
        model.HasAvailableMoves();


    public bool HasRemainingTiles() =>
        model.HasRemainingTiles();


    // =========================================================
    // OUTPUT
    // =========================================================

    public event Action<MahjongPairRemovedData> OnPairRemoved
    {
        add => view.OnPairRemoved += value;
        remove => view.OnPairRemoved -= value;
    }


    public event Action OnStartHint
    {
        add => view.OnStartHint += value;
        remove => view.OnStartHint -= value;
    }


    public event Action OnStopHint
    {
        add => view.OnStopHint += value;
        remove => view.OnStopHint -= value;
    }


    public event Action OnStartMix
    {
        add => view.OnStartMix += value;
        remove => view.OnStartMix -= value;
    }


    public event Action OnStopMix
    {
        add => view.OnStopMix += value;
        remove => view.OnStopMix -= value;
    }


    public event Action OnStartGenerate
    {
        add => model.OnStartGenerate += value;
        remove => model.OnStartGenerate -= value;
    }


    public event Action OnStopGenerate
    {
        add => model.OnStopGenerate += value;
        remove => model.OnStopGenerate -= value;
    }


    // =========================================================
    // PROVIDER
    // =========================================================

    public UniTask GenerateBoard(
        List<Sprite> sprites)
    {
        return model.GenerateBoard(
            sprites,
            GenerateTileDelay
        );
    }


    public void Mix()
    {
        model.Mix();
    }


    public void Hint()
    {
        model.Hint();
    }
}

public interface IMahjongInfo
{
    public bool HasAvailableMoves();
    public bool HasRemainingTiles();
}

public interface IMahjongProvider
{
    public UniTask GenerateBoard(List<Sprite> sprites);
    public void Mix();
    public void Hint();
}

public interface IMahjongListener
{
    public event Action<MahjongPairRemovedData> OnPairRemoved;


    public event Action OnStartHint;
    public event Action OnStopHint;


    public event Action OnStartMix;
    public event Action OnStopMix;


    public event Action OnStartGenerate;
    public event Action OnStopGenerate;
}