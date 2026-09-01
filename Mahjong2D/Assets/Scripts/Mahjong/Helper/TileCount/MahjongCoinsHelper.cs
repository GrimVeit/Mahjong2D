using UnityEngine;

public static class MahjongCoinsHelper
{
    private const int CoinsPerTile = 10;
    private const int CoinsPerRemainingSecond = 2;

    public static int GetCoins(int tileCount, int totalTime, int elapsedTime)
    {
        tileCount = Mathf.Max(0, tileCount);
        totalTime = Mathf.Max(0, totalTime);
        elapsedTime = Mathf.Clamp(elapsedTime, 0, totalTime);

        int remainingTime = totalTime - elapsedTime;

        int tileCoins = tileCount * CoinsPerTile;
        int timeCoins = remainingTime * CoinsPerRemainingSecond;

        return tileCoins + timeCoins;
    }
}
