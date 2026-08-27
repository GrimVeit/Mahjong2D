using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MahjongTile : MonoBehaviour, IPointerClickHandler
{
    public Vector3 Position => transform.localPosition;
    public Vector2 Size => transform.GetComponent<RectTransform>().rect.size;
    public Sprite Sprite => background.sprite;

    [SerializeField] private Image background;
    [SerializeField] private RectTransform transformShake;

    [Header("Color")]
    [SerializeField] private Color colorActive;
    [SerializeField] private Color colorInactive;
    [SerializeField] private float duraionActive;

    [Header("Select")]
    [SerializeField] private Image imageSelectDeselect;
    [SerializeField] private float durationSelectDeselect;

    [SerializeField] private float selectScale = 1.04f;
    [SerializeField] private float durationSelectScale = 0.15f;

    private Tween tweenActive;
    private Tween tweenSelect;
    private Tween tweenSelectScale;
    private Sequence sequenceHint;

    private int id;


    public void Initialize(
        int id,
        Sprite sprite)
    {
        this.id = id;


        if (background != null)
        {
            background.sprite = sprite;
        }
    }


    public void SetActiveVisual(bool isActive)
    {
        if (background == null)
            return;

        tweenActive?.Kill();

        if (isActive)
        {
            background.DOColor(colorActive, duraionActive);
        }
        else
        {
            background.DOColor(colorInactive, duraionActive);
        }
    }


    // =========================================================
    // SELECTION
    // =========================================================

    public void Select()
    {
        tweenSelect?.Kill();
        tweenSelectScale?.Kill();

        tweenSelect = imageSelectDeselect
            .DOFade(1f, durationSelectDeselect)
            .SetEase(Ease.OutQuad);

        tweenSelectScale = transform
            .DOScale(Vector3.one * selectScale, durationSelectScale)
            .SetEase(Ease.OutQuad);
    }

    public void Unselect()
    {
        tweenSelect?.Kill();
        tweenSelectScale?.Kill();

        tweenSelect = imageSelectDeselect
            .DOFade(0f, durationSelectDeselect)
            .SetEase(Ease.OutQuad);

        tweenSelectScale = transform
            .DOScale(Vector3.one, durationSelectScale)
            .SetEase(Ease.InOutQuad);
    }

    public void ShowHint(Action onComplete = null)
    {
        // Не запускаем новую анимацию, пока старая не закончилась
        if (sequenceHint != null && sequenceHint.IsActive() && sequenceHint.IsPlaying())
        {
            onComplete?.Invoke();
            return;
        }

        transformShake.localScale = Vector3.one;
        transformShake.localRotation = Quaternion.identity;

        sequenceHint = DOTween.Sequence();

        sequenceHint.Append(
            transformShake
                .DOScale(1.05f, 0.12f)
                .SetEase(Ease.OutQuad)
        );

        sequenceHint.Append(
            transformShake
                .DOLocalRotate(new Vector3(0, 0, 7f), 0.10f)
                .SetEase(Ease.InOutSine)
        );

        sequenceHint.Append(
            transformShake
                .DOLocalRotate(new Vector3(0, 0, -7f), 0.16f)
                .SetEase(Ease.InOutSine)
        );

        sequenceHint.Append(
            transformShake
                .DOLocalRotate(new Vector3(0, 0, 5f), 0.14f)
                .SetEase(Ease.InOutSine)
        );

        sequenceHint.Append(
            transformShake
                .DOLocalRotate(new Vector3(0, 0, -3f), 0.12f)
                .SetEase(Ease.InOutSine)
        );

        sequenceHint.Append(
            transformShake
                .DOLocalRotate(Vector3.zero, 0.12f)
                .SetEase(Ease.OutSine)
        );

        sequenceHint.Join(
            transformShake
                .DOScale(Vector3.one, 0.25f)
                .SetEase(Ease.OutQuad)
        );

        sequenceHint.OnComplete(() =>
        {
            transformShake.localScale = Vector3.one;
            transformShake.localRotation = Quaternion.identity;

            onComplete?.Invoke();
        });
    }


    // =========================================================
    // INPUT
    // =========================================================

    public void OnPointerClick(
        PointerEventData eventData)
    {
        OnClick?.Invoke(
            id
        );
    }


    // =========================================================
    // OUTPUT
    // =========================================================

    public event Action<int>
        OnClick;
}