using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadePanel : Panel
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.25f;

    private Tween tween;

    protected override void OnStartShow()
    {
        tween?.Kill();

        tween = canvasGroup
            .DOFade(1f, duration)
            .SetUpdate(true)
            .OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        tween?.Kill();

        tween = canvasGroup
            .DOFade(0f, duration)
            .SetUpdate(true)
            .OnComplete(CompleteHide);
    }

    public override void Dispose()
    {
        tween?.Kill();
        tween = null;

        base.Dispose();
    }
}
