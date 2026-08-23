using System.Collections;
using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : SceneEntryPoint
{
    [Header("UI Root Prefab")]
    [SerializeField] private UIRoot_Game uIRoot;

    private UIRoot_Game _uIRoot;
    private ViewContainer _viewContainer;

    private MoneyVisualPresenter _moneyVisualPresenter;

    public override async UniTask Initialize(DIContainer container)
    {
        _uIRoot = Instantiate(uIRoot);
        container.RegisterInstance(_uIRoot);

        var uiRootView = container.Resolve<UIRootView>();
        uiRootView.AttachSceneUI(
            _uIRoot.gameObject,
            Camera.main
        );

        _viewContainer = _uIRoot.GetComponent<ViewContainer>();
        _viewContainer.Initialize();
        container.RegisterInstance(_viewContainer);

        await base.Initialize(container);

        await OnSceneInitialized(container);
    }

    protected override UniTask OnBaseInitialized(DIContainer container)
    {
        _moneyVisualPresenter = new MoneyVisualPresenter(
            new MoneyVisualModel(
                _storeMoneyPresenter,
                _storeMoneyPresenter
            ),
            _viewContainer.GetView<MoneyVisualView>()
        );

        _uIRoot.Initialize();
        _moneyVisualPresenter.Initialize();

        return UniTask.CompletedTask;
    }

    protected override UniTask OnSceneInitialized(DIContainer container)
    {
        return UniTask.CompletedTask;
    }

    public override async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        await base.ShutDown();

        _uIRoot?.Dispose();
        _moneyVisualPresenter?.Dispose();
    }

    #region Output

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
            new SceneTransition(
                Scenes.Menu,
                LoadingType.Game
            )
        );
    }

    #endregion
}
