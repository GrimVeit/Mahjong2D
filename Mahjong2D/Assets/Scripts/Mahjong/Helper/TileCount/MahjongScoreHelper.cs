using UnityEngine;

public static class MahjongScoreHelper
{
    private const int ScorePerTile = 10;
    private const int ScorePerRemainingSecond = 5;

    public static int GetScore(
        int tileCount,
        int totalTime,
        int elapsedTime)
    {
        tileCount = Mathf.Max(0, tileCount);
        totalTime = Mathf.Max(0, totalTime);
        elapsedTime = Mathf.Clamp(elapsedTime, 0, totalTime);

        int remainingTime = totalTime - elapsedTime;

        int tileScore = tileCount * ScorePerTile;
        int timeScore = remainingTime * ScorePerRemainingSecond;

        return tileScore + timeScore;
    }
}
