using System;

public sealed class WebViewPresenter : IWebViewProvider, IWebViewListen, IDisposable
{
    private readonly WebViewModel _model;
    private readonly WebViewView _view;

    public event Action OnStartPage;
    public event Action OnFinishPage;
    public event Action<string> OnErrorPage;
    public event Action OnShowPage;
    public event Action OnHidePage;

    public WebViewPresenter(WebViewModel model, WebViewView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
        _view.Initialize();
    }

    private void ActivateEvents()
    {
        // View -> Model
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

        // Если эти события нужны наружу:
        _model.OnShow += HandleShowPage;
        _model.OnHide += HandleHidePage;
    }

    private void DeactivateEvents()
    {
        // View -> Model
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
        _model.SetURL(url);
    }

    public void Load()
    {
        _model.Load();
    }

    public void Reload()
    {
        _model.Reload();
    }

    public void Show()
    {
        _model.Show();
    }

    public void Hide()
    {
        _model.Hide();
    }

    #endregion

    #region View Input

    private void OnPageStarted(
        UniWebView webView,
        string url)
    {
        _model.OnPageStarted();
    }

    private void OnPageFinished(
        UniWebView webView,
        string url)
    {
        _model.OnPageFinished();
    }

    private void OnPageError(
        UniWebView webView,
        int errorCode,
        string errorMessage)
    {
        _model.OnError(errorMessage);
    }

    private void OnPageClosed()
    {
        _model.OnPageClosed();
    }

    #endregion

    #region Model Output

    private void HandleStartPage()
    {
        OnStartPage?.Invoke();
    }

    private void HandleFinishPage()
    {
        OnFinishPage?.Invoke();
    }

    private void HandleErrorPage(string errorMessage)
    {
        OnErrorPage?.Invoke(errorMessage);
    }

    private void HandleShowPage()
    {
        OnShowPage?.Invoke();
    }

    private void HandleHidePage()
    {
        OnHidePage?.Invoke();
    }

    #endregion

    public void Dispose()
    {
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


