using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDesignShopVisualPresenter
{
    private readonly CardDesignShopVisualModel _model;
    private readonly CardDesignShopVisualView _view;

    public CardDesignShopVisualPresenter(CardDesignShopVisualModel model, CardDesignShopVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnChooseBackground += _model.ChooseDesign;
        _view.OnBuy += _model.Buy;

        _model.OnSetDesigns += _view.SetBackgrounds;
        _model.OnOpenDesign += _view.OpenBackground;
        _model.OnSelectDesign += _view.SelectBackground;
        _model.OnDeselectDesign += _view.DeselectBackground;
        _model.OnShowSelectDesign += _view.SelectShopBackground;
        _model.OnHideSelectDesign += _view.DeselectShopBackground;
        _model.OnShowBuy += _view.ShowBuy;
        _model.OnHideBuy += _view.HideBuy;
    }

    private void DeactivateEvents()
    {
        _view.OnChooseBackground -= _model.ChooseDesign;
        _view.OnBuy -= _model.Buy;

        _model.OnSetDesigns -= _view.SetBackgrounds;
        _model.OnOpenDesign -= _view.OpenBackground;
        _model.OnSelectDesign -= _view.SelectBackground;
        _model.OnDeselectDesign -= _view.DeselectBackground;
        _model.OnShowSelectDesign -= _view.SelectShopBackground;
        _model.OnHideSelectDesign -= _view.DeselectShopBackground;
        _model.OnShowBuy -= _view.ShowBuy;
        _model.OnHideBuy -= _view.HideBuy;
    }
}
