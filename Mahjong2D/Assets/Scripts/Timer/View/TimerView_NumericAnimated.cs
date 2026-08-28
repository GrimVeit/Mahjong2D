using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerView_NumericAnimated : View
    //, ITimerView
    , IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform target;
    [SerializeField] private UIEffect effectText;

    public void Initialize()
    {
        effectText.Initialize();
    }

    public void Dispose()
    {
        effectText.Dispose();
    }

    public string GetID() => id;

    public void ChangeTime(int sec)
    {
        text.text = sec.ToString();
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
        target.DOKill();

        //effectText.DeactivateEffect();
    }
}
