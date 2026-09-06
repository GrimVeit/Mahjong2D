using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class StartPrepareEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    private UIRoot_Menu _uIRoot;
    private ViewContainer _viewContainer;

    private ISceneService _sceneService;
    private FirebaseDatabasePresenter _firebaseDatabasePresenter;

    private InternetPresenter _internetPresenter;

    #region ENTRY

    public override async UniTask Initialize(DIContainer container)
    {
        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    public override UniTask BeforeShutdown()
    {
        base.BeforeShutdown();

        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();

        await base.ShutDown();
    }

    #endregion

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _sceneService = container.Resolve<ISceneService>();

        //-----------------------FIREBASE---------------------//

        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
        FirebaseDatabase database = FirebaseDatabase.DefaultInstance;

        _firebaseDatabasePresenter = new FirebaseDatabasePresenter(new FirebaseDatabaseModel(database));
        container.RegisterInstance<IDatabaseProvider>(_firebaseDatabasePresenter);

        //-----------------------------------------------------//

        _internetPresenter = new InternetPresenter(new InternetModel());

        return UniTask.CompletedTask;
    }

    protected override async UniTask OnSceneInitialized(DIContainer container)
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            return;
        }

        if (!_internetPresenter.HasNetwork)
        {
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            //íåò èíòåğíåòà
            return;
        }

        (DatabaseResult result, PlayerData player) = await _firebaseDatabasePresenter.GetPlayerByPlace(1);

        if(result != DatabaseResult.Success)
        {
            StartAutoTransition(new SceneTransition(Scenes.Menu, LoadingType.None)).Forget();
            //ÍÅ ÏÎËÓ×ÈËÎÑÜ ÏÎËÓ×ÈÒÜ ÏÅĞÂÎÃÎ ×ÅËÎÂÅÊÀ
            return;
        }
    }

    private async UniTask StartAutoTransition(SceneTransition transition)
    {
        await UniTask.Delay(50);

        await _sceneService.ChangeSceneAsync(transition);
    }
}
