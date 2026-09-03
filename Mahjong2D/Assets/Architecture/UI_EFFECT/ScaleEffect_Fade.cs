using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleEffect_Fade : UIEffect
{
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Transform scaleElement;

    private Tween tween;

    public override void Initialize()
    {
        ResetEffect();
    }

    public override void Dispose()
    {
        tween?.Kill();
    }

    public override void ResetEffect()
    {
        isActive = false;

        tween?.Kill();

        scaleElement.localScale = Vector3.zero;
    }

    public override void PlayShow(Action onComplete = null)
    {
        isActive = true;

        tween?.Kill();

        scaleElement.localScale = Vector3.zero;

        tween = scaleElement
            .DOScale(Vector3.one, duration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => onComplete?.Invoke());
    }

    public override void PlayHide(Action onComplete = null)
    {
        isActive = false;

        tween?.Kill();

        tween = scaleElement
            .DOScale(Vector3.zero, duration)
            .SetEase(Ease.InBack)
            .OnComplete(() => onComplete?.Invoke());
    }
}
