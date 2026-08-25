using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongHintModel
{
    private readonly IMahjongProvider _mahjongProvider;
    private readonly IMahjongListener _mahjongListener;

    private bool isActive = true;

    public MahjongHintModel(IMahjongProvider mahjongProvider, IMahjongListener mahjongListener)
    {
        _mahjongProvider = mahjongProvider;
        _mahjongListener = mahjongListener;
    }

    public void Initialize()
    {
        _mahjongListener.OnStartMix += Deactive;
        _mahjongListener.OnStartHint += Deactive;

        _mahjongListener.OnStopHint += Active;
        _mahjongListener.OnStopMix += Active;
    }

    public void Dispose()
    {
        _mahjongListener.OnStartMix -= Deactive;
        _mahjongListener.OnStartHint -= Deactive;

        _mahjongListener.OnStopHint -= Active;
        _mahjongListener.OnStopMix -= Active;
    }

    public void Hint()
    {
        if (!isActive) return;

        _mahjongProvider.Hint();
    }

    private void Active()
    {
        isActive = true;

        OnActive?.Invoke();
    }

    private void Deactive()
    {
        isActive = false;

        OnInactive?.Invoke();
    }

    #region Output

    public event Action OnActive;
    public event Action OnInactive;

    #endregion
}
