using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuEntryPoint : SceneEntryPoint, ISceneEntry
{
    private SceneService _sceneService;

    public override async UniTask Initialize(DIContainer container)
    {
        await base.Initialize(container);

        _sceneService = container.Resolve<SceneService>();

        await UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await base.ShutDown();

        await UniTask.CompletedTask;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoToGame();
        }
    }

    private void GoToGame()
    {
        GoToGame_UniTask().Forget(Debug.LogException);
    }

    private async UniTask GoToGame_UniTask()
    {
        await _sceneService.ChangeScene(new SceneTransition(Scenes.Game, LoadingType.Game));
    }
}
