using System;

public class InternetPresenter : IInternetInfo
{
    private readonly InternetModel _model;

    public InternetPresenter(InternetModel internetModel)
    {
        _model = internetModel;
    }

    #region Info

    public bool HasNetwork => _model.HasNetwork;

    #endregion
}

public interface IInternetInfo
{
    public bool HasNetwork { get; }
}
