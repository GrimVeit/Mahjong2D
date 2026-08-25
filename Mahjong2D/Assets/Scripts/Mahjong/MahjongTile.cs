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

    [Header("Select")]
    [SerializeField] private Image imageSelectDeselect;
    [SerializeField] private float durationSelectDeselect;

    private Tween tweenSelect;


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


    public void SetActiveVisual(
        bool isActive)
    {
        if (background == null)
            return;


        Color color =
            background.color;


        if (isActive)
        {
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            color.a = 1f;
        }
        else
        {
            color.r = 0.5f;
            color.g = 0.5f;
            color.b = 0.5f;
            color.a = 1f;
        }


        background.color =
            color;
    }


    // =========================================================
    // SELECTION
    // =========================================================

    public void Select()
    {
        tweenSelect?.Kill();

        tweenSelect = imageSelectDeselect.DOFade(1, durationSelectDeselect);
    }


    public void Unselect()
    {
        tweenSelect?.Kill();

        tweenSelect = imageSelectDeselect.DOFade(0, durationSelectDeselect);
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