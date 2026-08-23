using System;

public class StoreMoneyPresenter : IMoneyInfoProvider, IMoneyEventsProvider, IMoneyProvider
{
    private readonly StoreMoneyModel _model;

    public StoreMoneyPresenter(StoreMoneyModel model)
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

    #region Info

    public int Money => _model.Money;

    #endregion

    #region Events

    public event Action<int> OnChangeMoney
    {
        add => _model.OnChangeMoney += value;
        remove => _model.OnChangeMoney -= value;
    }

    public event Action<int> OnMoneyChangedBy
    {
        add => _model.OnMoneyChangedBy += value;
        remove => _model.OnMoneyChangedBy -= value;
    }

    public event Action OnAdd
    {
        add => _model.OnAdd += value;
        remove => _model.OnAdd -= value;
    }

    public event Action OnRemove
    {
        add => _model.OnRemove += value;
        remove => _model.OnRemove -= value;
    }

    #endregion

    #region Input

    public void ChangeMoney(int amount)
    {
        _model.ChangeMoney(amount);
    }

    public bool CanAfford(int amount)
    {
        return _model.CanAfford(amount);
    }

    #endregion
}

public interface IMoneyInfoProvider
{
    int Money { get; }
}

public interface IMoneyEventsProvider
{
    event Action<int> OnChangeMoney;
    event Action<int> OnMoneyChangedBy;

    event Action OnAdd;
    event Action OnRemove;
}

public interface IMoneyProvider
{
    public void ChangeMoney(int amount);
    public bool CanAfford(int amount);
}
