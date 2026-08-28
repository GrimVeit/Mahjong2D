using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState_Game : IState
{
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    public LoseState_Game(ISceneService sceneService, UIRoot_Game sceneRoot)
    {
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickMenu_Lose += ChangeSceneToMenu;
        _sceneRoot.OnClickGame_Lose += ChangeSceneToGame;

        _sceneRoot.ShowLosePanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickMenu_Lose -= ChangeSceneToMenu;
        _sceneRoot.OnClickGame_Lose -= ChangeSceneToGame;

        _sceneRoot.HideLosePanel();
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
