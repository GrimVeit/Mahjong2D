using System;

public class MahjongMixModel
{
    private readonly IMahjongProvider _mahjongProvider;
    private readonly IMahjongListener _mahjongListener;

    private bool isActive = true;

    public MahjongMixModel(IMahjongProvider mahjongProvider, IMahjongListener mahjongListener)
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

    public void Mix()
    {
        if(!isActive) return;

        _mahjongProvider.Mix();
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
