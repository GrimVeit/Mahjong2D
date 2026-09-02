using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongScorePresenter : IMahjongScoreProvider
{
    private readonly MahjongScoreModel _model;
    private readonly MahjongScoreView _view;

    public MahjongScorePresenter(MahjongScoreModel model, MahjongScoreView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnSetScore += _view.SetScore;
    }

    private void DeactivateEvents()
    {
        _model.OnSetScore -= _view.SetScore;
    }

    #region Input

    public void ApplyScore()
    {
        _model.SetScore();
    }

    #endregion
}

public interface IMahjongScoreProvider
{
    public void ApplyScore();
}
