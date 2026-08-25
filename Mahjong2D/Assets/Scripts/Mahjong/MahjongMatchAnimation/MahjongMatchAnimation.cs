using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class MahjongMatchAnimation : View
{
    [Header("Spawn")]
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform first;
    [SerializeField] private RectTransform second;

    [Header("Animation")]
    [SerializeField] private float moveDuration = 0.45f;
    [SerializeField] private float jumpOutDistance = 35f;

    [Header("Arc")]
    [SerializeField] private float arcHeight = 80f;
    [SerializeField] private float arcHeightMultiplier = 0.45f;

    [Header("Impact")]
    [SerializeField] private float impactDuration = 0.12f;
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float punchPosition = 8f;

    [Header("Destroy")]
    [SerializeField] private float destroyDelay = 2f;

    [Header("Impact")]
    [SerializeField] private GameObject impactPrefab;

    [SerializeField] private float impactScale = 1f;

    public void Play(Vector2 firstPosition, Vector2 secondPosition, Vector2 cardSize, Sprite sprite)
    {
        if (parent == null)
        {
            Debug.LogError(
                $"{nameof(MahjongMatchAnimation)}: Parent не назначен!",
                this
            );

            return;
        }

        // =========================================================
        // Определяем левую / правую карту
        // =========================================================

        Vector2 leftPosition;
        Vector2 rightPosition;

        if (firstPosition.x <= secondPosition.x)
        {
            leftPosition = firstPosition;
            rightPosition = secondPosition;
        }
        else
        {
            leftPosition = secondPosition;
            rightPosition = firstPosition;
        }

        // =========================================================
        // Создаём карты
        // =========================================================

        RectTransform leftCard =
            CreateCard(
                "Match Animation Left",
                leftPosition,
                cardSize,
                sprite
            );

        RectTransform rightCard =
            CreateCard(
                "Match Animation Right",
                rightPosition,
                cardSize,
                sprite
            );

        // =========================================================
        // Центр столкновения
        // =========================================================

        Vector2 center =
            (leftPosition + rightPosition) *
            0.5f;

        float halfCardWidth =
            cardSize.x * 0.5f;

        Vector2 leftHitPosition =
            center +
            Vector2.left *
            halfCardWidth;

        Vector2 rightHitPosition =
            center +
            Vector2.right *
            halfCardWidth;

        // =========================================================
        // Определяем ориентацию
        // =========================================================

        Vector2 between =
            rightPosition -
            leftPosition;

        float absX =
            Mathf.Abs(between.x);

        float absY =
            Mathf.Abs(between.y);

        bool horizontal =
            absX >= absY;

        // =========================================================
        // Направления наружу
        // =========================================================

        Vector2 leftOutDirection;
        Vector2 rightOutDirection;

        if (horizontal)
        {
            leftOutDirection =
                Vector2.left;

            rightOutDirection =
                Vector2.right;
        }
        else
        {
            if (leftPosition.y <= rightPosition.y)
            {
                leftOutDirection =
                    Vector2.down;

                rightOutDirection =
                    Vector2.up;
            }
            else
            {
                leftOutDirection =
                    Vector2.up;

                rightOutDirection =
                    Vector2.down;
            }
        }

        // =========================================================
        // Начальные точки после отскока
        // =========================================================

        Vector2 leftJumpOut =
            leftPosition +
            leftOutDirection *
            jumpOutDistance;

        Vector2 rightJumpOut =
            rightPosition +
            rightOutDirection *
            jumpOutDistance;

        // =========================================================
        // MAIN SEQUENCE
        // =========================================================

        Sequence sequence =
            DOTween.Sequence();

        // =========================================================
        // 1. Отскок наружу
        // =========================================================

        float jumpOutDuration =
            moveDuration * 0.2f;

        sequence.Append(
            leftCard
                .DOAnchorPos(
                    leftJumpOut,
                    jumpOutDuration
                )
                .SetEase(
                    Ease.OutQuad
                )
        );

        sequence.Join(
            rightCard
                .DOAnchorPos(
                    rightJumpOut,
                    jumpOutDuration
                )
                .SetEase(
                    Ease.OutQuad
                )
        );

        // =========================================================
        // Squash
        // =========================================================

        sequence.Join(
            leftCard
                .DOScale(
                    new Vector3(
                        1.08f,
                        0.92f,
                        1f
                    ),
                    moveDuration * 0.15f
                )
        );

        sequence.Join(
            rightCard
                .DOScale(
                    new Vector3(
                        1.08f,
                        0.92f,
                        1f
                    ),
                    moveDuration * 0.15f
                )
        );

        // =========================================================
        // 2. Возвращаем Scale
        // =========================================================

        sequence.Append(
            leftCard
                .DOScale(
                    Vector3.one,
                    moveDuration * 0.1f
                )
        );

        sequence.Join(
            rightCard
                .DOScale(
                    Vector3.one,
                    moveDuration * 0.1f
                )
        );

        // =========================================================
        // 3. ДУГА
        // =========================================================

        sequence.Append(
            CreateArcTween(
                leftCard,
                leftJumpOut,
                leftHitPosition,
                leftOutDirection
            )
        );

        sequence.Join(
            CreateArcTween(
                rightCard,
                rightJumpOut,
                rightHitPosition,
                rightOutDirection
            )
        );

        // =========================================================
        // 4. УДАР
        // =========================================================

        GameObject impact = null;

        sequence.AppendCallback(() =>
        {
            impact = CreateImpact(center);
        });

        sequence.Append(
            leftCard.DOPunchScale(
                Vector3.one *
                punchScale,
                impactDuration,
                6,
                0.7f
            )
        );

        sequence.Join(
            rightCard.DOPunchScale(
                Vector3.one *
                punchScale,
                impactDuration,
                6,
                0.7f
            )
        );

        sequence.Join(
            leftCard.DOPunchAnchorPos(
                Vector2.left *
                punchPosition,
                impactDuration,
                5,
                0.5f
            )
        );

        sequence.Join(
            rightCard.DOPunchAnchorPos(
                Vector2.right *
                punchPosition,
                impactDuration,
                5,
                0.5f
            )
        );

        // =========================================================
        // 5. Ждём 2 секунды
        // =========================================================

        sequence.AppendInterval(
            destroyDelay
        );

        // =========================================================
        // 6. Удаление
        // =========================================================

        sequence.AppendCallback(() =>
        {
            FadeAndDestroy(
                leftCard
            );

            FadeAndDestroy(
                rightCard
            );

            FadeAndDestroyImpact(impact);
        });

        sequence.Play();
    }

    // =================================================================
    // ARC
    // =================================================================

    private Tween CreateArcTween(
        RectTransform card,
        Vector2 start,
        Vector2 target,
        Vector2 outwardDirection)
    {
        Vector2 middle =
            (start + target) *
            0.5f;

        // Расстояние от старта до точки столкновения.
        float distance =
            Vector2.Distance(
                start,
                target
            );

        /*
         * Чем дальше карта должна лететь,
         * тем больше становится дуга.
         *
         * arcHeight — минимальная базовая высота.
         *
         * arcHeightMultiplier — насколько расстояние
         * дополнительно влияет на высоту.
         */

        float calculatedArcHeight =
            arcHeight +
            distance *
            arcHeightMultiplier;

        Vector2 controlPoint =
            middle +
            outwardDirection *
            calculatedArcHeight;

        /*
         * Дополнительное смещение наружу,
         * чтобы начало траектории не выглядело
         * как практически прямая линия.
         */

        controlPoint +=
            outwardDirection *
            (jumpOutDistance * 0.5f);

        float progress = 0f;

        return DOTween.To(
                () => progress,
                value =>
                {
                    progress = value;

                    card.anchoredPosition =
                        CalculateQuadraticBezier(
                            start,
                            controlPoint,
                            target,
                            progress
                        );
                },
                1f,
                moveDuration
            )
            .SetEase(
                Ease.InOutSine
            );
    }

    // =================================================================
    // BEZIER
    // =================================================================

    private Vector2 CalculateQuadraticBezier(
        Vector2 start,
        Vector2 control,
        Vector2 end,
        float t)
    {
        float inverse =
            1f - t;

        return
            inverse *
            inverse *
            start

            +

            2f *
            inverse *
            t *
            control

            +

            t *
            t *
            end;
    }

    // =================================================================
    // CREATE CARD
    // =================================================================

    private RectTransform CreateCard(
        string objectName,
        Vector2 position,
        Vector2 cardSize,
        Sprite sprite)
    {
        GameObject card =
            new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image)
            );

        RectTransform rect =
            card.GetComponent<RectTransform>();

        rect.SetParent(
            parent,
            false
        );

        rect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            cardSize;

        Image image =
            card.GetComponent<Image>();

        image.sprite =
            sprite;

        image.preserveAspect =
            false;

        image.raycastTarget =
            false;

        card.transform.SetAsLastSibling();

        return rect;
    }

    // =================================================================
    // IMPACT
    // =================================================================

    private GameObject CreateImpact(Vector2 position)
    {
        if (impactPrefab == null)
        {
            Debug.LogWarning(
                $"{nameof(MahjongMatchAnimation)}: Impact Prefab не назначен!",
                this
            );

            return null;
        }

        GameObject impact =
            Instantiate(
                impactPrefab,
                parent
            );

        RectTransform rect =
            impact.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchorMax =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.pivot =
            new Vector2(
                0.5f,
                0.5f
            );

        rect.anchoredPosition =
            position;

        rect.localScale =
            Vector3.one * impactScale;

        impact.transform.SetAsLastSibling();

        return impact;
    }

    // =================================================================
    // FADE / DESTROY
    // =================================================================

    private void FadeAndDestroy(
        RectTransform card)
    {
        Image image =
            card.GetComponent<Image>();

        Sequence sequence =
            DOTween.Sequence();

        sequence.Append(
            card
                .DOScale(
                    Vector3.zero,
                    0.2f
                )
                .SetEase(
                    Ease.InBack
                )
        );

        sequence.Join(
            image.DOFade(
                0f,
                0.2f
            )
        );

        sequence.OnComplete(() =>
        {
            Destroy(
                card.gameObject
            );
        });
    }

    private void FadeAndDestroyImpact(GameObject card)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            card.transform
                .DOScale(
                    Vector3.zero,
                    0.25f
                )
                .SetEase(
                    Ease.InBack
                )
        );

        sequence.OnComplete(() =>
        {
            Destroy(
                card.gameObject
            );
        });
    }
}
