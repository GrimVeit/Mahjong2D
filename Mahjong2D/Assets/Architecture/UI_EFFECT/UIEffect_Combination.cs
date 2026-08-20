using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIEffect_Combination : MonoBehaviour
{
    [SerializeField] private List<UIEffect> effects = new();

    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float delayBetweenEffects = 0.05f;

    [SerializeField] private bool reverseHide = true;

    private Sequence sequence;

    public void Initialize()
    {
        foreach (var effect in effects)
        {
            effect.Initialize();
        }
    }

    public void Dispose()
    {
        sequence?.Kill();

        foreach (var effect in effects)
        {
            effect.Dispose();
        }
    }

    public void PlayShow()
    {
        sequence?.Kill();

        foreach (var effect in effects)
        {
            effect.ResetEffect();
        }

        sequence = DOTween.Sequence();

        sequence.AppendInterval(startDelay);

        foreach (var effect in effects)
        {
            sequence.AppendCallback(() =>
            {
                effect.PlayShow();
            });

            sequence.AppendInterval(delayBetweenEffects);
        }
    }

    public void PlayHide()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence();

        if (reverseHide)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                UIEffect effect = effects[i];

                sequence.AppendCallback(() =>
                {
                    effect.PlayHide();
                });

                sequence.AppendInterval(delayBetweenEffects);
            }
        }
        else
        {
            foreach (var effect in effects)
            {
                sequence.AppendCallback(() =>
                {
                    effect.PlayHide();
                });

                sequence.AppendInterval(delayBetweenEffects);
            }
        }
    }

    public void ResetEffects()
    {
        sequence?.Kill();

        foreach (var effect in effects)
        {
            effect.ResetEffect();
        }
    }
}
