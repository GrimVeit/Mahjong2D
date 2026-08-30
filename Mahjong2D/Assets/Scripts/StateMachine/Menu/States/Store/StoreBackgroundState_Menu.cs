using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBackgroundState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public StoreBackgroundState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_StoreBackground += ChangeStateToStoreChooseType;

        _sceneRoot.ShowStoreBackgroundPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_StoreBackground -= ChangeStateToStoreChooseType;

        _sceneRoot.HideStoreBackgroundPanel();
    }

    private void ChangeStateToStoreChooseType()
    {
        _stateProvider.SetState(_stateProvider.GetState<StoreChooseTypeState_Menu>());
    }
}
