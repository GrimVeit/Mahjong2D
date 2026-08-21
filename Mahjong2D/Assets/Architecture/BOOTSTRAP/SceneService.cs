using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService
{
    private ISceneEntry currentScene;
    private bool isTransitioning;

    private readonly DIContainer globalContainer;

    public SceneService(DIContainer container)
    {
        globalContainer = container;
    }

    public async UniTask ChangeScene(SceneTransition transition)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("A scene transition is already in progress.");
        }

        isTransitioning = true;

        try
        {
            await UnloadCurrentScene();
            await ShowLoading(transition.Loading);
            await LoadScene(transition.SceneName);
            await CreateScene();
        }
        finally
        {
            await HideLoading(transition.Loading);
            isTransitioning = false;
        }
    }


    private async UniTask LoadScene(string scene)
    {
        await SceneManager.LoadSceneAsync(scene).ToUniTask();
    }


    private async UniTask CreateScene()
    {
        var currentSceneContainer = new DIContainer(globalContainer);

        var controllers = Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        var sceneControllers = controllers.OfType<ISceneEntry>().ToArray();

        if (sceneControllers.Length != 1)
        {
            throw new System.InvalidOperationException(
                $"Scene must contain exactly one {nameof(ISceneEntry)}; found {sceneControllers.Length}.");
        }

        currentScene = sceneControllers[0];

        await currentScene.Initialize(currentSceneContainer);
    }


    private async UniTask UnloadCurrentScene()
    {
        if (currentScene != null)
        {
            await currentScene.ShutDown();
            currentScene = null;
        }
    }


    private async UniTask ShowLoading(LoadingType type)
    {
        if (type == LoadingType.None) return;

        var uiRoot = globalContainer.Resolve<UIRootView>();

        await uiRoot.ShowLoadingScreen(type);
    }


    private async UniTask HideLoading(LoadingType type)
    {
        if (type == LoadingType.None) return;

        var uiRoot = globalContainer.Resolve<UIRootView>();

        await uiRoot.HideLoadingScreen(type);
    }
}
