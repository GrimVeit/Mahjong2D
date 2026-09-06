using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class GeoLocationModel : IDisposable
{
    private const string UrlGetIp = "https://ipinfo.io/json";
    private const int RequestTimeoutSeconds = 5;

    private readonly CancellationTokenSource _disposeCts = new();

    public event Action<GeoLocationResult, IPInfo> OnGetUserLocation;

    public async UniTask<(GeoLocationResult Result, IPInfo Info)> GetUserLocation()
    {
        GeoLocationResult result;
        IPInfo info = null;

        try
        {
            using UnityWebRequest request = UnityWebRequest.Get(UrlGetIp);

            await request
                .SendWebRequest()
                .ToUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            if (request.result != UnityWebRequest.Result.Success)
            {
                result = GeoLocationResult.NetworkError;
            }
            else
            {
                info = JsonUtility.FromJson<IPInfo>(request.downloadHandler.text);

                if (info == null)
                {
                    result = GeoLocationResult.InvalidResponse;
                }
                else
                {
                    result = GeoLocationResult.Success;
                }
            }
        }
        catch (OperationCanceledException)
        {
            return (GeoLocationResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = GeoLocationResult.Timeout;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = GeoLocationResult.UnknownError;
        }

        OnGetUserLocation?.Invoke(result, info);

        return (result, info);
    }

    public void Dispose()
    {
        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public enum GeoLocationResult
{
    Success,
    Cancelled,
    Timeout,
    NetworkError,
    InvalidResponse,
    UnknownError
}

[Serializable]
public sealed class IPInfo
{
    public string ip;
    public string hostname;
    public string city;
    public string region;
    public string country;
    public string loc;
    public string org;
    public string postal;
    public string timezone;
    public string readme;
}
