using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundShopVisualPresenter
{
    private readonly BackgroundShopVisualModel _model;
    private readonly BackgroundShopVisualView _view;

    public BackgroundShopVisualPresenter(BackgroundShopVisualModel model, BackgroundShopVisualView view)
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
        _view.OnChooseBackground += _model.ChooseBackground;
        _view.OnBuy += _model.Buy;

        _model.OnSetBackgrounds += _view.SetBackgrounds;
        _model.OnOpenBackground += _view.OpenBackground;
        _model.OnSelectBackground += _view.SelectBackground;
        _model.OnDeselectBackground += _view.DeselectBackground;
        _model.OnShowSelectBackground += _view.SelectShopBackground;
        _model.OnHideSelectBackground += _view.DeselectShopBackground;
        _model.OnShowBuy += _view.ShowBuy;
        _model.OnHideBuy += _view.HideBuy;
    }

    private void DeactivateEvents()
    {
        _view.OnChooseBackground -= _model.ChooseBackground;
        _view.OnBuy -= _model.Buy;

        _model.OnSetBackgrounds -= _view.SetBackgrounds;
        _model.OnOpenBackground -= _view.OpenBackground;
        _model.OnSelectBackground -= _view.SelectBackground;
        _model.OnDeselectBackground -= _view.DeselectBackground;
        _model.OnShowSelectBackground -= _view.SelectShopBackground;
        _model.OnHideSelectBackground -= _view.DeselectShopBackground;
        _model.OnShowBuy -= _view.ShowBuy;
        _model.OnHideBuy -= _view.HideBuy;
    }
}
