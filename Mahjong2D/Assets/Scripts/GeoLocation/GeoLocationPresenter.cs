using System;
using Cysharp.Threading.Tasks;

public sealed class GeoLocationPresenter : IGeoLocationListen, IGeoLocationProvider
{
    private readonly GeoLocationModel _model;

    public GeoLocationPresenter(GeoLocationModel model)
    {
        _model = model;
    }

    #region Output

    public event Action<GeoLocationResult, IPInfo> OnGetUserLocation
    {
        add => _model.OnGetUserLocation += value;
        remove => _model.OnGetUserLocation -= value;
    }

    #endregion

    #region Input

    public UniTask<(GeoLocationResult Result, IPInfo Info)> GetUserLocation()
    {
        return _model.GetUserLocation();
    }

    public void Dispose() => _model.Dispose();

    #endregion
}

public interface IGeoLocationListen
{
    public event Action<GeoLocationResult, IPInfo> OnGetUserLocation;
}

public interface IGeoLocationProvider
{
    public UniTask<(GeoLocationResult Result, IPInfo Info)> GetUserLocation();

    public void Dispose();
}
