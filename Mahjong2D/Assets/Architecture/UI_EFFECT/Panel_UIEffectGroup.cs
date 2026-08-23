using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Panel_UIEffectGroup : MonoBehaviour
{
    [SerializeField] private List<UIEffect> effects = new();

    [Header("Show")]
    [SerializeField] private float showStartDelay = 0f;
    [SerializeField] private float showDelayBetweenEffects = 0.05f;

    [Header("Hide")]
    [SerializeField] private float hideDelayBetweenEffects = 0.05f;
    [SerializeField] private bool reverseHide = false;

    private Sequence sequence;

    public void Initialize()
    {
        foreach (var effect in effects)
            effect.Initialize();
    }

    public void Dispose()
    {
        sequence?.Kill();
        sequence = null;

        foreach (var effect in effects)
            effect.Dispose();
    }

    public void PlayShow()
    {
        sequence?.Kill();

        foreach (var effect in effects)
            effect.ResetEffect();

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.AppendInterval(showStartDelay);

        foreach (var effect in effects)
        {
            sequence.AppendCallback(() => effect.PlayShow());
            sequence.AppendInterval(showDelayBetweenEffects);
        }
    }

    public void PlayHide()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        if (reverseHide)
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];

                sequence.AppendCallback(() => effect.PlayHide());
                sequence.AppendInterval(hideDelayBetweenEffects);
            }
        }
        else
        {
            foreach (var effect in effects)
            {
                effect.PlayHide();
            }
        }
    }
}
