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

        states[typeof(HoldOnStartState_Game)] = new HoldOnStartState_Game(this, root);
        states[typeof(MahjongGenerateState_Game)] = new MahjongGenerateState_Game(this, container.Resolve<IMahjongProvider>(), container.Resolve<List<Sprite>>("MahjongSprites"), root);
        states[typeof(MainState_Game)] = new MainState_Game(this, container.Resolve<ISceneService>(), root, container.Resolve<IMahjongMatchListener>(), container.Resolve<IMahjongInfo>());
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
