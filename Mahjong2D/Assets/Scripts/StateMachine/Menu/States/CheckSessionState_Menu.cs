using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSessionState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISessionInfoProvider _sessionInfoProvider;
    private readonly ISessionProvider _sessionProvider;

    public CheckSessionState_Menu(IStateProvider stateProvider, ISessionInfoProvider sessionInfoProvider, ISessionProvider sessionProvider)
    {
        _stateProvider = stateProvider;
        _sessionInfoProvider = sessionInfoProvider;
        _sessionProvider = sessionProvider;
    }

    public void Enter()
    {
        if (_sessionInfoProvider.IsFirstLaunch)
        {
            _sessionProvider.CompleteFirstLaunch();

            ChangeStateToIntroVideo();
        }
        else
        {
            ChangeStateToMain();
        }
    }

    public void Exit()
    {

    }

    private void ChangeStateToIntroVideo()
    {
        _stateProvider.SetState(_stateProvider.GetState<IntroVideoState_Menu>());
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
