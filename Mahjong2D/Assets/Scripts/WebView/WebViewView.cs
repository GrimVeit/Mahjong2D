using System;
using TMPro;
using UnityEngine;

public class WebViewView : View
{
    [SerializeField] private UniWebView uniWebView;
    [SerializeField] private RectTransform referenceRectTransform;
    [SerializeField] private TextMeshProUGUI textLoading;

    public event Action<UniWebView, int, string> OnError;
    public event Action<UniWebView, string> OnStart;
    public event Action<UniWebView, string> OnFinish;
    public event Action OnClosePage;


    private string LogPrefix => $"[WebViewView]";

    public void Initialize()
    {
        Debug.Log(
            $"{LogPrefix} Initialize() START | " +
            $"GameObject={gameObject.name}, " +
            $"InstanceID={GetInstanceID()}, " +
            $"UniWebView={(uniWebView != null ? "ASSIGNED" : "NULL")}, " +
            $"ReferenceRectTransform={(referenceRectTransform != null ? "ASSIGNED" : "NULL")}, " +
            $"TextLoading={(textLoading != null ? "ASSIGNED" : "NULL")}",
            this);

        if (uniWebView == null)
        {
            Debug.Log(
                $"{LogPrefix} UniWebView is NULL. Creating UniWebView component...",
                this);

            uniWebView = gameObject.AddComponent<UniWebView>();

            Debug.Log(
                $"{LogPrefix} UniWebView component CREATED. " +
                $"InstanceID={uniWebView.GetInstanceID()}",
                this);
        }
        else
        {
            Debug.Log(
                $"{LogPrefix} UniWebView already exists. " +
                $"InstanceID={uniWebView.GetInstanceID()}",
                this);
        }

        InitializeWebView();
        ActivateEvents();

        Debug.Log(
            $"{LogPrefix} Initialize() END",
            this);
    }

    private void InitializeWebView()
    {
        Debug.Log(
            $"{LogPrefix} InitializeWebView() START",
            this);

        if (referenceRectTransform == null)
        {
            Debug.LogError(
                $"{LogPrefix} InitializeWebView() FAILED: " +
                "Reference Rect Transform is not assigned.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} Setting ReferenceRectTransform. " +
            $"Transform={referenceRectTransform.name}",
            this);

        uniWebView.ReferenceRectTransform = referenceRectTransform;

        Debug.Log(
            $"{LogPrefix} Hiding EmbeddedToolbar navigation buttons",
            this);

        uniWebView.EmbeddedToolbar.HideNavigationButtons();

        Debug.Log(
            $"{LogPrefix} Hiding EmbeddedToolbar",
            this);

        uniWebView.EmbeddedToolbar.Hide();

        Debug.Log(
            $"{LogPrefix} InitializeWebView() END",
            this);
    }

    private void ActivateEvents()
    {
        if (uniWebView == null)
        {
            Debug.LogError(
                $"{LogPrefix} ActivateEvents() FAILED: UniWebView is NULL.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} ActivateEvents() START. Subscribing to UniWebView events...",
            this);

        uniWebView.OnPageStarted += OnPageStarted;
        uniWebView.OnPageFinished += OnPageFinished;
        uniWebView.OnShouldClose += OnShouldClose;
        uniWebView.OnLoadingErrorReceived += OnLoadingErrorReceived;

        Debug.Log(
            $"{LogPrefix} Events SUBSCRIBED: " +
            "OnPageStarted, OnPageFinished, OnShouldClose, OnLoadingErrorReceived",
            this);
    }

    private void DeactivateEvents()
    {
        if (uniWebView == null)
        {
            Debug.LogWarning(
                $"{LogPrefix} DeactivateEvents() skipped: UniWebView is NULL.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} DeactivateEvents() START. Unsubscribing from UniWebView events...",
            this);

        uniWebView.OnPageStarted -= OnPageStarted;
        uniWebView.OnPageFinished -= OnPageFinished;
        uniWebView.OnShouldClose -= OnShouldClose;
        uniWebView.OnLoadingErrorReceived -= OnLoadingErrorReceived;

        Debug.Log(
            $"{LogPrefix} Events UNSUBSCRIBED",
            this);
    }

    public void Dispose()
    {
        Debug.Log(
            $"{LogPrefix} Dispose() START",
            this);

        DeactivateEvents();

        Debug.Log(
            $"{LogPrefix} Dispose() END",
            this);
    }

    public void OnLoad(string url)
    {
        Debug.Log(
            $"{LogPrefix} OnLoad() CALLED | URL={url ?? "NULL"}",
            this);

        if (uniWebView == null)
        {
            Debug.LogError(
                $"{LogPrefix} OnLoad() FAILED: UniWebView is NULL.",
                this);

            return;
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            Debug.LogWarning(
                $"{LogPrefix} OnLoad() received EMPTY URL.",
                this);
        }

        Debug.Log(
            $"{LogPrefix} Calling UniWebView.Load() | URL={url}",
            this);

        uniWebView.Load(url);

        Debug.Log(
            $"{LogPrefix} UniWebView.Load() CALLED",
            this);
    }

    public void OnReload()
    {
        Debug.Log(
            $"{LogPrefix} OnReload() CALLED",
            this);

        if (uniWebView == null)
        {
            Debug.LogError(
                $"{LogPrefix} OnReload() FAILED: UniWebView is NULL.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} Calling UniWebView.Reload()",
            this);

        uniWebView.Reload();

        Debug.Log(
            $"{LogPrefix} UniWebView.Reload() CALLED",
            this);
    }

    public void OnHide()
    {
        Debug.Log(
            $"{LogPrefix} OnHide() CALLED",
            this);

        if (uniWebView == null)
        {
            Debug.LogError(
                $"{LogPrefix} OnHide() FAILED: UniWebView is NULL.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} Setting orientation for HIDE | " +
            "Portrait=false, PortraitUpsideDown=false, " +
            "LandscapeLeft=true, LandscapeRight=true",
            this);

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Debug.Log(
            $"{LogPrefix} Screen.orientation={Screen.orientation}",
            this);

        Debug.Log(
            $"{LogPrefix} Calling UniWebView.Hide()",
            this);

        uniWebView.Hide();

        Debug.Log(
            $"{LogPrefix} UniWebView.Hide() CALLED",
            this);
    }

    public void OnShow()
    {
        Debug.Log(
            $"{LogPrefix} OnShow() CALLED",
            this);

        if (uniWebView == null)
        {
            Debug.LogError(
                $"{LogPrefix} OnShow() FAILED: UniWebView is NULL.",
                this);

            return;
        }

        Debug.Log(
            $"{LogPrefix} Calling UniWebView.Show()",
            this);

        uniWebView.Show();

        Debug.Log(
            $"{LogPrefix} UniWebView.Show() CALLED",
            this);

        Debug.Log(
            $"{LogPrefix} Hiding EmbeddedToolbar navigation buttons",
            this);

        uniWebView.EmbeddedToolbar.HideNavigationButtons();

        Debug.Log(
            $"{LogPrefix} Hiding EmbeddedToolbar",
            this);

        uniWebView.EmbeddedToolbar.Hide();

        Debug.Log(
            $"{LogPrefix} Setting orientation for SHOW | " +
            "Portrait=true, PortraitUpsideDown=true, " +
            "LandscapeLeft=true, LandscapeRight=true",
            this);

        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.AutoRotation;

        Debug.Log(
            $"{LogPrefix} Screen.orientation={Screen.orientation}",
            this);

        Debug.Log(
            $"{LogPrefix} OnShow() END",
            this);
    }

    public void OnStartDisplay()
    {
        Debug.Log(
            $"{LogPrefix} OnStartDisplay() CALLED",
            this);

        if (textLoading == null)
        {
            Debug.LogWarning(
                $"{LogPrefix} OnStartDisplay() skipped: TextLoading is NULL.",
                this);

            return;
        }

        textLoading.text = "Loading page...";

        Debug.Log(
            $"{LogPrefix} Loading text SET: \"{textLoading.text}\"",
            this);
    }

    public void OnFinishDisplay()
    {
        Debug.Log(
            $"{LogPrefix} OnFinishDisplay() CALLED",
            this);

        if (textLoading == null)
        {
            Debug.LogWarning(
                $"{LogPrefix} OnFinishDisplay() skipped: TextLoading is NULL.",
                this);

            return;
        }

        textLoading.text = string.Empty;

        Debug.Log(
            $"{LogPrefix} Loading text CLEARED",
            this);
    }

    public void OnErrorDisplay(string errorMessage)
    {
        Debug.LogError(
            $"{LogPrefix} OnErrorDisplay() CALLED | " +
            $"ErrorMessage={errorMessage ?? "NULL"}",
            this);

        if (textLoading == null)
        {
            Debug.LogWarning(
                $"{LogPrefix} OnErrorDisplay() skipped: TextLoading is NULL.",
                this);

            return;
        }

        textLoading.text = errorMessage;

        Debug.Log(
            $"{LogPrefix} Error text SET: \"{textLoading.text}\"",
            this);
    }

    #region Input

    private void OnPageStarted(
        UniWebView webView,
        string url)
    {
        Debug.Log(
            $"{LogPrefix} EVENT OnPageStarted RECEIVED | " +
            $"WebView={(webView != null ? webView.GetInstanceID().ToString() : "NULL")}, " +
            $"URL={url ?? "NULL"}",
            this);

        Debug.Log(
            $"{LogPrefix} Invoking external OnStart event",
            this);

        OnStart?.Invoke(webView, url);

        Debug.Log(
            $"{LogPrefix} External OnStart event INVOKED",
            this);
    }

    private void OnPageFinished(
        UniWebView webView,
        int statusCode,
        string url)
    {
        Debug.Log(
            $"{LogPrefix} EVENT OnPageFinished RECEIVED | " +
            $"WebView={(webView != null ? webView.GetInstanceID().ToString() : "NULL")}, " +
            $"StatusCode={statusCode}, " +
            $"URL={url ?? "NULL"}",
            this);

        Debug.Log(
            $"{LogPrefix} Invoking external OnFinish event",
            this);

        OnFinish?.Invoke(webView, url);

        Debug.Log(
            $"{LogPrefix} External OnFinish event INVOKED",
            this);
    }

    private void OnLoadingErrorReceived(
        UniWebView webView,
        int errorCode,
        string errorMessage,
        UniWebViewNativeResultPayload payload)
    {
        Debug.LogError(
            $"{LogPrefix} EVENT OnLoadingErrorReceived RECEIVED | " +
            $"WebView={(webView != null ? webView.GetInstanceID().ToString() : "NULL")}, " +
            $"ErrorCode={errorCode}, " +
            $"ErrorMessage={errorMessage ?? "NULL"}, " +
            $"Payload={(payload != null ? payload.ToString() : "NULL")}",
            this);

        Debug.LogError(
            $"{LogPrefix} Invoking external OnError event",
            this);

        OnError?.Invoke(webView, errorCode, errorMessage);

        Debug.Log(
            $"{LogPrefix} External OnError event INVOKED",
            this);
    }

    private bool OnShouldClose(UniWebView webView)
    {
        Debug.Log(
            $"{LogPrefix} EVENT OnShouldClose RECEIVED | " +
            $"WebView={(webView != null ? webView.GetInstanceID().ToString() : "NULL")}",
            this);

        Debug.Log(
            $"{LogPrefix} Invoking external OnClosePage event",
            this);

        OnClosePage?.Invoke();

        Debug.Log(
            $"{LogPrefix} External OnClosePage event INVOKED | Returning FALSE",
            this);

        return false;
    }

    #endregion
}


