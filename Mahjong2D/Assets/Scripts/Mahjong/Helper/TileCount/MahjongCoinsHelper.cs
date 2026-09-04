using UnityEngine;

public static class MahjongCoinsHelper
{
    private const float CoinsPerTile = 5f;
    private const float CoinsPerTileSquared = 0.25f;

    private const float MaxTimeBonus = 0.5f;

    public static int GetCoins(
        int tileCount,
        int totalTime,
        int elapsedTime)
    {
        tileCount = Mathf.Max(0, tileCount);
        totalTime = Mathf.Max(0, totalTime);
        elapsedTime = Mathf.Clamp(elapsedTime, 0, totalTime);

        if (tileCount <= 0)
            return 0;

        float remainingTimePercent =
            GetRemainingTimePercent(totalTime, elapsedTime);

        float baseCoins =
            tileCount * CoinsPerTile +
            tileCount * tileCount * CoinsPerTileSquared;

        float timeMultiplier =
            1f + remainingTimePercent * MaxTimeBonus;

        return Mathf.RoundToInt(baseCoins * timeMultiplier);
    }

    private static float GetRemainingTimePercent(
        int totalTime,
        int elapsedTime)
    {
        if (totalTime <= 0)
            return 0f;

        int remainingTime = totalTime - elapsedTime;

        return Mathf.Clamp01(
            (float)remainingTime / totalTime);
    }
}
