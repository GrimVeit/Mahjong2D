using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundShopVisual : MonoBehaviour
{
    public Background Background => _background;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Button buttonVisual;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textPrice;
    [SerializeField] private Image imageFrameSelect;
    [SerializeField] private Image imageFrameShop;

    [Header("Description Display")]
    [SerializeField] private UIEffect effectDescription;

    private Background _background;

    private Tween _tweenFrameSelect;
    private Tween _tweenFrameShop;

    public void Initialize()
    {
        effectDescription.Initialize();

        buttonVisual.onClick.AddListener(ChooseBackground);
    }

    public void Dispose()
    {
        effectDescription.Dispose();

        buttonVisual.onClick.RemoveListener(ChooseBackground);
    }

    public void SetData(Background background)
    {
        _background = background;

        imageSprite.sprite = _background.Sprite;
        textName.text = _background.Name;
        textPrice.text = _background.Price.ToString();
    }

    public void ShowDescription()
    {
        effectDescription.PlayShow();
    }

    public void HideDescription()
    {
        effectDescription.PlayHide();
    }

    public void ShowFrameSelect(float duration)
    {
        _tweenFrameSelect?.Kill();

        _tweenFrameSelect = imageFrameSelect.DOFade(1, duration);
    }

    public void HideFrameSelect(float duration)
    {
        _tweenFrameSelect?.Kill();

        _tweenFrameSelect = imageFrameSelect.DOFade(0, duration);
    }

    public void ShowFrameShop(float duration)
    {
        _tweenFrameShop?.Kill();

        _tweenFrameShop = imageFrameShop.DOFade(1, duration);
    }

    public void HideFrameShop(float duration)
    {
        _tweenFrameShop?.Kill();

        _tweenFrameShop = imageFrameShop.DOFade(0, duration);
    }

    #region Output

    public event Action<int> OnChooseBackground;

    private void ChooseBackground()
    {
        if(_background == null) return;

        OnChooseBackground?.Invoke(_background.Index);
    }

    #endregion
}
