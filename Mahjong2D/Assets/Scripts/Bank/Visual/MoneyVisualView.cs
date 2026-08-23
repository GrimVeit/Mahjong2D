using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyVisualView : View
{
    [SerializeField] private List<MoneyVisual> visuals = new();

    public void Initialize()
    {
        foreach (var visual in visuals)
        {
            visual.Initialize();
        }
    }

    public void SetMoney(int money)
    {
        foreach (var visual in visuals)
        {
            visual.SendMoneyDisplay(money);
        }
    }

    public void AddMoney()
    {
        foreach (var visual in visuals)
        {
            visual.AddMoney();
        }
    }

    public void RemoveMoney()
    {
        foreach (var visual in visuals)
        {
            visual.AddMoney();
        }
    }
}
