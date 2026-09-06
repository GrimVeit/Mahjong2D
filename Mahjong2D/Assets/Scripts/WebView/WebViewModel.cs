using System;

public sealed class WebViewModel
{
    private string _url;

    public WebViewModel(string url = null)
    {
        _url = url;
    }

    public event Action<string> OnLoad;
    public event Action OnReload;
    public event Action OnShow;
    public event Action OnHide;
    public event Action OnStartPage;
    public event Action OnFinishPage;
    public event Action<string> OnErrorPage;

    public void SetURL(string url)
    {
        _url = url;
    }

    public void Load()
    {
        if (string.IsNullOrEmpty(_url))
            return;

        OnLoad?.Invoke(_url);
    }

    public void Reload()
    {
        OnReload?.Invoke();
    }

    public void Show()
    {
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }

    public void OnPageStarted()
    {
        OnStartPage?.Invoke();
    }

    public void OnPageFinished()
    {
        OnFinishPage?.Invoke();
        OnShow?.Invoke();
    }

    public void OnPageClosed()
    {
        OnHide?.Invoke();
    }

    public void OnError(string message)
    {
        OnErrorPage?.Invoke(message);
    }
}

