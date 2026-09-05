using System.Threading;
using Cysharp.Threading.Tasks;

public class WinVideoState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ILevelProvider _levelProvider;
    private readonly IMahjongScoreProvider _mahjongScoreProvider;
    private readonly IMahjongRewardProvider _mahjongRewardProvider;
    private readonly ISoundProvider _soundProvider;
    private readonly ISound _soundBackground_1;
    private readonly ISound _soundBackground_2;

    public WinVideoState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ILevelProvider levelProvider, IMahjongScoreProvider mahjongScoreProvider, IMahjongRewardProvider mahjongRewardProvider, ISoundProvider soundProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _levelProvider = levelProvider;
        _mahjongScoreProvider = mahjongScoreProvider;
        _mahjongRewardProvider = mahjongRewardProvider;
        _soundProvider = soundProvider;
        _soundBackground_1 = _soundProvider.GetSound("Background_Music");
        _soundBackground_2 = _soundProvider.GetSound("Background_Vocals");
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundResultPanel();

        _soundBackground_1.SetVolumeEnd(0.1f, 0.2f);
        _soundBackground_2.SetVolumeEnd(0.1f, 0.2f);

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.ShowWinVideoPanel();

        _soundProvider.PlayOneShot("Win_Background");

        await UniTask.Delay(100, cancellationToken: token);

        _soundProvider.PlayOneShot("Win_Wave");
        _soundProvider.PlayOneShot("Win_Pipe");

        await UniTask.Delay(2600, cancellationToken: token);

        _soundProvider.PlayOneShot("Win_Close");

        await UniTask.Delay(200, cancellationToken: token);

        _mahjongScoreProvider.ApplyScore();
        _mahjongRewardProvider.ApplyReward();

        _levelProvider.IncreaseLevel();
        _sceneRoot.HideWinVideoPanel();

        _soundBackground_1.SetVolumeEnd(1f, 0.2f);
        _soundBackground_2.SetVolumeEnd(1f, 0.2f);

        ChangeStateToWin();
    }

    private void ChangeStateToWin()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinState_Game>());
    }
}
