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
    private Tween fadeTween;

    private static readonly int Radius =
        Shader.PropertyToID("_Radius");

    private static readonly int Softness =
        Shader.PropertyToID("_Softness");

    private static readonly int Aspect =
        Shader.PropertyToID("_Aspect");


    private void Awake()
    {
        if (transitionImage == null)
        {
            Debug.LogError(
                "[CircleTransitionUI] Transition Image is not assigned.",
                this
            );

            return;
        }

        // Создаём отдельный экземпляр материала
        // только для этого Image.
        material = Instantiate(
            transitionImage.material
        );

        transitionImage.material = material;

        material.SetFloat(
            Softness,
            softness
        );

        UpdateAspect();

        // В начале круг полностью закрыт?
        // Нет — Radius 0 означает, что Image невидим.
        SetRadius(0f);

        transitionImage.raycastTarget = false;
    }


    private void Start()
    {
        UpdateAspect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Show();
        }

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            Hide();
        }
    }


    // =========================================================
    // SHOW
    // Круг появляется из центра
    // =========================================================

    public Tween Show()
    {
        radiusTween?.Kill();
        fadeTween?.Kill();

        radiusTween = DOTween.To(
                () => material.GetFloat(Radius),
                value => SetRadius(value),
                maxRadius,
                duration
            )
            .SetEase(showEase);

        fadeTween = transitionCanvasGroup.DOFade(1, duration);

        return radiusTween;
    }


    // =========================================================
    // HIDE
    // Круг исчезает к центру
    // =========================================================

    public Tween Hide()
    {
        radiusTween?.Kill();
        fadeTween?.Kill();

        radiusTween = DOTween.To(
                () => material.GetFloat(Radius),
                value => SetRadius(value),
                0f,
                duration
            )
            .SetEase(hideEase);

        fadeTween = transitionCanvasGroup.DOFade(0, duration);

        return radiusTween;
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