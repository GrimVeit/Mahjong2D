using System.Collections.Generic;
using UnityEngine;

public sealed class StoreMahjongTilesPresenter : IMahjongTilesSpritesProvider
{
    private readonly StoreMahjongTilesModel _model;

    public StoreMahjongTilesPresenter(StoreMahjongTilesModel model)
    {
        _model = model;
    }

    public IReadOnlyList<Sprite> GetRandomTiles(int index, int count)
    {
        return _model.GetRandomTiles(index, count);
    }
}

public interface IMahjongTilesSpritesProvider
{
    public IReadOnlyList<Sprite> GetRandomTiles(int index, int count);
}
