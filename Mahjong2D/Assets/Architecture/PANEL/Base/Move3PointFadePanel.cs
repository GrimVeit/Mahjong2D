using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Move3PointFadePanel : Panel
{
    [Header("Positions")]
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 open;
    [SerializeField] private Vector3 exit;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.25f;

    private Sequence sequence;

    protected override void OnStartShow()
    {
        sequence?.Kill();

        transform.localPosition = from;
        canvasGroup.alpha = 0f;

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalMove(open, duration)
        );

        sequence.Join(
            canvasGroup.DOFade(1f, duration)
        );

        sequence.OnComplete(CompleteShow);
    }

    protected override void OnStartHide()
    {
        sequence?.Kill();

        sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Join(
            transform.DOLocalMove(exit, duration)
        );

        sequence.Join(
            canvasGroup.DOFade(0f, duration)
        );

        sequence.OnComplete(CompleteHide);
    }

    public override void Dispose()
    {
        sequence?.Kill();
        sequence = null;

        base.Dispose();
    }
}
