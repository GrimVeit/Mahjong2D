using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class HoldOnStartState_Menu : AsyncState
{
    private readonly IStateProvider _stateProvider;

    public HoldOnStartState_Menu(IStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);

        ChangeStateToIntro();
    }

    private void ChangeStateToIntro()
    {
        _stateProvider.SetState(_stateProvider.GetState<IntroVideoState_Menu>());
    }
}
