using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Menu uIRoot;

    private UIRoot_Menu _uIRoot;
    private ViewContainer _viewContainer;

    private VolumeSettingsPresenter _volumeSettingsPresenter;
    private MoneyVisualPresenter _moneyVisualPresenter;

    private VideoPresenter _videoPresenter;

    private BookPagesPresenter _bookPagesPresenter;
    private BookPagesControlPresenter _bookPagesControlPresenter;

    private StateMachine_Menu _stateMachine;

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

        _videoPresenter = new VideoPresenter(new VideoModel(), _viewContainer.GetView<VideoView>());
        container.RegisterInstance<IVideoProvider>(_videoPresenter);
        await _videoPresenter.Initialize();

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

        _volumeSettingsPresenter?.Dispose();
        _moneyVisualPresenter?.Dispose();

        _bookPagesPresenter?.Dispose();
        _bookPagesControlPresenter?.Dispose(); 

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _uIRoot.SetSoundProvider(_soundPresenter);

        _bookPagesPresenter = new BookPagesPresenter(new BookPagesModel(2), _viewContainer.GetView<BookPagesView>());
        container.RegisterInstance<IBookPageEventsProvider>(_bookPagesPresenter);
        container.RegisterInstance<IBookPageInfoProvider>(_bookPagesPresenter);
        container.RegisterInstance<IBookPageProvider>(_bookPagesPresenter);

        _bookPagesControlPresenter = new BookPagesControlPresenter(new BookPagesControlModel(_bookPagesPresenter, _bookPagesPresenter, _bookPagesPresenter), _viewContainer.GetView<BookPagesControlView>());

        _volumeSettingsPresenter = new VolumeSettingsPresenter(new VolumeSettingsModel(_storeSoundSettingsPresenter,_storeSoundSettingsPresenter,_storeSoundSettingsPresenter), _viewContainer.GetView<VolumeSettingsView>());
        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter,_storeMoneyPresenter),_viewContainer.GetView<MoneyVisualView>());

        _stateMachine = new StateMachine_Menu(container);

        _uIRoot.Initialize();
        _volumeSettingsPresenter.Initialize();
        _moneyVisualPresenter.Initialize();

        _bookPagesPresenter.Initialize();
        _bookPagesControlPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}
