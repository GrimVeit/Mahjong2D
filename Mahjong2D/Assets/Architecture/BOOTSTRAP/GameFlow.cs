using System.Collections;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class GameFlow
{
    private readonly SceneService sceneService;


    public GameFlow(
        DIContainer container)
    {
        sceneService =
            container.Resolve<SceneService>();
    }


    public void Start()
    {
        LoadInitialScene();
    }


    private async void LoadInitialScene()
    {
        await sceneService.ChangeSceneAsync(
            new SceneTransition(
                Scenes.Menu,
                LoadingType.Default
            )
        );
    }
}
