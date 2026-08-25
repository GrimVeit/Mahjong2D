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
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClickHint += _model.Hint;
    }

    private void DeactivateEvents()
    {
        _view.OnClickHint -= _model.Hint;
    }
}
