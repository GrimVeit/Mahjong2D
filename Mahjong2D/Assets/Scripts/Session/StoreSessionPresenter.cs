public class StoreSessionPresenter
    : ISessionInfoProvider,
      ISessionProvider
{
    private readonly StoreSessionModel _model;

    public StoreSessionPresenter(StoreSessionModel model)
    {
        _model = model;
    }

    #region Info

    public bool IsFirstLaunch =>
        _model.IsFirstLaunch;

    #endregion

    #region Input

    public void CompleteFirstLaunch()
    {
        _model.CompleteFirstLaunch();
    }

    #endregion
}

public interface ISessionInfoProvider
{
    bool IsFirstLaunch { get; }
}

public interface ISessionProvider
{
    void CompleteFirstLaunch();
}
