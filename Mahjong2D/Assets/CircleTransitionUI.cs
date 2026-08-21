using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleTransitionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private CanvasGroup transitionCanvasGroup;

    [Header("Animation")]
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Ease showEase = Ease.InOutCubic;
    [SerializeField] private Ease hideEase = Ease.InOutCubic;

    [Header("Circle")]
    [SerializeField] private float maxRadius = 0.8f;
    [SerializeField] private float softness = 0.02f;

    private Material material;

    private Tween radiusTween;
    private Sequence fadeSequence;

    private static readonly int Radius =
        Shader.PropertyToID("_Radius");

    private static readonly int Softness =
        Shader.PropertyToID("_Softness");

    private static readonly int Aspect =
        Shader.PropertyToID("_Aspect");


    public void Initialize()
    {
        if (transitionImage == null)
        {
            Debug.LogError(
                "[CircleTransitionUI] Transition Image is not assigned.",
                this
            );

            return;
        }

        material = Instantiate(
            transitionImage.material
        );

        transitionImage.material = material;

        material.SetFloat(
            Softness,
            softness
        );

        UpdateAspect();

        SetRadius(0f);

        transitionImage.raycastTarget = false;
    }

    public void Show()
    {
        radiusTween?.Kill();
        fadeSequence?.Kill();

        radiusTween = DOTween.To(
                () => material.GetFloat(Radius),
                value => SetRadius(value),
                maxRadius,
                duration
            )
            .SetEase(showEase);

        fadeSequence = DOTween.Sequence();
        fadeSequence.AppendInterval(0.3f).Append(transitionCanvasGroup.DOFade(1, duration / 2));
    }


    // =========================================================
    // HIDE
    // Круг исчезает к центру
    // =========================================================

    public void Hide()
    {
        radiusTween?.Kill();
        fadeSequence?.Kill();

        radiusTween = DOTween.To(
                () => material.GetFloat(Radius),
                value => SetRadius(value),
                0f,
                duration
            )
            .SetEase(hideEase);

        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(transitionCanvasGroup.DOFade(0, duration/2));
    }


    // =========================================================
    // SET RADIUS
    // =========================================================

    private void SetRadius(float value)
    {
        if (material == null)
            return;

        material.SetFloat(
            Radius,
            value
        );
    }


    // =========================================================
    // ASPECT RATIO
    // =========================================================

    private void UpdateAspect()
    {
        if (transitionImage == null || material == null)
            return;

        RectTransform rect =
            transitionImage.rectTransform;

        float width = rect.rect.width;
        float height = rect.rect.height;

        if (height <= 0f)
            return;

        material.SetFloat(
            Aspect,
            width / height
        );
    }


    private void OnRectTransformDimensionsChange()
    {
        UpdateAspect();
    }


    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDestroy()
    {
        radiusTween?.Kill();

        if (material != null)
        {
            Destroy(material);
        }
    }
}