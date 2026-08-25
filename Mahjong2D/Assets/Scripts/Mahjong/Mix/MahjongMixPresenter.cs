using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMixPresenter
{
    private readonly MahjongMixModel _model;
    private readonly MahjongMixView _view;

    public MahjongMixPresenter(MahjongMixModel model, MahjongMixView view)
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
        _model.OnActive += _view.Active;
        _model.OnInactive += _view.Deactive;

        _view.OnClickMix += _model.Mix;
    }

    private void DeactivateEvents()
    {
        _model.OnActive -= _view.Active;
        _model.OnInactive -= _view.Deactive;

        _view.OnClickMix -= _model.Mix;
    }
}
