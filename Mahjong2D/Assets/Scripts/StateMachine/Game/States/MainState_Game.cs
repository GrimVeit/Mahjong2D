public class MainState_Game : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly ISceneService _sceneService;
    private readonly UIRoot_Game _sceneRoot;

    private readonly IMahjongMatchListener _mahjongMatchListener;
    private readonly IMahjongInfo _mahjongInfo;

    private readonly ITimerListener _timerListener;
    private readonly ITimerProvider _timerProvider;

    private readonly ILevelInfoProvider _levelInfoProvider;

    private int _activeMatches;

    public MainState_Game(
        IStateProvider stateProvider,
        ISceneService sceneService,
        UIRoot_Game sceneRoot,
        IMahjongMatchListener mahjongMatchListener,
        IMahjongInfo mahjongInfo,
        ITimerListener timerListener,
        ITimerProvider timerProvider,
        ILevelInfoProvider levelInfoProvider)
    {
        _stateProvider = stateProvider;
        _sceneService = sceneService;
        _sceneRoot = sceneRoot;

        _mahjongMatchListener = mahjongMatchListener;
        _mahjongInfo = mahjongInfo;
        _timerListener = timerListener;
        _timerProvider = timerProvider;
        _levelInfoProvider = levelInfoProvider;
    }

    public void Enter()
    {
        _activeMatches = 0;

        _timerListener.OnStopTimer += ChangeSceneToLoseVideo;
        _mahjongMatchListener.OnStartMatch += OnStartMatch;
        _mahjongMatchListener.OnEndMatch += OnEndMatch;

        _sceneRoot.OnClickMenu_MainHeader += ChangeSceneToMenu;

        _sceneRoot.ShowMainHeaderPanel();
        _sceneRoot.ShowMainFooterPanel();
        _timerProvider.ResetTimer();
        _timerProvider.ActivateTimer(MahjongTimerHelper.GetTime(_levelInfoProvider.Level + 1), TimerDirection.Backward);
    }

    public void Exit()
    {
        _timerListener.OnStopTimer -= ChangeSceneToLoseVideo;
        _mahjongMatchListener.OnStartMatch -= OnStartMatch;
        _mahjongMatchListener.OnEndMatch -= OnEndMatch;

        _sceneRoot.OnClickMenu_MainHeader -= ChangeSceneToMenu;

        _sceneRoot.HideMainHeaderPanel();
        _sceneRoot.HideMainFooterPanel();
        _sceneRoot.HideMainPanel();
        _timerProvider.DeactivateTimer();
    }

    private void OnStartMatch()
    {
        _activeMatches++;
    }

    private void OnEndMatch()
    {
        _activeMatches--;

        if (_activeMatches < 0)
        {
            _activeMatches = 0;
        }

        CheckMatch();
    }

    private void CheckMatch()
    {
        if (_activeMatches > 0)
        {
            return;
        }

        if (!_mahjongInfo.HasRemainingTiles())
        {
            ChangeSceneToWinVideo();
        }
    }


    #region OUTPUT

    private void ChangeSceneToWinVideo()
    {
        _stateProvider.SetState(_stateProvider.GetState<WinVideoState_Game>());
    }

    private void ChangeSceneToLoseVideo()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseVideoState_Game>());
    }

    private void ChangeSceneToMenu()
    {
        _sceneService.ChangeScene(new SceneTransition(Scenes.Menu,LoadingType.Default));
    }

    #endregion
}
