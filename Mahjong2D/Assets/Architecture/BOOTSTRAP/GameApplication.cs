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

        var sceneService = GlobalContainer.Resolve<SceneService>();

        await sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Default));
    }


    private void RegisterServices()
    {
        GlobalContainer.RegisterFactory(container => new SceneService(container)).AsSingle();
    }


    private void RegisterGlobalObjects()
    {
        var uiRootPrefab = Resources.Load<UIRootView>("UIRootView");

        if (uiRootPrefab == null)
        {
            throw new InvalidOperationException(
                "UIRootView prefab was not found in Assets/Resources."
            );
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