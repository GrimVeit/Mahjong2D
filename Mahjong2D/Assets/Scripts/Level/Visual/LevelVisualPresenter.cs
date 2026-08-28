using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualPresenter
{
    private readonly LevelVisualModel _model;
    private readonly LevelVisualView _view;

    public LevelVisualPresenter(LevelVisualModel model, LevelVisualView view)
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
        _model.OnSetLevel += _view.SetLevel;
    }

    private void DeactivateEvents()
    {
        _model.OnSetLevel -= _view.SetLevel;
    }
}
