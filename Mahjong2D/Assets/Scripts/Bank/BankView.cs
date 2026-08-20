using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankView : View
{
    [SerializeField] private List<BankDisplayView> bankDisplayViews = new List<BankDisplayView>();

    public void Initialize()
    {
        for (int i = 0; i < bankDisplayViews.Count; i++)
        {
            bankDisplayViews[i].Initialize();
        }
    }

    public void AddMoney()
    {
        for (int i = 0; i < bankDisplayViews.Count; i++)
        {
            bankDisplayViews[i].AddMoney();
        }
    }

    public void RemoveMoney()
    {
        for (int i = 0; i < bankDisplayViews.Count; i++)
        {
            bankDisplayViews[i].RemoveMoney();
        }
    }

    public void SendMoneyDisplay(float money)
    {
        for (int i = 0; i < bankDisplayViews.Count; i++)
        {
            bankDisplayViews[i].SendMoneyDisplay(money);
        }
    }
}
