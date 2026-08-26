using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreChooseTypeState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public StoreChooseTypeState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_StoreChooseType += ChangeStateToMain;

        _sceneRoot.ShowStoreChooseTypePanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_StoreChooseType -= ChangeStateToMain;

        _sceneRoot.HideStoreChooseTypePanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
