using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition
{
    public string SceneName { get; }

    public LoadingType Loading { get; }


    public SceneTransition(
        string sceneName,
        LoadingType loading)
    {
        SceneName = sceneName;
        Loading = loading;
    }
}
