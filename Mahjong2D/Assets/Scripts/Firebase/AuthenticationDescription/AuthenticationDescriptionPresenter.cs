using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationDescriptionPresenter
{
    private readonly AuthenticationDescriptionModel _model;
    private readonly AuthenticationDescriptionView _view;

    public AuthenticationDescriptionPresenter(AuthenticationDescriptionModel model, AuthenticationDescriptionView view)
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
        _model.OnSetDescription += _view.SetDescription;
    }

    private void DeactivateEvents()
    {
        _model.OnSetDescription -= _view.SetDescription;
    }
}
