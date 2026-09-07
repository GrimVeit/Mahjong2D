using System;
using UnityEngine;

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
        Debug.Log(
            $"[WebView][Model] SetURL() | URL={url}");

        _url = url;
    }

    public void Load()
    {
        Debug.Log(
            $"[WebView][Model] Load() | URL={_url}");

        if (string.IsNullOrWhiteSpace(_url))
        {
            Debug.LogWarning(
                "[WebView][Model] Load() cancelled: URL is empty.");

            return;
        }

        Debug.Log(
            $"[WebView][Model] Invoking OnLoad | URL={_url}");

        OnLoad?.Invoke(_url);
    }

    public void Reload()
    {
        Debug.Log(
            "[WebView][Model] Reload()");

        OnReload?.Invoke();
    }

    public void Show()
    {
        Debug.Log(
            "[WebView][Model] Show()");

        OnShow?.Invoke();
    }

    public void Hide()
    {
        Debug.Log(
            "[WebView][Model] Hide()");

        OnHide?.Invoke();
    }

    #region WebView callbacks

    public void OnPageStarted()
    {
        Debug.Log(
            "[WebView][Model] OnPageStarted()");

        OnStartPage?.Invoke();
    }

    public void OnPageFinished()
    {
        Debug.Log(
            "[WebView][Model] OnPageFinished()");

        // ВАЖНО:
        // Здесь больше НЕ вызываем OnShow().
        //
        // Завершение загрузки и отображение WebView —
        // это разные действия.

        OnFinishPage?.Invoke();
    }

    public void OnPageClosed()
    {
        Debug.Log(
            "[WebView][Model] OnPageClosed()");

        OnHide?.Invoke();
    }

    public void OnError(string message)
    {
        Debug.LogError(
            $"[WebView][Model] OnError() | Message={message}");

        OnErrorPage?.Invoke(message);
    }

    #endregion
}


