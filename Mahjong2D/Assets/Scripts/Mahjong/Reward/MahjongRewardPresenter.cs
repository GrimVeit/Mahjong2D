using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongRewardPresenter : IMahjongRewardProvider
{
    private readonly MahjongRewardModel _model;
    private readonly MahjongRewardView _view;

    public MahjongRewardPresenter(MahjongRewardModel model, MahjongRewardView view)
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
        _model.OnSetReward += _view.SetReward;
    }

    private void DeactivateEvents()
    {
        _model.OnSetReward -= _view.SetReward;
    }

    #region input

    public void ApplyReward() => _model.SetReward();

    #endregion
}

public interface IMahjongRewardProvider
{
    public void ApplyReward();
}
