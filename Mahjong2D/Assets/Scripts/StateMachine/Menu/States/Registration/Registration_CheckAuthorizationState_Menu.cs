using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registration_CheckAuthorizationState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IAuthenticationInfoProvider _authenticationInfo;
    private readonly UIRoot_Menu _sceneRoot;

    public Registration_CheckAuthorizationState_Menu(
        IStateProvider stateProvider,
        IAuthenticationInfoProvider authenticationInfo,
        UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _authenticationInfo = authenticationInfo;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        if (_authenticationInfo.IsAuthorized)
        {
            ChangeStateToStartMain();
        }
        else
        {
            ChangeStateToStartRegistration();
        }
    }
    

    public void Exit()
    {
    }

    private void ChangeStateToStartRegistration()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_NicknameProfileUnputState_Menu>());
    }

    private void ChangeStateToStartMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
    }
}
