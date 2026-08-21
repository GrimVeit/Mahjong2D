using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class GameBootstrap
{
    private static GameApplication application;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        Application.quitting -= Shutdown;

        application = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        InitializeAsync().Forget();
    }

    private static async UniTask InitializeAsync()
    {
        Application.targetFrameRate = 90;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        try
        {
            application = new GameApplication();

            Application.quitting += Shutdown;

            await application.Start();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Shutdown();
        }
    }

    private static void Shutdown()
    {
        Application.quitting -= Shutdown;

        application?.Dispose();
        application = null;
    }
}
