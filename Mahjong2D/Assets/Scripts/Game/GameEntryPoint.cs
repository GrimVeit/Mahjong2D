using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : SceneEntryPoint, ISceneEntry
{
    private SceneService _sceneService;

    public override async UniTask Initialize(DIContainer container)
    {
        _sceneService = container.Resolve<SceneService>();

        await base.Initialize(container);

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
        await _sceneService.ChangeScene(new SceneTransition(Scenes.Menu, LoadingType.Menu));
    }
}
