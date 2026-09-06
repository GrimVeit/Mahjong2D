using System;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class GameApplication : IDisposable
{
    public DIContainer GlobalContainer { get; }

    private UIRootView uiRoot;

    public GameApplication()
    {
        GlobalContainer = new DIContainer();

        RegisterServices();
        RegisterGlobalObjects();
    }


    public async UniTask Start()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);

        await uiRoot.Initialize();

        var sceneService = GlobalContainer.Resolve<ISceneService>();
        await sceneService.ChangeSceneAsync(new SceneTransition(Scenes.StartPrepare, LoadingType.None));
    }


    private void RegisterServices()
    {
        var sceneService = new SceneService(GlobalContainer);
        GlobalContainer.RegisterInstance<ISceneService>(sceneService);

        var storeSessionPresenter = new StoreSessionPresenter(new StoreSessionModel());
        GlobalContainer.RegisterInstance<ISessionInfoProvider>(storeSessionPresenter);
        GlobalContainer.RegisterInstance<ISessionProvider>(storeSessionPresenter);
    }


    private void RegisterGlobalObjects()
    {
        var uiRootPrefab = Resources.Load<UIRootView>("UIRootView");

        if (uiRootPrefab == null)
        {
            throw new InvalidOperationException("UIRootView prefab was not found in Assets/Resources.");
        }

        uiRoot = Object.Instantiate(uiRootPrefab);
        Object.DontDestroyOnLoad(uiRoot.gameObject);
        GlobalContainer.RegisterInstance(uiRoot);
    }


    public void Dispose()
    {
        if (uiRoot != null)
        {
            Object.Destroy(uiRoot.gameObject);
            uiRoot = null;
        }
    }
}