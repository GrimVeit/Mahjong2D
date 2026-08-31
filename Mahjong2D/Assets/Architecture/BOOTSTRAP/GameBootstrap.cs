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
        FrameRateManager.Initialize();

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

public static class FrameRateManager
{
    public static int TargetFrameRate { get; private set; }

    public static void Initialize()
    {
        float refreshRate =
            (float)Screen.currentResolution.refreshRateRatio.value;

        TargetFrameRate = GetTargetFrameRate(refreshRate);

        Application.targetFrameRate = TargetFrameRate;
    }

    private static int GetTargetFrameRate(float refreshRate)
    {
        if (refreshRate >= 120f)
            return 120;

        if (refreshRate >= 90f)
            return 90;

        return 60;
    }
}
