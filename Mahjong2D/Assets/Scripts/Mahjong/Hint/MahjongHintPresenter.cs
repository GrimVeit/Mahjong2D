using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongHintPresenter
{
    private readonly MahjongHintModel _model;
    private readonly MahjongHintView _view;

    public MahjongHintPresenter(MahjongHintModel model, MahjongHintView view)
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

        _view.OnClickHint += _model.Hint;
    }

    private void DeactivateEvents()
    {
        _model.OnActive -= _view.Active;
        _model.OnInactive -= _view.Deactive;

        _view.OnClickHint -= _model.Hint;
    }
}
