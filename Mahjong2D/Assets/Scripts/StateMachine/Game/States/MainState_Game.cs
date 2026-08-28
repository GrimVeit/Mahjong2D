using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    private readonly IMahjongMatchListener _mahjongMatchListener;
    private readonly IMahjongInfo _mahjongInfo;

    private int _activeMatches;

    public MainState_Game(
        IStateProvider stateProvider,
        ISceneService sceneService,
        UIRoot_Game sceneRoot,
        IMahjongMatchListener mahjongMatchListener,
        IMahjongInfo mahjongInfo)
    {
        _stateProvider = stateProvider;
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;

        _mahjongMatchListener = mahjongMatchListener;
        _mahjongInfo = mahjongInfo;
    }

    public void Enter()
    {
        _activeMatches = 0;

        _mahjongMatchListener.OnStartMatch += OnStartMatch;
        _mahjongMatchListener.OnEndMatch += OnEndMatch;

        _sceneRoot.OnClickMenu_MainHeader += ChangeSceneToMenu;

        _sceneRoot.ShowMainHeaderPanel();
        _sceneRoot.ShowMainFooterPanel();
    }

    public void Exit()
    {
        _mahjongMatchListener.OnStartMatch -= OnStartMatch;
        _mahjongMatchListener.OnEndMatch -= OnEndMatch;

        _sceneRoot.OnClickMenu_MainHeader -= ChangeSceneToMenu;

        _sceneRoot.HideMainHeaderPanel();
        _sceneRoot.HideMainFooterPanel();
    }

    private void OnStartMatch()
    {
        _activeMatches++;
    }

    private void OnEndMatch()
    {
        _activeMatches--;

        if (_activeMatches < 0)
        {
            _activeMatches = 0;
        }

        CheckMatch();
    }

    private void CheckMatch()
    {
        if (_activeMatches > 0)
        {
            return;
        }

        if (!_mahjongInfo.HasRemainingTiles())
        {
            ChangeSceneToWinVideo();
        }
    }


    private void ChangeSceneToWinVideo()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinVideoState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(
            new SceneTransition(
                Scenes.Menu,
                LoadingType.Default
            )
        );
    }
}
