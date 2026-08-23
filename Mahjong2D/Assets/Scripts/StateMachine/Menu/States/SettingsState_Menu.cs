using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public SettingsState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_Settings += ChangeStateToMain;

        _sceneRoot.ShowSettingsPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_Settings -= ChangeStateToMain;

        _sceneRoot.HideSettingsPanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
