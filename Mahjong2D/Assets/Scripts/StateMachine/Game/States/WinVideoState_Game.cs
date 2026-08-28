using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;

public class WinVideoState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ILevelProvider _levelProvider;

    public WinVideoState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ILevelProvider levelProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _levelProvider = levelProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundResultPanel();

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.ShowWinVideoPanel();

        await UniTask.Delay(2900, cancellationToken: token);

        _levelProvider.IncreaseLevel();
        _sceneRoot.HideWinVideoPanel();

        ChangeStateToWin();
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinState_Game>());
    }
}
