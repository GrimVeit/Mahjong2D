using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDesignShopVisual : MonoBehaviour
{
    public CardsDesign CardDesign => _cardDesign;

    [Header("References")]
    [SerializeField] private Image imageSprite;
    [SerializeField] private Button buttonVisual;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textPrice;
    [SerializeField] private Image imageFrameSelect;
    [SerializeField] private Image imageFrameShop;

    [Header("Description Display")]
    [SerializeField] private UIEffect effectDescription;

    private CardsDesign _cardDesign;

    private Tween _tweenFrameSelect;
    private Tween _tweenFrameShop;

    public void Initialize()
    {
        effectDescription.Initialize();

        buttonVisual.onClick.AddListener(ChooseCardDesign);
    }

    public void Dispose()
    {
        effectDescription.Dispose();

        buttonVisual.onClick.RemoveListener(ChooseCardDesign);
    }

    public void SetData(CardsDesign cardsDesign)
    {
        _cardDesign = cardsDesign;

        imageSprite.sprite = _cardDesign.Sprite;
        textName.text = _cardDesign.Name;
        textPrice.text = _cardDesign.Price.ToString();
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

    public event Action<int> OnChooseCardDessign;

    private void ChooseCardDesign()
    {
        if (_cardDesign == null) return;

        OnChooseCardDessign?.Invoke(_cardDesign.Index);
    }

    #endregion
}
