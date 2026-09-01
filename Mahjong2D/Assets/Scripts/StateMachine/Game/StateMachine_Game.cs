using System;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class StateMachine_Game : IStateProvider
{
    private readonly Dictionary<Type, IState> states = new();

    private IState _currentState;

    public StateMachine_Game(DIContainer container)
    {
        var root = container.Resolve<UIRoot_Game>();
        var sceneService = container.Resolve<ISceneService>();
        var levelInfo = container.Resolve<ILevelInfoProvider>();

        states[typeof(HoldOnStartState_Game)] = new HoldOnStartState_Game(this, root);
        states[typeof(MahjongGenerateState_Game)] = new MahjongGenerateState_Game(this, container.Resolve<IMahjongProvider>(), root, levelInfo, container.Resolve<IMahjongTilesSpritesProvider>(), container.Resolve<ICardDesignInfoProvider>());
        states[typeof(MainState_Game)] = new MainState_Game(this, sceneService, root, container.Resolve<IMahjongMatchListener>(), container.Resolve<IMahjongInfo>(), container.Resolve<ITimerListener>(), container.Resolve<ITimerProvider>(), levelInfo);

        states[typeof(WinVideoState_Game)] = new WinVideoState_Game(this, root, container.Resolve<ILevelProvider>());
        states[typeof(WinState_Game)] = new WinState_Game(sceneService, root);

        states[typeof(LoseVideoState_Game)] = new LoseVideoState_Game(this, root);
        states[typeof(LoseState_Game)] = new LoseState_Game(sceneService, root);
    }

    public void Initialize()
    {
        SetState(GetState<HoldOnStartState_Game>());
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
