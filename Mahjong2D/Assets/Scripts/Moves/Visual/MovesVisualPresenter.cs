using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesVisualPresenter
{
    private readonly MovesVisualModel _model;
    private readonly MovesVisualView _view;

    public MovesVisualPresenter(MovesVisualModel model, MovesVisualView view)
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
        _model.OnSetMove += _view.SetMoves;
    }

    private void DeactivateEvents()
    {
        _model.OnSetMove -= _view.SetMoves;
    }
}
