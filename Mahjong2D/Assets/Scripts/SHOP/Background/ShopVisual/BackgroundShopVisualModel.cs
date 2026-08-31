using System.Collections.Generic;
using System;

public class BackgroundShopVisualModel
{
    private const int NoBackground = -1;

    private readonly IBackgroundInfoProvider _backgroundInfoProvider;
    private readonly IBackgroundListener _backgroundListener;
    private readonly IBackgroundProvider _backgroundProvider;
    private readonly IMoneyProvider _moneyProvider;

    // Закрытый фон, который сейчас выбран для покупки.
    // Это НЕ CurrentBackground.
    private int _chooseShopBackground = NoBackground;

    public BackgroundShopVisualModel(
        IBackgroundInfoProvider backgroundInfoProvider,
        IBackgroundListener backgroundListener,
        IBackgroundProvider backgroundProvider,
        IMoneyProvider moneyProvider)
    {
        _backgroundInfoProvider = backgroundInfoProvider;
        _backgroundListener = backgroundListener;
        _backgroundProvider = backgroundProvider;
        _moneyProvider = moneyProvider;
    }

    public void Initialize()
    {
        _backgroundListener.OnOpenBackground += OpenBackground;
        _backgroundListener.OnSelectBackground += SelectBackground;

        OnSetBackgrounds?.Invoke(_backgroundInfoProvider.GetBackgrounds());
        OnSelectBackground?.Invoke(_backgroundInfoProvider.CurrentBackgroundIndex);
    }

    public void Dispose()
    {
        _backgroundListener.OnOpenBackground -= OpenBackground;
        _backgroundListener.OnSelectBackground -= SelectBackground;
    }

    #region INPUT

    private void OpenBackground(Background background)
    {
        OnOpenBackground?.Invoke(background.Index);
    }

    private void SelectBackground(Background background)
    {
        OnSelectBackground?.Invoke(background.Index);
    }

    #endregion

    public void ChooseBackground(int index)
    {
        var background = _backgroundInfoProvider.GetBackground(index);

        if (background.IsOpened)
        {
            ChooseOpenedBackground(index);
            return;
        }

        ChooseClosedBackground(index);
    }

    private void ChooseOpenedBackground(int index)
    {
        int currentIndex = _backgroundInfoProvider.CurrentBackgroundIndex;

        // Если до этого был выбран закрытый фон для покупки,
        // снимаем с него визуальный выбор.
        if (_chooseShopBackground != NoBackground)
        {
            OnHideSelectBackground?.Invoke(_chooseShopBackground);
            _chooseShopBackground = NoBackground;
        }

        // Нажали на уже выбранный открытый фон.
        if (currentIndex == index)
        {
            OnHideBuy?.Invoke();
            return;
        }

        // Снимаем selected со старого текущего фона.
        OnDeselectBackground?.Invoke(currentIndex);

        // Store меняет CurrentBackground и сам вызовет
        // OnSelectBackground через listener.
        _backgroundProvider.SelectBackground(index);

        OnHideBuy?.Invoke();
    }

    private void ChooseClosedBackground(int index)
    {
        // Если до этого был выбран другой закрытый фон,
        // снимаем с него визуальный выбор.
        if (_chooseShopBackground != NoBackground && _chooseShopBackground != index)
        {
            OnHideSelectBackground?.Invoke(_chooseShopBackground);
        }

        // Запоминаем закрытый фон для покупки.
        _chooseShopBackground = index;

        OnShowSelectBackground?.Invoke(_chooseShopBackground);

        // Текущий открытый фон НЕ трогаем.
        OnShowBuy?.Invoke();
    }

    public void Buy()
    {
        // Покупать нечего.
        if (_chooseShopBackground == NoBackground)
            return;

        var newBack = _backgroundInfoProvider.GetBackground(_chooseShopBackground);
        int oldIndex = _backgroundInfoProvider.CurrentBackgroundIndex;

        if(!_moneyProvider.CanAfford(newBack.Price)) return;

        _moneyProvider.ChangeMoney(-newBack.Price);

        // Открываем фон в Store.
        _backgroundProvider.OpenBackground(newBack.Index);

        // Снимаем selected со старого текущего фона.
        OnDeselectBackground?.Invoke(oldIndex);

        OnHideSelectBackground?.Invoke(_chooseShopBackground);

        // Новый фон сразу становится текущим.
        _backgroundProvider.SelectBackground(newBack.Index);

        // Сбрасываем временное состояние покупки.
        _chooseShopBackground = NoBackground;

        // Кнопка покупки больше не нужна.
        OnHideBuy?.Invoke();
    }

    #region Output

    public event Action<IReadOnlyList<Background>> OnSetBackgrounds;
    public event Action<int> OnOpenBackground;
    public event Action<int> OnSelectBackground;
    public event Action<int> OnDeselectBackground;

    public event Action<int> OnShowSelectBackground;
    public event Action<int> OnHideSelectBackground;

    // Кнопка покупки.
    public event Action OnShowBuy;
    public event Action OnHideBuy;

    #endregion
}
