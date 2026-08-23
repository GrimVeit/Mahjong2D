using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyVisualPresenter
{
    private readonly MoneyVisualModel _model;
    private readonly MoneyVisualView _view;

    public MoneyVisualPresenter(MoneyVisualModel model, MoneyVisualView view)
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

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnAdd += _view.AddMoney;
        _model.OnRemove += _view.RemoveMoney;
        _model.OnChangeVisual += _view.SetMoney;
    }

    private void DeactivateEvents()
    {
        _model.OnAdd -= _view.AddMoney;
        _model.OnRemove -= _view.RemoveMoney;
        _model.OnChangeVisual -= _view.SetMoney;
    }
}
