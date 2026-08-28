using Cysharp.Threading.Tasks;
using System.Threading;

public class LoseVideoState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;

    public LoseVideoState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }


    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundResultPanel();

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.ShowLoseVideoPanel();

        await UniTask.Delay(2900, cancellationToken: token);

        _sceneRoot.HideLoseVideoPanel();

        ChangeStateToLose();
    }

    private void ChangeStateToLose()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseState_Game>());
    }
}
