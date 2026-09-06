using System;
using TMPro;
using UnityEngine;

public class WebViewView : View, IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private UniWebView uniWebView;
    [SerializeField] private RectTransform referenceRectTransform;
    [SerializeField] private TextMeshProUGUI textLoading;

    public event Action<UniWebView, int, string> OnError;
    public event Action<UniWebView, string> OnStart;
    public event Action<UniWebView, string> OnFinish;
    public event Action OnClosePage;

    public string GetID() => id;

    public void Initialize()
    {
        if (uniWebView == null)
        {
            uniWebView = gameObject.AddComponent<UniWebView>();
        }

        InitializeWebView();
        ActivateEvents();
    }

    private void InitializeWebView()
    {
        if (referenceRectTransform == null)
        {
            Debug.LogError(
                $"{nameof(WebViewView)}: Reference Rect Transform is not assigned.",
                this);

            return;
        }

        uniWebView.ReferenceRectTransform = referenceRectTransform;

        uniWebView.EmbeddedToolbar.HideNavigationButtons();
        uniWebView.EmbeddedToolbar.Hide();
    }

    private void ActivateEvents()
    {
        uniWebView.OnPageStarted += OnPageStarted;
        uniWebView.OnPageFinished += OnPageFinished;
        uniWebView.OnShouldClose += OnShouldClose;
        uniWebView.OnLoadingErrorReceived += OnLoadingErrorReceived;
    }

    private void DeactivateEvents()
    {
        if (uniWebView == null)
            return;

        uniWebView.OnPageStarted -= OnPageStarted;
        uniWebView.OnPageFinished -= OnPageFinished;
        uniWebView.OnShouldClose -= OnShouldClose;
        uniWebView.OnLoadingErrorReceived -= OnLoadingErrorReceived;
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    public void OnLoad(string url)
    {
        if (uniWebView == null)
            return;

        uniWebView.Load(url);
    }

    public void OnReload()
    {
        if (uniWebView == null)
            return;

        uniWebView.Reload();
    }

    public void OnHide()
    {
        if (uniWebView == null)
            return;

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        uniWebView.Hide();
    }

    public void OnShow()
    {
        if (uniWebView == null)
            return;

        uniWebView.Show();

        uniWebView.EmbeddedToolbar.HideNavigationButtons();
        uniWebView.EmbeddedToolbar.Hide();

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void OnStartDisplay()
    {
        if (textLoading == null)
            return;

        textLoading.text = "Loading page...";
    }

    public void OnFinishDisplay()
    {
        if (textLoading == null)
            return;

        textLoading.text = string.Empty;
    }

    public void OnErrorDisplay(string errorMessage)
    {
        if (textLoading == null)
            return;

        textLoading.text = errorMessage;
    }

    #region Input

    private void OnPageStarted(
        UniWebView webView,
        string url)
    {
        OnStart?.Invoke(webView, url);
    }

    private void OnPageFinished(
        UniWebView webView,
        int statusCode,
        string url)
    {
        OnFinish?.Invoke(webView, url);
    }

    private void OnLoadingErrorReceived(
        UniWebView webView,
        int errorCode,
        string errorMessage,
        UniWebViewNativeResultPayload payload)
    {
        OnError?.Invoke(webView, errorCode, errorMessage);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        OnClosePage?.Invoke();
        return false;
    }

    #endregion
}

