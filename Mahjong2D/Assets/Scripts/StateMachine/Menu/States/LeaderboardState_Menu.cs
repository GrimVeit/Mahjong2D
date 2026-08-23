using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public LeaderboardState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_Leader += ChangeStateToMain;

        _sceneRoot.ShowLeaderboardPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_Leader -= ChangeStateToMain;

        _sceneRoot.HideLeaderboardPanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
