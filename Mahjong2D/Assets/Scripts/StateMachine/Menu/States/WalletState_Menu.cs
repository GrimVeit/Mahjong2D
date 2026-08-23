using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public WalletState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_Wallet += ChangeStateToMain;

        _sceneRoot.ShowWalletPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_Wallet -= ChangeStateToMain;

        _sceneRoot.HideWalletPanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
