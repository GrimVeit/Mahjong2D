using DG.Tweening;
using UnityEngine;

public class MovePanel : Panel
{
    [SerializeField] protected Vector3 from;
    [SerializeField] protected Vector3 to;
    [SerializeField] protected float duration = 0.25f;

    protected Tween tween;

    protected override void OnStartShow()
    {
        tween?.Kill();

        tween = transform
            .DOLocalMove(to, duration)
            .SetUpdate(true)
            .OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        tween?.Kill();

        tween = transform
            .DOLocalMove(from, duration)
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
