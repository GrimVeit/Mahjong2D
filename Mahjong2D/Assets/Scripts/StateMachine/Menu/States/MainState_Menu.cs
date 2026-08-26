using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public MainState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickSettings_Main += ChangeStateToSettings;
        _sceneRoot.OnClickWallet_Main += ChangeStateToWallet;
        _sceneRoot.OnClickLeaderboard_Main += ChangeStateToLeaderboard;
        _sceneRoot.OnClickStore_Main += ChangeStateToStoreChooseType;

        _sceneRoot.ShowMainPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickSettings_Main -= ChangeStateToSettings;
        _sceneRoot.OnClickWallet_Main -= ChangeStateToWallet;
        _sceneRoot.OnClickLeaderboard_Main -= ChangeStateToLeaderboard;
        _sceneRoot.OnClickStore_Main -= ChangeStateToStoreChooseType;

        _sceneRoot.HideMainPanel();
    }

    private void ChangeStateToSettings()
    {
        _stateProvider.SetState(_stateProvider.GetState<SettingsState_Menu>());
    }

    private void ChangeStateToWallet()
    {
        _stateProvider.SetState(_stateProvider.GetState<WalletState_Menu>());
    }

    private void ChangeStateToLeaderboard()
    {
        _stateProvider.SetState(_stateProvider.GetState<LeaderboardState_Menu>());
    }

    private void ChangeStateToStoreChooseType()
    {
        _stateProvider.SetState(_stateProvider.GetState<StoreChooseTypeState_Menu>());
    }
}
