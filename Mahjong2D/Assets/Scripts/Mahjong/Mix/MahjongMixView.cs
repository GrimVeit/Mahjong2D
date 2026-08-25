using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MahjongMixView : View
{
    [SerializeField] private Button buttonMix;

    [Header("Active")]
    [SerializeField] private Image imageColor;
    [SerializeField] private Color colorActive;
    [SerializeField] private Color colorInactive;
    [SerializeField] private float durationColor;

    private Tween tweenColor;

    public void Initialize()
    {
        buttonMix.onClick.AddListener(ClickMix);
    }

    public void Dispose()
    {
        buttonMix.onClick.RemoveListener(ClickMix);
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

    public event Action OnClickMix;

    private void ClickMix()
    {
        OnClickMix?.Invoke();
    }

    #endregion
}
