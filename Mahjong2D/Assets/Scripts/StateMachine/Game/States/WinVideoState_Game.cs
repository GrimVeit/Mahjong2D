using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;

public class WinVideoState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ILevelProvider _levelProvider;
    private readonly IMahjongScoreProvider _mahjongScoreProvider;
    private readonly IMahjongRewardProvider _mahjongRewardProvider;

    public WinVideoState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ILevelProvider levelProvider, IMahjongScoreProvider mahjongScoreProvider, IMahjongRewardProvider mahjongRewardProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _levelProvider = levelProvider;
        _mahjongScoreProvider = mahjongScoreProvider;
        _mahjongRewardProvider = mahjongRewardProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundResultPanel();

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.ShowWinVideoPanel();

        await UniTask.Delay(2900, cancellationToken: token);

        _mahjongScoreProvider.ApplyScore();
        _mahjongRewardProvider.ApplyReward();

        _levelProvider.IncreaseLevel();
        _sceneRoot.HideWinVideoPanel();

        ChangeStateToWin();
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinState_Game>());
    }
}
