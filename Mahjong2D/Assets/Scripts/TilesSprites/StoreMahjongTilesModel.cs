using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class StoreMahjongTilesModel
{
    private readonly Dictionary<int, MahjongTilesGroupSO> _groups = new();

    public StoreMahjongTilesModel(IReadOnlyList<MahjongTilesGroupSO> groups)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            MahjongTilesGroupSO group = groups[i];

            if (group == null) continue;

            _groups[group.Index] = group;
        }
    }

    public IReadOnlyList<Sprite> GetRandomTiles(int index, int count)
    {
        if (count <= 0)
            return Array.Empty<Sprite>();

        if (!_groups.TryGetValue(index, out MahjongTilesGroupSO group))
            return Array.Empty<Sprite>();

        if (group.Sprites == null || group.Sprites.Count == 0)
            return Array.Empty<Sprite>();

        int resultCount = Mathf.Min(count, group.Sprites.Count);

        var sprites = new List<Sprite>(group.Sprites);

        Shuffle(sprites);

        return sprites.GetRange(0, resultCount);
    }

    private static void Shuffle(List<Sprite> sprites)
    {
        for (int i = sprites.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            (sprites[i], sprites[randomIndex]) =
                (sprites[randomIndex], sprites[i]);
        }
    }
}
