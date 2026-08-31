using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDesignShopVisualModel
{
    private const int NoDesign = -1;

    private readonly ICardDesignInfoProvider _cardDesignInfoProvider;
    private readonly ICardDesignListener _cardDesignListener;
    private readonly ICardDesignProvider _cardDesignProvider;
    private readonly IMoneyProvider _moneyProvider;

    // Закрытый фон, который сейчас выбран для покупки.
    // Это НЕ CurrentBackground.
    private int _chooseShopCardDesign = NoDesign;

    public CardDesignShopVisualModel(
        ICardDesignInfoProvider cardDesignInfoProvider,
        ICardDesignListener cardDesignListener,
        ICardDesignProvider cardDesignProvider,
        IMoneyProvider moneyProvider)
    {
        _cardDesignInfoProvider = cardDesignInfoProvider;
        _cardDesignListener = cardDesignListener;
        _cardDesignProvider = cardDesignProvider;
        _moneyProvider = moneyProvider;
    }

    public void Initialize()
    {
        _cardDesignListener.OnOpenCardDesign += OpenCardDesign;
        _cardDesignListener.OnSelectCardDesign += SelectCardDesign;

        OnSetDesigns?.Invoke(_cardDesignInfoProvider.GetCardDesigns());
        OnSelectDesign?.Invoke(_cardDesignInfoProvider.CurrentCardDesignIndex);
    }

    public void Dispose()
    {
        _cardDesignListener.OnOpenCardDesign -= OpenCardDesign;
        _cardDesignListener.OnSelectCardDesign -= SelectCardDesign;
    }

    #region INPUT

    private void OpenCardDesign(CardsDesign design)
    {
        OnOpenDesign?.Invoke(design.Index);
    }

    private void SelectCardDesign(CardsDesign design)
    {
        OnSelectDesign?.Invoke(design.Index);
    }

    #endregion

    public void ChooseDesign(int index)
    {
        var design = _cardDesignInfoProvider.GetCardDesign(index);

        if (design.IsOpened)
        {
            ChooseOpenedDesign(index);
            return;
        }

        ChooseClosedDesign(index);
    }

    private void ChooseOpenedDesign(int index)
    {
        int currentIndex = _cardDesignInfoProvider.CurrentCardDesignIndex;

        // Если до этого был выбран закрытый фон для покупки,
        // снимаем с него визуальный выбор.
        if (_chooseShopCardDesign != NoDesign)
        {
            OnHideSelectDesign?.Invoke(_chooseShopCardDesign);
            _chooseShopCardDesign = NoDesign;
        }

        // Нажали на уже выбранный открытый фон.
        if (currentIndex == index)
        {
            OnHideBuy?.Invoke();
            return;
        }

        // Снимаем selected со старого текущего фона.
        OnDeselectDesign?.Invoke(currentIndex);

        // Store меняет CurrentBackground и сам вызовет
        // OnSelectBackground через listener.
        _cardDesignProvider.SelectCardDesign(index);

        OnHideBuy?.Invoke();
    }

    private void ChooseClosedDesign(int index)
    {
        // Если до этого был выбран другой закрытый фон,
        // снимаем с него визуальный выбор.
        if (_chooseShopCardDesign != NoDesign && _chooseShopCardDesign != index)
        {
            OnHideSelectDesign?.Invoke(_chooseShopCardDesign);
        }

        // Запоминаем закрытый фон для покупки.
        _chooseShopCardDesign = index;

        OnShowSelectDesign?.Invoke(_chooseShopCardDesign);

        // Текущий открытый фон НЕ трогаем.
        OnShowBuy?.Invoke();
    }

    public void Buy()
    {
        // Покупать нечего.
        if (_chooseShopCardDesign == NoDesign)
            return;

        var newDesign = _cardDesignInfoProvider.GetCardDesign(_chooseShopCardDesign);
        int oldIndex = _cardDesignInfoProvider.CurrentCardDesignIndex;

        if (!_moneyProvider.CanAfford(newDesign.Price)) return;

        _moneyProvider.ChangeMoney(-newDesign.Price);

        // Открываем фон в Store.
        _cardDesignProvider.OpenCardDesign(newDesign.Index);

        // Снимаем selected со старого текущего фона.
        OnDeselectDesign?.Invoke(oldIndex);

        OnHideSelectDesign?.Invoke(_chooseShopCardDesign);

        // Новый фон сразу становится текущим.
        _cardDesignProvider.SelectCardDesign(newDesign.Index);

        // Сбрасываем временное состояние покупки.
        _chooseShopCardDesign = NoDesign;

        // Кнопка покупки больше не нужна.
        OnHideBuy?.Invoke();
    }

    #region Output

    public event Action<IReadOnlyList<CardsDesign>> OnSetDesigns;
    public event Action<int> OnOpenDesign;
    public event Action<int> OnSelectDesign;
    public event Action<int> OnDeselectDesign;

    public event Action<int> OnShowSelectDesign;
    public event Action<int> OnHideSelectDesign;

    // Кнопка покупки.
    public event Action OnShowBuy;
    public event Action OnHideBuy;

    #endregion
}
