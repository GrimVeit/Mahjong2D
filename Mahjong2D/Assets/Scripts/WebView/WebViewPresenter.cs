using System;
using UnityEngine;

public sealed class WebViewPresenter : IWebViewProvider, IWebViewListen, IDisposable
{
    private readonly WebViewModel _model;
    private readonly WebViewView _view;

    public event Action OnStartPage;
    public event Action OnFinishPage;
    public event Action<string> OnErrorPage;
    public event Action OnShowPage;
    public event Action OnHidePage;

    public WebViewPresenter(
        WebViewModel model,
        WebViewView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        Debug.Log("[WebView][Presenter] Initialize()");

        ActivateEvents();

        _view.Initialize();

        Debug.Log("[WebView][Presenter] Initialize() completed");
    }

    private void ActivateEvents()
    {
        Debug.Log("[WebView][Presenter] ActivateEvents()");

        // View -> Presenter
        _view.OnStart += OnPageStarted;
        _view.OnFinish += OnPageFinished;
        _view.OnError += OnPageError;
        _view.OnClosePage += OnPageClosed;

        // Model -> View
        _model.OnLoad += _view.OnLoad;
        _model.OnReload += _view.OnReload;
        _model.OnShow += _view.OnShow;
        _model.OnHide += _view.OnHide;

        // Model -> Presenter
        _model.OnStartPage += HandleStartPage;
        _model.OnFinishPage += HandleFinishPage;
        _model.OnErrorPage += HandleErrorPage;
        _model.OnShow += HandleShowPage;
        _model.OnHide += HandleHidePage;
    }

    private void DeactivateEvents()
    {
        Debug.Log("[WebView][Presenter] DeactivateEvents()");

        // View -> Presenter
        _view.OnStart -= OnPageStarted;
        _view.OnFinish -= OnPageFinished;
        _view.OnError -= OnPageError;
        _view.OnClosePage -= OnPageClosed;

        // Model -> View
        _model.OnLoad -= _view.OnLoad;
        _model.OnReload -= _view.OnReload;
        _model.OnShow -= _view.OnShow;
        _model.OnHide -= _view.OnHide;

        // Model -> Presenter
        _model.OnStartPage -= HandleStartPage;
        _model.OnFinishPage -= HandleFinishPage;
        _model.OnErrorPage -= HandleErrorPage;
        _model.OnShow -= HandleShowPage;
        _model.OnHide -= HandleHidePage;
    }

    #region Provider

    public void SetURL(string url)
    {
        Debug.Log(
            $"[WebView][Presenter] SetURL() | URL={url}");

        _model.SetURL(url);
    }

    public void Load()
    {
        Debug.Log("[WebView][Presenter] Load()");

        _model.Load();
    }

    public void Reload()
    {
        Debug.Log("[WebView][Presenter] Reload()");

        _model.Reload();
    }

    public void Show()
    {
        Debug.Log("[WebView][Presenter] Show()");

        _model.Show();
    }

    public void Hide()
    {
        Debug.Log("[WebView][Presenter] Hide()");

        _model.Hide();
    }

    #endregion

    #region View Input

    private void OnPageStarted(
        UniWebView webView,
        string url)
    {
        Debug.Log(
            $"[WebView][Presenter] OnPageStarted() | URL={url}");

        _model.OnPageStarted();
    }

    private void OnPageFinished(
        UniWebView webView,
        string url)
    {
        Debug.Log(
            $"[WebView][Presenter] OnPageFinished() | URL={url}");

        _model.OnPageFinished();
    }

    private void OnPageError(
        UniWebView webView,
        int errorCode,
        string errorMessage)
    {
        Debug.LogError(
            $"[WebView][Presenter] OnPageError() | " +
            $"Code={errorCode} | " +
            $"Message={errorMessage}");

        _model.OnError(errorMessage);
    }

    private void OnPageClosed()
    {
        Debug.Log(
            "[WebView][Presenter] OnPageClosed()");

        _model.OnPageClosed();
    }

    #endregion

    #region Model Output

    private void HandleStartPage()
    {
        Debug.Log(
            "[WebView][Presenter] HandleStartPage()");

        OnStartPage?.Invoke();
    }

    private void HandleFinishPage()
    {
        Debug.Log(
            "[WebView][Presenter] HandleFinishPage()");

        OnFinishPage?.Invoke();
    }

    private void HandleErrorPage(string errorMessage)
    {
        Debug.LogError(
            $"[WebView][Presenter] HandleErrorPage() | " +
            $"Message={errorMessage}");

        OnErrorPage?.Invoke(errorMessage);
    }

    private void HandleShowPage()
    {
        Debug.Log(
            "[WebView][Presenter] HandleShowPage()");

        OnShowPage?.Invoke();
    }

    private void HandleHidePage()
    {
        Debug.Log(
            "[WebView][Presenter] HandleHidePage()");

        OnHidePage?.Invoke();
    }

    #endregion

    public void Dispose()
    {
        Debug.Log("[WebView][Presenter] Dispose()");

        DeactivateEvents();

        _view.Dispose();
    }
}


public interface IWebViewProvider
{
    void SetURL(string url);
    void Load();
    void Reload();
    void Show();
    void Hide();
}

public interface IWebViewListen
{
    event Action OnShowPage;
    event Action OnHidePage;
    event Action OnStartPage;
    event Action OnFinishPage;
    event Action<string> OnErrorPage;
}


