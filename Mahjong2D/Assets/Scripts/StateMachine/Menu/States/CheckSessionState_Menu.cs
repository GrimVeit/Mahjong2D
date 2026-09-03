using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSessionState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISessionInfoProvider _sessionInfoProvider;
    private readonly ISessionProvider _sessionProvider;
    private readonly IAuthenticationInfoProvider _authenticationInfoProvider;

    public CheckSessionState_Menu(IStateProvider stateProvider, ISessionInfoProvider sessionInfoProvider, ISessionProvider sessionProvider, IAuthenticationInfoProvider authenticationInfoProvider)
    {
        _stateProvider = stateProvider;
        _sessionInfoProvider = sessionInfoProvider;
        _sessionProvider = sessionProvider;
        _authenticationInfoProvider = authenticationInfoProvider;
    }

    public void Enter()
    {
        if (_sessionInfoProvider.IsFirstLaunch || !_authenticationInfoProvider.IsAuthorized)
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
        _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
    }
}
