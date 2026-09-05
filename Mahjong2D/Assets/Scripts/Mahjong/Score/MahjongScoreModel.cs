using System;

public class MahjongScoreModel
{
    private readonly ITimerInfo _timerInfo;
    private readonly ILevelInfoProvider _levelInfoProvider;

    public MahjongScoreModel(ITimerInfo timerInfo, ILevelInfoProvider levelInfoProvider)
    {
        _timerInfo = timerInfo;
        _levelInfoProvider = levelInfoProvider;
    }

    public void Initialize()
    {
        OnSetScore?.Invoke(0);
    }

    public void SetScore()
    {
        OnSetScore?.Invoke(MahjongScoreHelper.GetScore(MahjongTileCountHelper.GetTileCount(_levelInfoProvider.Level + 1), _timerInfo.TotalTime, _timerInfo.ElapsedTime));
    }

    #region Output

    public event Action<int> OnSetScore;

    #endregion
}
