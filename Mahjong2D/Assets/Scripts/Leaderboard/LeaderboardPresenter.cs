using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPresenter
{
    private readonly LeaderboardModel _model;
    private readonly LeaderboardView _view;

    public LeaderboardPresenter(LeaderboardModel model, LeaderboardView view)
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
        _view.OnRetry += _model.Refresh;

        _model.OnErrorGetTopPlayers += _view.SetError;
        _model.OnGetTopPlayersStarted += _view.SetHoldOn;
        _model.OnGetTopPlayers += _view.SetData;
    }

    private void DeactivateEvents()
    {
        _view.OnRetry -= _model.Refresh;

        _model.OnErrorGetTopPlayers -= _view.SetError;
        _model.OnGetTopPlayersStarted -= _view.SetHoldOn;
        _model.OnGetTopPlayers -= _view.SetData;
    }
}
