using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class HoldOnStartState_Menu : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISessionInfoProvider _sessionInfoProvider;
    private readonly UIRoot_Menu _sceneRoot;

    public HoldOnStartState_Menu(IStateProvider stateProvider, ISessionInfoProvider sessionInfoProvider, UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _sessionInfoProvider = sessionInfoProvider;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        if (!_sessionInfoProvider.IsFirstLaunch)
        {
            _sceneRoot.ShowBackgroundPanel();

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);

        ChangeStateToIntro();
    }

    private void ChangeStateToIntro()
    {
        _stateProvider.SetState(_stateProvider.GetState<CheckSessionState_Menu>());
    }
}
