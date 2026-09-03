using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileNicknameInputPresenter
{
    private readonly ProfileNicknameInputModel _model;
    private readonly ProfileNicknameInputView _view;

    public ProfileNicknameInputPresenter(ProfileNicknameInputModel model, ProfileNicknameInputView view)
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
        _view.OnChangeNickname += _model.SetNickname;
        _view.OnSubmitNickname += _model.SubmitNickname;

        _model.OnSetValidate += _view.SetValidate;
        _model.OnSetNotValidate += _view.SetNotValidate;
    }

    private void DeactivateEvents()
    {
        _view.OnChangeNickname -= _model.SetNickname;
        _view.OnSubmitNickname -= _model.SubmitNickname;

        _model.OnSetValidate -= _view.SetValidate;
        _model.OnSetNotValidate -= _view.SetNotValidate;
    }
}
