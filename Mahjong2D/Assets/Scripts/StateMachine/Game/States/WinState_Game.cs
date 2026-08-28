using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState_Game : IState
{
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    public WinState_Game(ISceneService sceneService, UIRoot_Game sceneRoot)
    {
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickMenu_Win += ChangeSceneToMenu;
        _sceneRoot.OnClickGame_Win += ChangeSceneToGame;

        _sceneRoot.ShowWinPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickMenu_Win -= ChangeSceneToMenu;
        _sceneRoot.OnClickGame_Win -= ChangeSceneToGame;

        _sceneRoot.HideWinPanel();
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }

    private void ChangeSceneToGame()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Game, LoadingType.Default));
    }
}
