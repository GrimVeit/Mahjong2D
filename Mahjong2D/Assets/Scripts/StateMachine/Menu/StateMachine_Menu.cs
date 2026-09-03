using System;
using System.Collections;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class StateMachine_Menu : IStateProvider
{
    private readonly Dictionary<Type, IState> states = new();

    private IState _currentState;

    public StateMachine_Menu(DIContainer container)
    {
        var root = container.Resolve<UIRoot_Menu>();
        var sessionInfo = container.Resolve<ISessionInfoProvider>();

        states[typeof(HoldOnStartState_Menu)] = new HoldOnStartState_Menu(this, sessionInfo, root);

        states[typeof(CheckSessionState_Menu)] = new CheckSessionState_Menu(this, sessionInfo, container.Resolve<ISessionProvider>(), container.Resolve<IAuthenticationInfoProvider>());

        states[typeof(Registration_NicknameProfileUnputState_Menu)] = new Registration_NicknameProfileUnputState_Menu(this, container.Resolve<IPlayerProfileEventsProvider>(), root);
        states[typeof(RegistrationState_Game)] = new RegistrationState_Game(this, container.Resolve<IPlayerProfileInfoProvider>(), container.Resolve<IAuthenticationProvider>(), container.Resolve<IPlayerDatabaseProvider>(), root);
        states[typeof(Registration_StartMainState_Menu)] = new Registration_StartMainState_Menu(this, container.Resolve<IPlayerDatabaseProvider>(), container.Resolve<IAuthenticationInfoProvider>(), container.Resolve<IPlayerProfileInfoProvider>(), container.Resolve<ILevelInfoProvider>());

        states[typeof(IntroVideoState_Menu)] = new IntroVideoState_Menu(this, root, container.Resolve<IVideoProvider>());
        states[typeof(IntroStartState_Menu)] = new IntroStartState_Menu(this, root, container.Resolve<IAuthenticationInfoProvider>());

        states[typeof(MainState_Menu)] = new MainState_Menu(this, root, container.Resolve<ISceneService>());
        states[typeof(SettingsState_Menu)] = new SettingsState_Menu(this, root);
        states[typeof(WalletState_Menu)] = new WalletState_Menu(this, root);
        states[typeof(LeaderboardState_Menu)] = new LeaderboardState_Menu(this, root);

        states[typeof(StoreChooseTypeState_Menu)] = new StoreChooseTypeState_Menu(this, root, container.Resolve<IBookPageProvider>());
        states[typeof(StoreBackgroundState_Menu)] = new StoreBackgroundState_Menu(this, root);
        states[typeof(StoreCardDesignState_Menu)] = new StoreCardDesignState_Menu(this, root);
    }

    public void Initialize()
    {
        SetState(GetState<HoldOnStartState_Menu>());
    }

    public void Dispose()
    {
        _currentState?.Exit();
        _currentState = null;
    }

    public IState GetState<T>() where T : IState
    {
        return states[typeof(T)];
    }

    public void SetState(IState state)
    {
        _currentState?.Exit();

        _currentState = state;
        _currentState.Enter();
    }
}
