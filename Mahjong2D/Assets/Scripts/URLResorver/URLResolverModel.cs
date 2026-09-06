using System;
using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class UrlResolverModel : IDisposable
{
    private const int RequestTimeoutSeconds = 4;

    private readonly CancellationTokenSource _disposeCts = new();

    public event Action OnStartURLResolve;
    public event Action<UrlResolverResult, string> OnURLResolve;

    public async UniTask<(UrlResolverResult Result, string Url)> ResolveFromTitle(string sourceUrl)
    {
        OnStartURLResolve?.Invoke();

        UrlResolverResult result;
        string url = null;

        try
        {
            using UnityWebRequest request = UnityWebRequest.Get(sourceUrl);

            await request
                .SendWebRequest()
                .ToUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            if (request.result != UnityWebRequest.Result.Success)
            {
                result = UrlResolverResult.NetworkError;
            }
            else
            {
                url = ParseUrlFromTitle(request.downloadHandler.text);

                result = string.IsNullOrEmpty(url)
                    ? UrlResolverResult.UrlNotFound
                    : UrlResolverResult.Success;
            }
        }
        catch (OperationCanceledException)
        {
            return (UrlResolverResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = UrlResolverResult.Timeout;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = UrlResolverResult.UnknownError;
        }

        OnURLResolve?.Invoke(result, url);

        return (result, url);
    }

    private static string ParseUrlFromTitle(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        Match match = Regex.Match(
            html,
            @"<title>\s*(.+?)\s*</title>",
            RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            return null;
        }

        string title = match.Groups[1].Value.Trim();

        if (!Uri.TryCreate(title, UriKind.Absolute, out Uri uri))
        {
            return null;
        }

        return uri.Scheme == Uri.UriSchemeHttp ||
               uri.Scheme == Uri.UriSchemeHttps
            ? uri.AbsoluteUri
            : null;
    }

    public void Dispose()
    {
        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public enum UrlResolverResult 
{ 
    Success, 
    Cancelled, 
    Timeout, 
    NetworkError, 
    UrlNotFound, 
    UnknownError 
}