using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyVisualModel
{
    private readonly IMoneyInfoProvider _moneyInfoProvider;
    private readonly IMoneyEventsProvider _moneyEventsProvider;

    public MoneyVisualModel(IMoneyInfoProvider moneyInfoProvider, IMoneyEventsProvider moneyEventsProvider)
    {
        _moneyInfoProvider = moneyInfoProvider;
        _moneyEventsProvider = moneyEventsProvider;
    }

    public void Initialize()
    {
        _moneyEventsProvider.OnChangeMoney += SetMoney;
        _moneyEventsProvider.OnAdd += Add;
        _moneyEventsProvider.OnRemove += Remove;

        SetMoney(_moneyInfoProvider.Money);
    }

    public void Dispose()
    {
        _moneyEventsProvider.OnChangeMoney -= SetMoney;
        _moneyEventsProvider.OnAdd -= Add;
        _moneyEventsProvider.OnRemove -= Remove;
    }

    private void Add()
    {
        OnAdd?.Invoke();
    }

    private void Remove()
    {
        OnRemove?.Invoke();
    }

    private void SetMoney(int money)
    {
        OnChangeVisual?.Invoke(money);
    }

    #region Output

    public event Action<int> OnChangeVisual;

    public event Action OnAdd;
    public event Action OnRemove;

    #endregion
}
