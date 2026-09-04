using UnityEngine;

public static class MahjongTileCountHelper
{
    private const int StartTileCount = 4;
    private const int TileIncreasePerGroup = 1;
    private const int LevelsPerGroup = 5;
    private const int MaxTileCount = 27;

    public static int GetTileCount(int level)
    {
        level = Mathf.Max(1, level);

        int group = (level - 1) / LevelsPerGroup;

        int tileCount =
            StartTileCount +
            group * TileIncreasePerGroup;

        return Mathf.Min(tileCount, MaxTileCount);
    }
}
