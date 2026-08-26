using UnityEngine;

public class IntroStartState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public IntroStartState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _sceneRoot.OnClickStart_IntroStart += ChangeStateToMain;

        _sceneRoot.ShowIntroStartPanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickStart_IntroStart -= ChangeStateToMain;

        _sceneRoot.HideIntroStartPanel();
        _sceneRoot.HideIntroVideoPanel();
        _sceneRoot.ShowBackgroundPanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
