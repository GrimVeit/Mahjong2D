using System;

public sealed class StorePlayerProfilePresenter : IPlayerProfileInfoProvider, IPlayerProfileProvider, IPlayerProfileEventsProvider
{
    private readonly StorePlayerProfileModel _model;

    public StorePlayerProfilePresenter(StorePlayerProfileModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Input

    public void SetNickname(string nickname)
    {
        _model.SetNickname(nickname);
    }

    #endregion

    #region Output

    public PlayerProfile Profile => _model.Profile;

    public event Action<PlayerProfile> OnProfileChanged
    {
        add => _model.OnProfileChanged += value;
        remove => _model.OnProfileChanged -= value;
    }

    #endregion
}

public interface IPlayerProfileInfoProvider
{
    PlayerProfile Profile { get; }
}

public interface IPlayerProfileProvider
{
    void SetNickname(string nickname);
}

public interface IPlayerProfileEventsProvider
{
    event Action<PlayerProfile> OnProfileChanged;
}
