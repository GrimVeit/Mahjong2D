using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class OtherSceneEntryPoint : MonoBehaviour
{
    //[SerializeField] private UIOtherSceneRoot sceneRootPrefab;

    //private UIOtherSceneRoot sceneRoot;
    //private BankPresenter bankPresenter;
    //private ViewContainer viewContainer;
    //private FirebaseDatabasePresenter firebaseDatabasePresenter;
    //private WebViewPresenter webViewPresenter;

    //public void Run(UIRootView uIRootView)
    //{
    //    FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
    //    FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;
    //    DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    //    sceneRoot = Instantiate(sceneRootPrefab);
    //    uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

    //    viewContainer = sceneRoot.GetComponent<ViewContainer>();
    //    viewContainer.Initialize();

    //    bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
    //    bankPresenter.Initialize();

    //    webViewPresenter = new WebViewPresenter(new WebViewModel(), viewContainer.GetView<WebViewView>());
    //    webViewPresenter.Initialize();

    //    firebaseDatabasePresenter = new FirebaseDatabasePresenter(new FirebaseDatabaseModel(firebaseAuth, databaseReference, bankPresenter));
    //    firebaseDatabasePresenter.Initialize();

    //    ActivateActions();

    //    firebaseDatabasePresenter.GetLink();
    //}

    //private void ActivateActions()
    //{
    //    firebaseDatabasePresenter.OnGetLink += GetURLBd;
    //    firebaseDatabasePresenter.OnErrorGetLink += GoToGame;

    //    webViewPresenter.OnGetLinkFromTitle += GetUrl;
    //    webViewPresenter.OnFail += GoToGame;
    //}

    //private void DeactivateActions()
    //{
    //    firebaseDatabasePresenter.OnGetLink -= GetURLBd;
    //    firebaseDatabasePresenter.OnErrorGetLink -= GoToGame;

    //    webViewPresenter.OnGetLinkFromTitle -= GetUrl;
    //    webViewPresenter.OnFail -= GoToGame;
    //}

    //private void GetURLBd(string link)
    //{
    //    webViewPresenter.GetLinkInTitleFromURL(link);
    //}

    //private void GetUrl(string URL)
    //{
    //    if (URL == null)
    //    {
    //        GoToGame();
    //        return;
    //    }

    //    webViewPresenter.SetURL(URL);
    //    webViewPresenter.Load();
    //}

    //private void GoToGame()
    //{
    //    //Debug.Log("NO GOOD, OPEN MAIN MENU");
    //    OnGoToMenu?.Invoke();
    //}

    //private void OnDestroy()
    //{
    //    DeactivateActions();

    //    webViewPresenter.Dispose();
    //}

    //#region Input

    //public event Action OnGoToMenu;

    //#endregion
}
