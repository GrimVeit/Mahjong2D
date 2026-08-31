using System.Collections.Generic;
using System;

public sealed class StoreCardsDesignPresenter : ICardDesignProvider, ICardDesignInfoProvider, ICardDesignListener
{
    private readonly StoreCardsDesignModel _model;

    public StoreCardsDesignPresenter(StoreCardsDesignModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Provider

    public void OpenCardDesign(int index)
    {
        _model.OpenCardDesign(index);
    }

    public void SelectCardDesign(int index)
    {
        _model.SelectCardDesign(index);
    }

    #endregion

    #region Info

    public CardsDesign GetCardDesign(int index)
    {
        return _model.GetCardDesign(index);
    }

    public IReadOnlyList<CardsDesign> GetCardDesigns()
    {
        return _model.GetCardDesigns();
    }

    public CardsDesign GetCurrentCardDesign()
    {
        return _model.GetCurrentCardDesign();
    }

    public int CurrentCardDesignIndex =>
        _model.GetCurrentCardDesignIndex();

    public bool IsCardDesignOpened(int index)
    {
        return _model.IsCardDesignOpened(index);
    }

    public bool IsBackgroundSelected(int index)
    {
        return _model.IsCardDesignSelected(index);
    }

    #endregion

    #region Listener

    public event Action<CardsDesign> OnOpenCardDesign
    {
        add => _model.OnOpenCardDessign += value;
        remove => _model.OnOpenCardDessign -= value;
    }

    public event Action<CardsDesign> OnSelectCardDesign
    {
        add => _model.OnSelectCardDesign += value;
        remove => _model.OnSelectCardDesign -= value;
    }

    #endregion
}

public interface ICardDesignProvider
{
    public void OpenCardDesign(int index);
    public void SelectCardDesign(int index);
}

public interface ICardDesignInfoProvider
{
    public CardsDesign GetCardDesign(int index);

    public IReadOnlyList<CardsDesign> GetCardDesigns();

    public CardsDesign GetCurrentCardDesign();

    public int CurrentCardDesignIndex {  get; }

    public bool IsCardDesignOpened(int index);

    public bool IsBackgroundSelected(int index);
}

public interface ICardDesignListener
{
    public event Action<CardsDesign> OnOpenCardDesign;
    public event Action<CardsDesign> OnSelectCardDesign;
}
