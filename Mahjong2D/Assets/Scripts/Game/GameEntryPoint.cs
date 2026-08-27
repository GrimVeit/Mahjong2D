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

    private MoneyVisualPresenter _moneyVisualPresenter;

    private MahjongPresenter _mahjongPresenter;
    private MahjongMatchPresenter _mahjongMatchPresenter;
    private MahjongMixPresenter _mahjongMixPresenter;
    private MahjongHintPresenter _mahjongHintPresenter;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _mahjongPresenter.GenerateBoard(sprites);
        }
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        await base.ShutDown();

        _uIRoot?.Dispose();
        _moneyVisualPresenter?.Dispose();
        _mahjongPresenter?.Dispose();
        _mahjongMatchPresenter?.Dispose();
        _mahjongMixPresenter?.Dispose();
        _mahjongHintPresenter?.Dispose();
        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _moneyVisualPresenter = new MoneyVisualPresenter(
            new MoneyVisualModel(
                _storeMoneyPresenter,
                _storeMoneyPresenter
            ),
            _viewContainer.GetView<MoneyVisualView>()
        );

        _mahjongPresenter = new MahjongPresenter(new MahjongModel(mahjongBoardGenerator), _viewContainer.GetView<MahjongView>());
        container.RegisterInstance<IMahjongProvider>(_mahjongPresenter);
        container.RegisterInstance<IMahjongListener>(_mahjongPresenter);
        container.RegisterInstance<IMahjongInfo>(_mahjongPresenter);

        _mahjongMatchPresenter = new MahjongMatchPresenter(new MahjongMatchModel(_mahjongPresenter), _viewContainer.GetView<MahjongMatchView>());
        container.RegisterInstance<IMahjongMatchListener>(_mahjongMatchPresenter);

        _mahjongMixPresenter = new MahjongMixPresenter(new MahjongMixModel(_mahjongPresenter, _mahjongPresenter), _viewContainer.GetView<MahjongMixView>());
        _mahjongHintPresenter = new MahjongHintPresenter(new MahjongHintModel(_mahjongPresenter, _mahjongPresenter), _viewContainer.GetView<MahjongHintView>());

        _stateMachine = new StateMachine_Game(container);

        _uIRoot.Initialize();
        _moneyVisualPresenter.Initialize();
        _mahjongPresenter.Initialize();
        _mahjongMatchPresenter.Initialize();
        _mahjongMixPresenter.Initialize();
        _mahjongHintPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}
