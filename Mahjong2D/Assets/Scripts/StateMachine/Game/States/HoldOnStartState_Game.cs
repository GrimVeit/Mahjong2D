using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class HoldOnStartState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;

    public HoldOnStartState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundPanel();

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

        ChangeStateToMahjongGenerate();
    }

    private void ChangeStateToMahjongGenerate()
    {
        _stateProvider.SetState(_stateProvider.GetState<MahjongGenerateState_Game>());
    }
}
