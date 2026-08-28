using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerView_Mapped : View
    //, ITimerView
    , IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform target;
    [SerializeField] private UIEffect effectText;
    [SerializeField] private List<TimerLabel> labels;

    [Serializable]
    private class TimerLabel
    {
        public int value;
        public string text;
    }

    public string GetID() => id;

    public void Initialize()
    {
        effectText.Initialize();
    }

    public void Dispose()
    {
        effectText.Dispose();
    }

    public void ChangeTime(int sec)
    {
        string result = null;

        for (int i = 0; i < labels.Count; i++)
        {
            if (labels[i].value == sec)
            {
                result = labels[i].text;
                break;
            }
        }

        text.text = result ?? sec.ToString();

        Animate();
    }

    private void Animate()
    {
        target.DOKill();
        target.localScale = Vector3.one;

        target
            .DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.2f, 8, 1);
    }

    public void ActivateTimer()
    {
        target.DOKill();

        //effectText.ActivateEffect();
    }

    public void DeactivateTimer()
    {
        Invoke(nameof(Deactivate), 0.2f);
    }

    private void Deactivate()
    {
        target.DOKill();

        //effectText.DeactivateEffect();
    }
}
