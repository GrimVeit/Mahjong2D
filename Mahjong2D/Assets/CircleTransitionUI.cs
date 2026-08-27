using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CircleTransitionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private CanvasGroup transitionCanvasGroup;


    [Header("Circle")]
    [SerializeField] private float maxRadius = 0.8f;
    [SerializeField] private float softness = 0.02f;

    [Header("Show")]
    [SerializeField] private float showDuration = 0.6f;
    [SerializeField] private Ease showEase = Ease.InOutCubic;

    [Tooltip("Угол картинки в самом начале открытия.")]
    [SerializeField] private float showRotation = -18f;

    [Tooltip(
        "При достижении этого радиуса картинка полностью раскручена."
    )]
    [SerializeField] private float showRotationRadius = 0.25f;

    [SerializeField] private Ease showRotationEase = Ease.OutCubic;


    [Header("Hide")]
    [SerializeField] private float hideDuration = 0.6f;
    [SerializeField] private Ease hideEase = Ease.InOutCubic;

    [Tooltip("Угол картинки в конце закрытия.")]
    [SerializeField] private float hideRotation = -18f;

    [Tooltip(
        "Ниже этого радиуса начинается вращение картинки."
    )]
    [SerializeField] private float hideRotationRadius = 0.25f;

    [SerializeField] private Ease hideRotationEase = Ease.InCubic;


    // =========================================================
    // INTERNAL
    // =========================================================

    private Material material;

    private Tween radiusTween;
    private Tween fadeTween;
    private Tween colorTween;

    private static readonly int Radius =
        Shader.PropertyToID("_Radius");

    private static readonly int Softness =
        Shader.PropertyToID("_Softness");

    private static readonly int Aspect =
        Shader.PropertyToID("_Aspect");


    // =========================================================
    // INITIALIZE
    // =========================================================

    public async UniTask Initialize()
    {
        if (transitionImage == null)
        {
            Debug.LogError(
                "[CircleTransitionUI] Transition Image is not assigned.",
                this
            );

            return;
        }

        if (transitionImage.material == null)
        {
            Debug.LogError(
                "[CircleTransitionUI] Transition Image has no material.",
                this
            );

            return;
        }

        material = Instantiate(transitionImage.material);
        transitionImage.material = material;

        material.SetFloat(Softness, softness);

        // Ждём кадр, чтобы RectTransform гарантированно
        // имел актуальные размеры.
        await UniTask.Yield();

        UpdateAspect();

        // Начальное состояние:
        // круг закрыт + картинка повернута.
        SetRadius(0f);
        SetRotation(showRotation);

        transitionImage.raycastTarget = false;

        if (transitionCanvasGroup != null)
            transitionCanvasGroup.alpha = 0f;

        await UniTask.CompletedTask;
    }


    // =========================================================
    // SHOW
    // =========================================================

    public async UniTask Show()
    {
        if (!IsReady())
            return;

        KillTweens();

        float startRadius =
            material.GetFloat(Radius);

        radiusTween = DOTween.To(
                () => startRadius,
                value =>
                {
                    SetRadius(value);

                    float rotation =
                        CalculateShowRotation(value);

                    SetRotation(rotation);
                },
                maxRadius,
                showDuration
            )
            .SetEase(showEase);

        fadeTween =
            transitionCanvasGroup
                .DOFade(1f, showDuration / 2f)
                .SetDelay(0.4f);

        colorTween = transitionImage.DOColor(Color.white, showDuration);

        await UniTask.Delay((int)(showDuration * 1000f));
    }


    // =========================================================
    // HIDE
    // =========================================================

    public async UniTask Hide()
    {
        if (!IsReady())
            return;

        KillTweens();

        float startRadius =
            material.GetFloat(Radius);

        radiusTween = DOTween.To(
                () => startRadius,
                value =>
                {
                    SetRadius(value);

                    float rotation =
                        CalculateHideRotation(value);

                    SetRotation(rotation);
                },
                0f,
                hideDuration
            )
            .SetEase(hideEase);

        fadeTween =
            transitionCanvasGroup
                .DOFade(0f, hideDuration / 2f);

        colorTween = transitionImage.DOColor(Color.black, showDuration);

        await UniTask.Delay((int)(hideDuration * 1000f));
    }


    // =========================================================
    // SHOW ROTATION
    // =========================================================
    //
    // radius 0
    //      ↓
    // rotation = showRotation
    //
    // radius showRotationRadius
    //      ↓
    // rotation = 0
    //
    // radius > showRotationRadius
    //      ↓
    // rotation = 0
    // =========================================================

    private float CalculateShowRotation(float radius)
    {
        if (showRotationRadius <= 0f)
            return 0f;

        // Уже полностью раскручено.
        if (radius >= showRotationRadius)
            return 0f;

        // 0 при radius = 0
        // 1 при radius = showRotationRadius
        float progress =
            Mathf.InverseLerp(
                0f,
                showRotationRadius,
                radius
            );

        // Применяем ease именно к вращению.
        progress =
            DOVirtual.EasedValue(
                0f,
                1f,
                progress,
                showRotationEase
            );

        // showRotation -> 0
        return Mathf.Lerp(
            showRotation,
            0f,
            progress
        );
    }


    // =========================================================
    // HIDE ROTATION
    // =========================================================
    //
    // radius > hideRotationRadius
    //      ↓
    // rotation = 0
    //
    // radius hideRotationRadius
    //      ↓
    // rotation = 0
    //
    // radius 0
    //      ↓
    // rotation = hideRotation
    // =========================================================

    private float CalculateHideRotation(float radius)
    {
        if (hideRotationRadius <= 0f)
            return hideRotation;

        // Пока круг большой — вообще не вращаем.
        if (radius >= hideRotationRadius)
            return 0f;

        // 0 при radius = hideRotationRadius
        // 1 при radius = 0
        float progress =
            Mathf.InverseLerp(
                hideRotationRadius,
                0f,
                radius
            );

        progress =
            DOVirtual.EasedValue(
                0f,
                1f,
                progress,
                hideRotationEase
            );

        // 0 -> hideRotation
        return Mathf.Lerp(
            0f,
            hideRotation,
            progress
        );
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
    // SET ROTATION
    // =========================================================

    private void SetRotation(float angle)
    {
        if (transitionImage == null)
            return;

        Vector3 rotation =
            transitionImage.rectTransform.localEulerAngles;

        rotation.z = angle;

        transitionImage.rectTransform.localEulerAngles =
            rotation;
    }


    // =========================================================
    // ASPECT
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
    // HELPERS
    // =========================================================

    private bool IsReady()
    {
        return
            material != null &&
            transitionImage != null &&
            transitionCanvasGroup != null;
    }


    private void KillTweens()
    {
        radiusTween?.Kill();
        fadeTween?.Kill();
        colorTween?.Kill();

        radiusTween = null;
        fadeTween = null;
        colorTween = null;
    }


    // =========================================================
    // CLEANUP
    // =========================================================

    private void OnDestroy()
    {
        KillTweens();

        if (material != null)
        {
            Destroy(material);
        }
    }
}