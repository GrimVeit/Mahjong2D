using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDesignShopVisualView : View
{
    [SerializeField] private BuyDisplay buyDisplay;
    [Header("Reference")]
    [SerializeField] private List<Transform> transformsPoint;
    [SerializeField] private CardDesignShopVisual cardDesignShopVisual_Prefab;

    [Header("Duration")]
    [SerializeField] private float durationFrameSelect;
    [SerializeField] private float durationFrameShop;

    private Dictionary<int, CardDesignShopVisual> cardDesignShopVisuals;

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

    public void SetBackgrounds(IEnumerable<CardsDesign> designs)
    {
        ClearDesigns();

        cardDesignShopVisuals = new Dictionary<int, CardDesignShopVisual>();

        int index = 0;

        foreach (var design in designs)
        {
            if (index >= transformsPoint.Count)
            {
                Debug.LogWarning(
                    $"Not enough spawn points for CardDesigns. " +
                    $"Available: {transformsPoint.Count}"
                );

                break;
            }

            Transform point = transformsPoint[index];

            CardDesignShopVisual visual = Instantiate(
                cardDesignShopVisual_Prefab,
                point
            );

            visual.transform.localPosition = Vector3.zero;

            visual.OnChooseCardDessign += HandleChooseDesign;
            visual.Initialize();
            visual.SetData(design);

            if (design.IsOpened)
            {
                visual.HideDescription();
            }
            else
            {
                visual.ShowDescription();
            }

            cardDesignShopVisuals.Add(design.Index, visual);

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

    private void ClearDesigns()
    {
        if (cardDesignShopVisuals == null)
            return;

        foreach (var visual in cardDesignShopVisuals.Values)
        {
            if (visual != null)
            {
                visual.OnChooseCardDessign -= HandleChooseDesign;
                visual.Dispose();
                Destroy(visual.gameObject);
            }
        }

        cardDesignShopVisuals.Clear();
    }
    private bool TryGetBackgroundShopVisual(int index, out CardDesignShopVisual visual)
    {
        if (cardDesignShopVisuals != null &&
            cardDesignShopVisuals.TryGetValue(index, out visual))
        {
            return true;
        }

        Debug.LogWarning(
            $"Not found CardDesignsShopVisual with Index - {index}"
        );

        visual = null;
        return false;
    }

    #region Output

    public event Action<int> OnChooseBackground;
    public event Action OnBuy;

    private void HandleChooseDesign(int index)
    {
        OnChooseBackground?.Invoke(index);
    }

    private void Buy()
    {
        OnBuy?.Invoke();
    }

    #endregion
}
