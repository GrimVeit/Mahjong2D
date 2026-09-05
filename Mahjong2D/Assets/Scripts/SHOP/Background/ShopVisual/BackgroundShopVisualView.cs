using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundShopVisualView : View
{
    [SerializeField] private BuyDisplay buyDisplay;
    [Header("Reference")]
    [SerializeField] private List<Transform> transformsPoint;
    [SerializeField] private BackgroundShopVisual backgroundShopVisual_Prefab;

    [Header("Duration")]
    [SerializeField] private float durationFrameSelect;
    [SerializeField] private float durationFrameShop;

    private Dictionary<int, BackgroundShopVisual> backgroundShopVisuals;

    public void Initialize()
    {
        buyDisplay.OnClickBuy += Buy;
        buyDisplay.Initialize();
    }

    public void Dispose()
    {
        buyDisplay.OnClickBuy -= Buy;
        buyDisplay.Dispose();
    }

    public void SetBackgrounds(IEnumerable<Background> backgrounds)
    {
        ClearBackgrounds();

        backgroundShopVisuals = new Dictionary<int, BackgroundShopVisual>();

        int index = 0;

        foreach (var background in backgrounds)
        {
            if (index >= transformsPoint.Count)
            {
                Debug.LogWarning(
                    $"Not enough spawn points for Backgrounds. " +
                    $"Available: {transformsPoint.Count}"
                );

                break;
            }

            Transform point = transformsPoint[index];

            BackgroundShopVisual visual = Instantiate(
                backgroundShopVisual_Prefab,
                point
            );

            visual.transform.localPosition = Vector3.zero;

            visual.OnChooseBackground += HandleChooseBackground;
            visual.Initialize();
            visual.SetData(background);

            if (background.IsOpened)
            {
                visual.HideDescription();
            }
            else
            {
                visual.ShowDescription();
            }

            backgroundShopVisuals.Add(background.Index, visual);

            index++;
        }
    }

    public void OpenBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.HideDescription();
    }

    #region SELECT

    public void SelectBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.ShowFrameSelect(durationFrameSelect);
    }

    public void DeselectBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.HideFrameSelect(durationFrameSelect);
    }

    #endregion

    #region SELECT SHOP

    public void SelectShopBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.ShowFrameShop(durationFrameSelect);
    }

    public void DeselectShopBackground(int index)
    {
        if (!TryGetBackgroundShopVisual(index, out var visual))
            return;

        visual.HideFrameShop(durationFrameSelect);
    }

    #endregion

    #region INPUT

    public void ShowBuy()
    {
        buyDisplay.Show();
    }

    public void HideBuy()
    {
        buyDisplay.Hide();
    }

    #endregion

    private void ClearBackgrounds()
    {
        if (backgroundShopVisuals == null)
            return;

        foreach (var visual in backgroundShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnChooseBackground -= HandleChooseBackground;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        backgroundShopVisuals.Clear();
    }
    private bool TryGetBackgroundShopVisual(int index, out BackgroundShopVisual visual)
    {
        if (backgroundShopVisuals != null &&
            backgroundShopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning(
            $"Not found BackgroundShopVisual with Index - {index}"
        );

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnChooseBackground;
    public event Action OnBuy;

    private void HandleChooseBackground(int index)
    {
        OnChooseBackground?.Invoke(index);
    }

    private void Buy()
    {
        OnBuy?.Invoke();
    }

    #endregion


}

[Serializable]
public class BuyDisplay
{
    [Header("References")]
    [SerializeField] private Transform transformMoney;
    [SerializeField] private Transform transformBuy;
    [SerializeField] private Button buttonBuy;

    [Header("Hide")]
    [SerializeField] private Transform transformZero;

    [Header("Show")]
    [SerializeField] private Transform transformLeft;
    [SerializeField] private Transform transformRight;

    [Header("Animation")]
    [SerializeField] private float duration = 0.35f;
    [SerializeField] private Ease ease = Ease.OutCubic;

    private bool isShow = false;

    private Sequence sequence;

    public void Initialize()
    {
        buttonBuy.onClick.AddListener(ClickBuy);

        HideImmediate();
    }

    public void Dispose()
    {
        sequence?.Kill();

        buttonBuy.onClick.RemoveListener(ClickBuy);
    }

    public void Show()
    {
        if (isShow) return;

        isShow = true;

        sequence?.Kill();

        buttonBuy.interactable = true;

        // Начальное состояние:
        // монеты в центре, кнопка скрыта и немного "готова" к появлению.
        transformMoney.localPosition = transformZero.localPosition;

        transformBuy.localPosition = transformZero.localPosition;
        transformBuy.localScale = Vector3.zero;

        sequence = DOTween.Sequence();

        // Монеты уезжают влево.
        sequence.Join(
            transformMoney
                .DOLocalMove(transformLeft.localPosition, duration)
                .SetEase(ease)
        );

        // Кнопка появляется и одновременно выезжает вправо.
        sequence.Insert(
            0f,
            transformBuy
                .DOLocalMove(transformRight.localPosition, duration)
                .SetEase(ease)
        );

        sequence.Insert(
            0f,
            transformBuy
                .DOScale(0.9f, duration)
                .SetEase(Ease.OutBack)
        );
    }

    public void Hide()
    {
        if (!isShow) return;

        isShow = false;

        sequence?.Kill();

        buttonBuy.interactable = false;

        sequence = DOTween.Sequence();

        // Монеты возвращаются в центр.
        sequence.Join(
            transformMoney
                .DOLocalMove(transformZero.localPosition, duration)
                .SetEase(ease)
        );

        // Кнопка возвращается в центр и исчезает.
        sequence.Join(
            transformBuy
                .DOLocalMove(transformZero.localPosition, duration)
                .SetEase(ease)
        );

        sequence.Join(
            transformBuy
                .DOScale(Vector3.zero, duration)
                .SetEase(Ease.InBack)
        );
    }

    private void HideImmediate()
    {
        buttonBuy.interactable = false;

        transformMoney.localPosition = transformZero.localPosition;
        transformBuy.localPosition = transformZero.localPosition;
        transformBuy.localScale = Vector3.zero;
    }

    #region Output

    public event Action OnClickBuy;

    private void ClickBuy()
    {
        OnClickBuy?.Invoke();
    }

    #endregion
}
