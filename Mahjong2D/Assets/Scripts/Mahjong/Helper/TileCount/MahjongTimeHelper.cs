using UnityEngine;

public static class MahjongTimerHelper
{
    private const int StartTime = 45;
    private const int TimeIncreasePerGroup = 5;
    private const int LevelsPerGroup = 5;
    private const int MaxTime = 100;

    public static int GetTime(int level)
    {
        level = Mathf.Max(1, level);

        int group = (level - 1) / LevelsPerGroup;

        int time =
            StartTime +
            group * TimeIncreasePerGroup;

        return Mathf.Min(time, MaxTime);
    }
}
