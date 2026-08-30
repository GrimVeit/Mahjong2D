using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Game uIRoot;

    [Header("Generator")]
    [SerializeField] private MahjongBoardGenerator mahjongBoardGenerator;
    [SerializeField] private List<Sprite> sprites = new();

    private UIRoot_Game _uIRoot;
    private ViewContainer _viewContainer;

    private StoreMovesPresenter _storeMovesPresenter;
    private MovesVisualPresenter _movesVisualPresenter;

    private MoneyVisualPresenter _moneyVisualPresenter;

    private MahjongPresenter _mahjongPresenter;
    private MahjongMatchPresenter _mahjongMatchPresenter;
    private MahjongMixPresenter _mahjongMixPresenter;
    private MahjongHintPresenter _mahjongHintPresenter;

    private TimerPresenter _timerPresenter_Game;
    private TimerVisualPresenter _timerVisualPresenter_Game;

    private LevelVisualPresenter _levelVisualPresenter;

    private StateMachine_Game _stateMachine;

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        _uIRoot = Instantiate(uIRoot);
        container.RegisterInstance(_uIRoot);

        var uiRootView = container.Resolve<UIRootView>();
        uiRootView.AttachSceneUI(
            _uIRoot.gameObject,
            Camera.main
        );

        _viewContainer = _uIRoot.GetComponent<ViewContainer>();
        _viewContainer.Initialize();
        container.RegisterInstance(_viewContainer);

        container.RegisterInstance("MahjongSprites", sprites);

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        _uIRoot.Dispose();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        await base.ShutDown();

        _storeMovesPresenter?.Dispose();
        _movesVisualPresenter?.Dispose();
        _uIRoot?.Dispose();
        _moneyVisualPresenter?.Dispose();
        _mahjongPresenter?.Dispose();
        _mahjongMatchPresenter?.Dispose();
        _mahjongMixPresenter?.Dispose();
        _mahjongHintPresenter?.Dispose();

        _timerPresenter_Game?.Dispose();
        _timerVisualPresenter_Game?.Dispose();

        _levelVisualPresenter?.Dispose();

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _storeMovesPresenter = new StoreMovesPresenter(new StoreMovesModel());

        container.RegisterInstance<IMovesEventsProvider>(_storeMovesPresenter);
        container.RegisterInstance<IMovesInfoProvider>(_storeMovesPresenter);
        container.RegisterInstance<IMovesProvider>(_storeMovesPresenter);

        _movesVisualPresenter = new MovesVisualPresenter(new MovesVisualModel(_storeMovesPresenter), _viewContainer.GetView<MovesVisualView>());

        _moneyVisualPresenter = new MoneyVisualPresenter(
            new MoneyVisualModel(
                _storeMoneyPresenter,
                _storeMoneyPresenter
            ),
            _viewContainer.GetView<MoneyVisualView>()
        );

        _mahjongPresenter = new MahjongPresenter(new MahjongModel(mahjongBoardGenerator, _storeMovesPresenter), _viewContainer.GetView<MahjongView>());
        container.RegisterInstance<IMahjongProvider>(_mahjongPresenter);
        container.RegisterInstance<IMahjongListener>(_mahjongPresenter);
        container.RegisterInstance<IMahjongInfo>(_mahjongPresenter);

        _mahjongMatchPresenter = new MahjongMatchPresenter(new MahjongMatchModel(_mahjongPresenter), _viewContainer.GetView<MahjongMatchView>());
        container.RegisterInstance<IMahjongMatchListener>(_mahjongMatchPresenter);

        _mahjongMixPresenter = new MahjongMixPresenter(new MahjongMixModel(_mahjongPresenter, _mahjongPresenter), _viewContainer.GetView<MahjongMixView>());
        _mahjongHintPresenter = new MahjongHintPresenter(new MahjongHintModel(_mahjongPresenter, _mahjongPresenter), _viewContainer.GetView<MahjongHintView>());

        _timerPresenter_Game = new TimerPresenter(new TimerModel());
        container.RegisterInstance<ITimerInfo>(_timerPresenter_Game);
        container.RegisterInstance<ITimerListener>(_timerPresenter_Game);
        container.RegisterInstance<ITimerProvider>(_timerPresenter_Game);

        _timerVisualPresenter_Game = new TimerVisualPresenter(new TimerVisualModel(_timerPresenter_Game, _timerPresenter_Game), _viewContainer.GetView<TimerVisualView_CurrentAndElapsedTime>());

        _levelVisualPresenter = new LevelVisualPresenter(new LevelVisualModel(_storeLevelPresenter), _viewContainer.GetView<LevelVisualView>());

        _stateMachine = new StateMachine_Game(container);

        _storeMovesPresenter.Initialize();
        _movesVisualPresenter.Initialize();
        _uIRoot.Initialize();
        _moneyVisualPresenter.Initialize();
        _mahjongPresenter.Initialize();
        _mahjongMatchPresenter.Initialize();
        _mahjongMixPresenter.Initialize();
        _mahjongHintPresenter.Initialize();

        _timerPresenter_Game.Initialize();
        _timerVisualPresenter_Game.Initialize();

        _levelVisualPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}
