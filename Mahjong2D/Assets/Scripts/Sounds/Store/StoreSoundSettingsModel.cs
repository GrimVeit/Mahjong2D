using System;
using UnityEngine;

public class StoreSoundSettingsModel
{
    public float SoundVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public bool IsMuted { get; private set; }

    private readonly string soundVolumeKey;
    private readonly string musicVolumeKey;
    private readonly string muteKey;

    public event Action<float> OnChangeSoundVolume;
    public event Action<float> OnChangeMusicVolume;
    public event Action<bool> OnChangeMute;

    public StoreSoundSettingsModel(
        string soundVolumeKey,
        string musicVolumeKey,
        string muteKey)
    {
        this.soundVolumeKey = soundVolumeKey;
        this.musicVolumeKey = musicVolumeKey;
        this.muteKey = muteKey;
    }

    public void Initialize()
    {
        SoundVolume = PlayerPrefs.GetFloat(soundVolumeKey, 0.5f);
        MusicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.7f);
        IsMuted = PlayerPrefs.GetInt(muteKey, 0) == 1;

        OnChangeSoundVolume?.Invoke(SoundVolume);
        OnChangeMusicVolume?.Invoke(MusicVolume);
        OnChangeMute?.Invoke(IsMuted);
    }

    public void Dispose()
    {
        PlayerPrefs.SetFloat(soundVolumeKey, SoundVolume);
        PlayerPrefs.SetFloat(musicVolumeKey, MusicVolume);
        PlayerPrefs.SetInt(muteKey, IsMuted ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void SetSoundVolume(float value)
    {
        value = Mathf.Clamp01(value);

        if (Mathf.Approximately(SoundVolume, value))
            return;

        SoundVolume = value;

        OnChangeSoundVolume?.Invoke(SoundVolume);
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);

        if (Mathf.Approximately(MusicVolume, value))
            return;

        MusicVolume = value;

        OnChangeMusicVolume?.Invoke(MusicVolume);
    }

    public void SetMute(bool value)
    {
        if (IsMuted == value)
            return;

        IsMuted = value;

        OnChangeMute?.Invoke(IsMuted);
    }

    public void ToggleMute()
    {
        SetMute(!IsMuted);
    }
}
