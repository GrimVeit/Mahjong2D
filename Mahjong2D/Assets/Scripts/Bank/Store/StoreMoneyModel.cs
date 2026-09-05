using System;
using UnityEngine;

public class StoreMoneyModel
{
    public int Money { get; private set; }

    private readonly string _moneyKey;
    private readonly int _defaultMoney;

    public StoreMoneyModel(string moneyKey, int defaultMoney = 0)
    {
        _moneyKey = moneyKey;
        _defaultMoney = defaultMoney;
    }

    public void Initialize()
    {
        Money = PlayerPrefs.GetInt(_moneyKey, _defaultMoney);

        OnChangeMoney?.Invoke(Money);
    }

    public void Dispose()
    {
        PlayerPrefs.SetInt(_moneyKey,Money);
        PlayerPrefs.Save();
    }

    public void ChangeMoney(int amount)
    {
        if (amount == 0) return;

        int oldMoney = Money;
        Money = Math.Max(0, Money + amount);

        int actualChange = Money - oldMoney;

        if (actualChange > 0)
        {
            OnAdd?.Invoke();
        }
        else if (actualChange < 0)
        {
            OnRemove?.Invoke();
        }

        OnMoneyChangedBy?.Invoke(actualChange);
        OnChangeMoney?.Invoke(Money);
    }

    public void SetMoney(int value)
    {
        Money = value;
        OnChangeMoney?.Invoke(Money);
    }

    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }

    #region Output

    public event Action<int> OnChangeMoney;
    public event Action<int> OnMoneyChangedBy;

    public event Action OnAdd;
    public event Action OnRemove;

    #endregion
}
