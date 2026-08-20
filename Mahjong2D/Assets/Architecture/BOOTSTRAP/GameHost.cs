using System.Collections;
using System.Collections.Generic;
using BaCon;
using UnityEngine;

public class GameHost
{
    public DIContainer MainContainer { get; }
    //public GameFlow GameFlow { get; }

    public GameHost()
    {
        MainContainer = new DIContainer();

        RegisterGlobalServices(MainContainer);

        //GameFlow = new GameFlow(MainContainer);
    }

    public void Run()
    {
        //GameFlow.LoadScene(SceneType.Transit);
    }

    private void RegisterGlobalServices(DIContainer container)
    {
        var uiRootPrefab = Resources.Load<UIRootView>("UIRootView");
        var uiRoot = GameObject.Instantiate(uiRootPrefab);
        GameObject.DontDestroyOnLoad(uiRoot);

        container.RegisterInstance(uiRoot);

        //var network = new PhotonNetworkModel();
        //container.RegisterInstance(network);

        //var chat = new PhotonChatModel();
        //chat.Initialize();
        //container.RegisterInstance(chat);

        //container.RegisterInstance(new CoroutineService());
    }
}
