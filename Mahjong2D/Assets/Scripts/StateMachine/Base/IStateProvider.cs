using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateProvider
{
    public IState GetState<T>() where T : IState;

    public void SetState(IState state);
}
