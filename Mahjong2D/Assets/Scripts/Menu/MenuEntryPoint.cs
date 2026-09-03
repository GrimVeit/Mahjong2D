using BaCon;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class MenuEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Menu uIRoot;

    private UIRoot_Menu _uIRoot;
    private ViewContainer _viewContainer;

    private FirebaseAuthenticationPresenter _firebaseAuthenticationPresenter;
    private FirebaseDatabasePresenter _firebaseDatabasePresenter;

    private AuthenticationDescriptionPresenter _authenticationDescriptionPresenter;

    private ProfileNicknameInputPresenter _profileNicknameInputPresenter;

    private VolumeSettingsPresenter _volumeSettingsPresenter;
    private MoneyVisualPresenter _moneyVisualPresenter;

    private VideoPresenter _videoPresenter;

    private BookPagesPresenter _bookPagesPresenter;
    private BookPagesControlPresenter _bookPagesControlPresenter;

    private BackgroundShopVisualPresenter _backgroundShopVisualPresenter;
    private CardDesignShopVisualPresenter _cardDesignShopVisualPresenter;

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

        _authenticationDescriptionPresenter?.Dispose();
        _profileNicknameInputPresenter?.Dispose();
        _bookPagesPresenter?.Dispose();
        _bookPagesControlPresenter?.Dispose(); 

        _backgroundShopVisualPresenter?.Dispose();
        _cardDesignShopVisualPresenter?.Dispose();

        _stateMachine?.Dispose();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
        FirebaseDatabase database = FirebaseDatabase.DefaultInstance;

        _firebaseAuthenticationPresenter = new FirebaseAuthenticationPresenter(new FirebaseAuthenticationModel(firebaseAuth));
        container.RegisterInstance<IAuthenticationInfoProvider>(_firebaseAuthenticationPresenter);
        container.RegisterInstance<IAuthenticationProvider>(_firebaseAuthenticationPresenter);

        _firebaseDatabasePresenter = new FirebaseDatabasePresenter(new FirebaseDatabaseModel(database, _firebaseAuthenticationPresenter));
        container.RegisterInstance<IPlayerDatabaseProvider>(_firebaseDatabasePresenter);


        _authenticationDescriptionPresenter = new AuthenticationDescriptionPresenter(new AuthenticationDescriptionModel(_firebaseAuthenticationPresenter), _viewContainer.GetView<AuthenticationDescriptionView>());

        _profileNicknameInputPresenter = new ProfileNicknameInputPresenter(new ProfileNicknameInputModel(_storePlayerProfilePresenter), _viewContainer.GetView<ProfileNicknameInputView>());

        _uIRoot.SetSoundProvider(_soundPresenter);

        _bookPagesPresenter = new BookPagesPresenter(new BookPagesModel(2), _viewContainer.GetView<BookPagesView>());
        container.RegisterInstance<IBookPageEventsProvider>(_bookPagesPresenter);
        container.RegisterInstance<IBookPageInfoProvider>(_bookPagesPresenter);
        container.RegisterInstance<IBookPageProvider>(_bookPagesPresenter);
        _bookPagesControlPresenter = new BookPagesControlPresenter(new BookPagesControlModel(_bookPagesPresenter, _bookPagesPresenter, _bookPagesPresenter), _viewContainer.GetView<BookPagesControlView>());

        _backgroundShopVisualPresenter = new BackgroundShopVisualPresenter(new BackgroundShopVisualModel(_storeBackgroundPresenter, _storeBackgroundPresenter, _storeBackgroundPresenter, _storeMoneyPresenter), _viewContainer.GetView<BackgroundShopVisualView>());
        _cardDesignShopVisualPresenter = new CardDesignShopVisualPresenter(new CardDesignShopVisualModel(_storeCardsDesignPresenter, _storeCardsDesignPresenter, _storeCardsDesignPresenter, _storeMoneyPresenter), _viewContainer.GetView<CardDesignShopVisualView>());

        _volumeSettingsPresenter = new VolumeSettingsPresenter(new VolumeSettingsModel(_storeSoundSettingsPresenter,_storeSoundSettingsPresenter,_storeSoundSettingsPresenter), _viewContainer.GetView<VolumeSettingsView>());
        _moneyVisualPresenter = new MoneyVisualPresenter(new MoneyVisualModel(_storeMoneyPresenter,_storeMoneyPresenter),_viewContainer.GetView<MoneyVisualView>());

        _stateMachine = new StateMachine_Menu(container);

        _uIRoot.Initialize();
        _volumeSettingsPresenter.Initialize();
        _moneyVisualPresenter.Initialize();

        _authenticationDescriptionPresenter.Initialize();
        _profileNicknameInputPresenter.Initialize();
        _bookPagesPresenter.Initialize();
        _bookPagesControlPresenter.Initialize();

        _backgroundShopVisualPresenter.Initialize();
        _cardDesignShopVisualPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        _stateMachine.Initialize();

        return UniTask.CompletedTask;
    }
}
