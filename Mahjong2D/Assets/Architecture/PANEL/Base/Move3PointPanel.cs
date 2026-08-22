using DG.Tweening;
using UnityEngine;

public class Move3PointPanel : Panel
{
    [Header("Positions")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 open;
    [SerializeField] private Vector3 exit;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Tween tween;

    protected override void OnStartShow()
    {
        tween?.Kill();

        transform.localPosition = from;

        tween = transform
            .DOLocalMove(open, duration)
            .SetUpdate(true)
            .OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        tween?.Kill();

        tween = transform
            .DOLocalMove(exit, duration)
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
