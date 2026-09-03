using UnityEngine;

public class IntroStartState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;
    private readonly IAuthenticationInfoProvider _authenticationInfoProvider;

    public IntroStartState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot, IAuthenticationInfoProvider authenticationInfoProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _authenticationInfoProvider = authenticationInfoProvider;
    }

    public void Enter()
    {
        _sceneRoot.OnClickStart_IntroStart += EndClick;

        _sceneRoot.ShowIntroStartPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickStart_IntroStart -= EndClick;

        _sceneRoot.HideIntroStartPanel();
        _sceneRoot.HideIntroVideoPanel();
        _sceneRoot.ShowBackgroundPanel();
    }

    private void EndClick()
    {
        if (_authenticationInfoProvider.IsAuthorized)
        {
            ChangeStateToStartMain();
        }
        else
        {
            ChangeStateToRegistration();
        }
    }

    private void ChangeStateToStartMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
    }

    private void ChangeStateToRegistration()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_NicknameProfileUnputState_Menu>());
    }
}
