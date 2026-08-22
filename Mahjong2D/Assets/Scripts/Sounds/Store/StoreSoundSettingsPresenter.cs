using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSoundSettingsPresenter : ISoundSettingsInfoProvider, ISoundSettingsEventsProvider, ISoundSettingsProvider
{
    private readonly StoreSoundSettingsModel _model;

    public StoreSoundSettingsPresenter(StoreSoundSettingsModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Info

    public float SoundVolume => _model.SoundVolume;
    public float MusicVolume => _model.MusicVolume;
    public bool IsMuted => _model.IsMuted;

    #endregion

    #region Events

    public event Action<float> OnChangeSoundVolume
    {
        add => _model.OnChangeSoundVolume += value;
        remove => _model.OnChangeSoundVolume -= value;
    }

    public event Action<float> OnChangeMusicVolume
    {
        add => _model.OnChangeMusicVolume += value;
        remove => _model.OnChangeMusicVolume -= value;
    }

    public event Action<bool> OnChangeMute
    {
        add => _model.OnChangeMute += value;
        remove => _model.OnChangeMute -= value;
    }

    #endregion

    #region Input

    public void SetSoundVolume(float value)
    {
        _model.SetSoundVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        _model.SetMusicVolume(value);
    }

    public void SetMute(bool value)
    {
        _model.SetMute(value);
    }

    public void ToggleMute()
    {
        _model.ToggleMute();
    }

    #endregion
}

public interface ISoundSettingsInfoProvider
{
    float SoundVolume { get; }
    float MusicVolume { get; }
    bool IsMuted { get; }
}

public interface ISoundSettingsEventsProvider
{
    event Action<float> OnChangeSoundVolume;
    event Action<float> OnChangeMusicVolume;
    event Action<bool> OnChangeMute;
}

public interface ISoundSettingsProvider
{
    void SetSoundVolume(float value);
    void SetMusicVolume(float value);

    void SetMute(bool value);
    void ToggleMute();
}
