using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMatchModel
{
    private readonly IMahjongListener _mahjongListener;
    private readonly ISoundProvider _soundProvider;

    public MahjongMatchModel(IMahjongListener mahjongListener, ISoundProvider soundProvider)
    {
        _mahjongListener = mahjongListener;
        _soundProvider = soundProvider;
    }

    public void Initialize()
    {
        _mahjongListener.OnPairRemoved += SetPair;
    }

    public void Dispose()
    {
        _mahjongListener.OnPairRemoved -= SetPair;
    }

    public void Punch()
    {
        _soundProvider.PlayOneShot("Correct");
    }

    private void SetPair(MahjongPairRemovedData removedData)
    {
        OnSetPair?.Invoke(removedData.FirstPosition, removedData.SecondPosition, removedData.TileSize, removedData.Sprite);
    }

    #region Output

    public event Action<Vector3, Vector3, Vector2, Sprite> OnSetPair;

    #endregion
}
