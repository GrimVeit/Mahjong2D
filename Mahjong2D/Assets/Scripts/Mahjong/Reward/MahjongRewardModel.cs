using System;
using UnityEngine;

public class MahjongRewardModel
{
    private readonly ITimerInfo _timerInfo;
    private readonly ILevelInfoProvider _levelInfoProvider;
    private readonly IMoneyProvider _moneyProvider;

    public MahjongRewardModel(ITimerInfo timerInfo, ILevelInfoProvider levelInfoProvider, IMoneyProvider moneyProvider)
    {
        _timerInfo = timerInfo;
        _levelInfoProvider = levelInfoProvider;
        _moneyProvider = moneyProvider;
    }

    public void SetReward()
    {
        int reward = MahjongCoinsHelper.GetCoins(MahjongTileCountHelper.GetTileCount(_levelInfoProvider.Level + 1), _timerInfo.TotalTime, _timerInfo.ElapsedTime);

        _moneyProvider.ChangeMoney(reward);

        OnSetReward?.Invoke(reward);

    }

    #region Output

    public event Action<int> OnSetReward;

    #endregion
}
