using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMatchPresenter
{
    private readonly MahjongMatchModel _model;
    private readonly MahjongMatchView _view;

    public MahjongMatchPresenter(MahjongMatchModel model, MahjongMatchView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnSetPair += _view.Play;
    }

    private void DeactivateEvents()
    {
        _model.OnSetPair -= _view.Play;
    }
}
