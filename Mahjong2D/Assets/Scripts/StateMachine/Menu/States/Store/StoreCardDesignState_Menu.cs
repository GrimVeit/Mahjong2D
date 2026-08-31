using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCardDesignState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public StoreCardDesignState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_StoreDesign += ChangeStateToStoreChooseType;

        _sceneRoot.ShowStoreCardDesignPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_StoreDesign -= ChangeStateToStoreChooseType;

        _sceneRoot.HideStoreCardDesignPanel();
    }

    private void ChangeStateToStoreChooseType()
    {
        _stateProvider.SetState(_stateProvider.GetState<StoreChooseTypeState_Menu>());
    }
}
