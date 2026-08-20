using System;
using UnityEngine;

public class BankModel
{
    public int Money => money;

    private int money;
    public event Action OnAddMoney;
    public event Action OnRemoveMoney;
    public event Action<float> OnChangeMoney;

    public event Action<int> OnSendMoney;

    private const string BANK_MONEY = "BANK_MONEY";

    public void Initialize()
    {
        money = PlayerPrefs.GetInt(BANK_MONEY, 3500);
    }

    public void Destroy()
    {
        PlayerPrefs.SetInt(BANK_MONEY, money);
    }

    public void SendMoney(int money)
    {
        Debug.Log(money);

        OnSendMoney?.Invoke(money);

        if(money >= 0)
        {
            OnAddMoney?.Invoke();
        }
        else
        {
            OnRemoveMoney?.Invoke();
        }
        this.money += money;
        OnChangeMoney?.Invoke(this.money);
    }

    public bool CanAfford(float bet)
    {
        return money >= bet;
    }
}
