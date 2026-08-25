using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MahjongHintView : View
{
    [SerializeField] private Button buttonHint;

    [Header("Active")]
    [SerializeField] private Image imageColor;
    [SerializeField] private Color colorActive;
    [SerializeField] private Color colorInactive;
    [SerializeField] private float durationColor;

    private Tween tweenColor;

    public void Initialize()
    {
        buttonHint.onClick.AddListener(ClickHint);
    }

    public void Dispose()
    {
        buttonHint.onClick.RemoveListener(ClickHint);
    }

    public void Active()
    {
        tweenColor?.Kill();

        tweenColor = imageColor.DOColor(colorActive, durationColor);
    }

    public void Deactive()
    {
        tweenColor?.Kill();

        tweenColor = imageColor.DOColor(colorInactive, durationColor);
    }

    #region Ouput

    public event Action OnClickHint;

    private void ClickHint()
    {
        OnClickHint?.Invoke();
    }

    #endregion
}
