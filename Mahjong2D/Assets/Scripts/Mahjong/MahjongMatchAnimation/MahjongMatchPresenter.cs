using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMatchPresenter : IMahjongMatchListener
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
        _view.OnPunch += _model.Punch;

        _model.OnSetPair += _view.Play;
    }

    private void DeactivateEvents()
    {
        _view.OnPunch -= _model.Punch;

        _model.OnSetPair -= _view.Play;
    }

    #region Output

    public event Action OnStartMatch
    {
        add => _view.OnStartMatch += value;
        remove => _view.OnStartMatch -= value;
    }

    public event Action OnEndMatch
    {
        add => _view.OnEndMatch += value;
        remove => _view.OnEndMatch -= value;
    }

    #endregion
}

public interface IMahjongMatchListener
{
    public event Action OnStartMatch;
    public event Action OnEndMatch;
}
