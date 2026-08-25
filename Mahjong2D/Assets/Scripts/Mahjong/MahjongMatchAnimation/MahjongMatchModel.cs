using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMatchModel
{
    private readonly IMahjongListener _mahjongListener;

    public MahjongMatchModel(IMahjongListener mahjongListener)
    {
        _mahjongListener = mahjongListener;
    }

    public void Initialize()
    {
        _mahjongListener.OnPairRemoved += SetPair;
    }

    public void Dispose()
    {
        _mahjongListener.OnPairRemoved -= SetPair;
    }

    private void SetPair(MahjongPairRemovedData removedData)
    {
        OnSetPair?.Invoke(removedData.FirstPosition, removedData.SecondPosition, removedData.TileSize, removedData.Sprite);
    }

    #region Output

    public event Action<Vector3, Vector3, Vector2, Sprite> OnSetPair;

    #endregion
}
