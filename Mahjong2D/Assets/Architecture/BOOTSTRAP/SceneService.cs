using System;
using System.Linq;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : ISceneService
{
    private ISceneEntry currentScene;
    private bool isTransitioning;

    private readonly DIContainer globalContainer;

    public SceneService(DIContainer container)
    {
        globalContainer = container;
    }

    public void ChangeScene(SceneTransition transition)
    {
        ChangeSceneAsync(transition).Forget(Debug.LogException);
    }

    public async UniTask<bool> ChangeSceneAsync(SceneTransition transition)
    {
        // Защита от повторного запуска transition
        if (isTransitioning)
        {
            Debug.LogWarning("A scene transition is already in progress.");
            return false;
        }

        isTransitioning = true;

        try
        {
            await BeforeUnloadCurrentScene();

            await ShowLoading(transition.Loading);

            await UnloadCurrentScene();

            await LoadScene(transition.SceneName);

            await CreateScene();

            return true;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return false;
        }
        finally
        {
            await HideLoading(transition.Loading);

            isTransitioning = false;
        }
    }

    private async UniTask LoadScene(string scene)
    {
        await SceneManager
            .LoadSceneAsync(scene)
            .ToUniTask();
    }

    private async UniTask CreateScene()
    {
        var currentSceneContainer = new DIContainer(globalContainer);

        var controllers = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        var sceneControllers = controllers
            .OfType<ISceneEntry>()
            .ToArray();

        if (sceneControllers.Length != 1)
        {
            throw new InvalidOperationException(
                $"Scene must contain exactly one {nameof(ISceneEntry)}; " +
                $"found {sceneControllers.Length}.");
        }

        currentScene = sceneControllers[0];

        await currentScene.Initialize(currentSceneContainer);
    }

    private async UniTask UnloadCurrentScene()
    {
        if (currentScene == null)
            return;

        await currentScene.ShutDown();

        currentScene = null;
    }

    private async UniTask BeforeUnloadCurrentScene()
    {
        if (currentScene == null)
            return;

        await currentScene.BeforeShutdown();
    }

    private async UniTask ShowLoading(LoadingType type)
    {
        if (type == LoadingType.None)
            return;

        var uiRoot = globalContainer.Resolve<UIRootView>();

        await uiRoot.ShowLoadingScreen(type);
    }

    private async UniTask HideLoading(LoadingType type)
    {
        if (type == LoadingType.None)
            return;

        var uiRoot = globalContainer.Resolve<UIRootView>();

        await uiRoot.HideLoadingScreen(type);
    }
}

public interface ISceneService
{
    void ChangeScene(SceneTransition transition);
    UniTask<bool> ChangeSceneAsync(SceneTransition transition);
}
