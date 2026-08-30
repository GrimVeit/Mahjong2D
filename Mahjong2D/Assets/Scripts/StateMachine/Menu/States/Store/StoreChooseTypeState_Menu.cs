using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreChooseTypeState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;
    private readonly IBookPageProvider _bookPageProvider;

    public StoreChooseTypeState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot, IBookPageProvider bookPageProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _bookPageProvider = bookPageProvider;
    }

    public void Enter()
    {
        _sceneRoot.OnClickExit_StoreChooseType += ChangeStateToMain;
        _sceneRoot.OnClickBackgrounds_StoreChooseType += ChangeStateToStoreBackground;
        _sceneRoot.OnClickCards_StoreChooseType += ChangeStateToStoreCards;

        _sceneRoot.ShowStoreChooseTypePanel();
    }

    public void Exit()
    {
        _sceneRoot.OnClickExit_StoreChooseType -= ChangeStateToMain;
        _sceneRoot.OnClickBackgrounds_StoreChooseType -= ChangeStateToStoreBackground;
        _sceneRoot.OnClickCards_StoreChooseType -= ChangeStateToStoreCards;

        _sceneRoot.HideStoreChooseTypePanel();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }

    private void ChangeStateToStoreBackground()
    {
        _stateProvider.SetState(_stateProvider.GetState<StoreBackgroundState_Menu>());
    }

    private void ChangeStateToStoreCards()
    {

    }
}
