using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView_Formatted : View
    //, ITimerView
    , IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private TextMeshProUGUI text;

    public string GetID() => id;

    public void Initialize() { }
    public void Dispose() { }

    public void ChangeTime(int sec)
    {
        int minutes = sec / 60;
        int seconds = sec % 60;

        text.text = $"{minutes:00}:{seconds:00}";
    }

    public void ActivateTimer() { }

    public void DeactivateTimer() { }
}
