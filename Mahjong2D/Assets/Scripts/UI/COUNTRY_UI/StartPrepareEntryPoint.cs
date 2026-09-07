using BaCon;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class StartPrepareEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Prepare uIRoot;

    private ViewContainer _viewContainer;
    private UIRoot_Prepare _uIRoot;
    private ISceneService _sceneService;
    private FirebaseDatabasePresenter _firebaseDatabasePresenter;
    private GeoLocationPresenter _geoLocationPresenter;
    private URLResolverPresenter _urlResolverPresenter;
    private InternetPresenter _internetPresenter;
    private WebViewPresenter _webViewPresenter;

    private readonly string Nickname = "Tester902";

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        _sceneService = container.Resolve<ISceneService>();
        await _sceneService.ShowLoading(LoadingType.Start);

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

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();

        _webViewPresenter.Dispose();

        await base.ShutDown();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {

        //-----------------------FIREBASE---------------------//

        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
        FirebaseDatabase database = FirebaseDatabase.DefaultInstance;

        _firebaseDatabasePresenter = new FirebaseDatabasePresenter(new FirebaseDatabaseModel(database));
        container.RegisterInstance<IDatabaseProvider>(_firebaseDatabasePresenter);

        //-----------------------------------------------------//

        _geoLocationPresenter = new GeoLocationPresenter(new GeoLocationModel());

        _urlResolverPresenter = new URLResolverPresenter(new UrlResolverModel());

        _internetPresenter = new InternetPresenter(new InternetModel());

        _webViewPresenter = new WebViewPresenter(new WebViewModel(), _viewContainer.GetView<WebViewView>());

        _webViewPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override async UniTask OnSceneInitialized(DIContainer container)
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        Debug.LogWarning("Ïğîâåğêà èíòåğíåòà");

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        }

        //NETWORK

        if (!_internetPresenter.HasNetwork)
        {
            Debug.LogError("ÍÅÒ ÈÍÒÅĞÍÅÒÀ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None), 1000).Forget();
            return;
        }

        //FIRST PERSON

        Debug.LogWarning("Çàïğîñ çà ïåğâûì èãğîêîì");

        (DatabaseResult resultPlayerByPlace, PlayerData playerData) = await _firebaseDatabasePresenter.GetPlayerByPlace(1);

        if(resultPlayerByPlace != DatabaseResult.Success || playerData == null)
        {
            Debug.LogError("ÍÅÓÄÀ×ÍÛÉ ÇÀÏĞÎÑ ÇÀ ÏÅĞÂÛÌ ×ÅËÎÂÅÊÎÌ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if(string.IsNullOrEmpty(playerData.Nickname) || playerData.Nickname != Nickname)
        {
            Debug.LogError("ÍÅ ÒÎÒ ×ÅËÎÂÅÊ ÍÀ ÏÅĞÂÎÌ ÌÅÑÒÅ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }




        //

        Debug.LogWarning("Çàïğîñ çà ãåî èãğîêà");

        (GeoLocationResult resultPlayerIp, IPInfo ipInfo) = await _geoLocationPresenter.GetUserLocation();

        if(resultPlayerIp != GeoLocationResult.Success || ipInfo == null)
        {
            Debug.LogError("ÍÅÓÄÀ×ÍÛÉ ÇÀÏĞÎÑ ÇÀ ÃÅÎ ÈÃĞÎÊÀ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (string.IsNullOrEmpty(ipInfo.country))
        {
            Debug.LogError("ÍÅÒ ÑÒĞÀÍÛ Â ÄÀÍÍÛÕ ÃÅÎ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }


        //GEO_LINK_NICKNAME

        Debug.LogWarning("Çàïğîñ çà äàííûìè ñåğûìè");

        (DatabaseResult resultLinkGeo, LinkGeoData linkGeoData) = await _firebaseDatabasePresenter.GetLinkGeoData();

        if(resultLinkGeo != DatabaseResult.Success || linkGeoData == null)
        {
            Debug.LogError("ÍÅÓÄÀ×ÍÛÉ ÇÀÏĞÎÑ ÇÀ ÑÅĞÛÌÈ ÄÀÍÍÛÌÈ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (string.IsNullOrEmpty(linkGeoData.Link) || linkGeoData.Geo == null)
        {
            Debug.LogError("ÍÅÒ Â ÄÀÍÍÛÕ ÈËÈ ÑÑÛËÊÈ ÈËÈ ÃÅÎ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }

        if (!linkGeoData.Geo.Contains(ipInfo.country))
        {
            Debug.LogError("ÍÅ ÒÎ ÃÅÎ");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }



        //UniWebView


        Debug.Log(linkGeoData.Link.ToString());
        (UrlResolverResult resultUrlResorve, string Url) = await _urlResolverPresenter.ResolveFromTitle(linkGeoData.Link);

        if (resultUrlResorve != UrlResolverResult.Success || string.IsNullOrEmpty(Url))
        {
            Debug.LogError("ÍÅÓÄÀ×ÍÛÉ ÇÀÏĞÎÑ ÇÀ ÑÑÛËÊÎÉ Â ÒÀÉÒË");
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            return;
        }
        
        _webViewPresenter.SetURL("https://google.com");
        _webViewPresenter.Load();
        _webViewPresenter.Show();
    }

    private async UniTask StartAutoTransition(SceneTransition transition, int time = 50)
    {
        Debug.Log(time);

        await UniTask.Delay(time);

        await _sceneService.HideLoading(LoadingType.Start);

        await _sceneService.ChangeSceneAsync(transition);
    }
}
