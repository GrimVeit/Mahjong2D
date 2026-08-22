using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : SceneEntryPoint
{
    public override async UniTask Initialize(DIContainer container)
    {
        await base.Initialize(container);
    }

    public override async UniTask ShutDown()
    {
        await base.ShutDown();
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
        await SceneService.ChangeScene(
            new SceneTransition(Scenes.Menu, LoadingType.Menu)
        );
    }
}
