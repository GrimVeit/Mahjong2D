using System;

public class VolumeSettingsModel
{
    private readonly ISoundSettingsInfoProvider infoProvider;
    private readonly ISoundSettingsProvider settingsProvider;
    private readonly ISoundSettingsEventsProvider eventsProvider;

    public VolumeSettingsModel(
        ISoundSettingsInfoProvider infoProvider,
        ISoundSettingsProvider settingsProvider,
        ISoundSettingsEventsProvider eventsProvider)
    {
        this.infoProvider = infoProvider;
        this.settingsProvider = settingsProvider;
        this.eventsProvider = eventsProvider;
    }

    public void Initialize()
    {
        eventsProvider.OnChangeSoundVolume += HandleSoundVolumeChanged;
        eventsProvider.OnChangeMusicVolume += HandleMusicVolumeChanged;

        OnSoundVolumeChanged?.Invoke(infoProvider.SoundVolume);
        OnMusicVolumeChanged?.Invoke(infoProvider.MusicVolume);
    }

    public void Dispose()
    {
        eventsProvider.OnChangeSoundVolume -= HandleSoundVolumeChanged;
        eventsProvider.OnChangeMusicVolume -= HandleMusicVolumeChanged;
    }

    public void SetSoundVolume(float value)
    {
        settingsProvider.SetSoundVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        settingsProvider.SetMusicVolume(value);
    }

    private void HandleSoundVolumeChanged(float value)
    {
        OnSoundVolumeChanged?.Invoke(value);
    }

    private void HandleMusicVolumeChanged(float value)
    {
        OnMusicVolumeChanged?.Invoke(value);
    }

    #region Output

    public event Action<float> OnSoundVolumeChanged;
    public event Action<float> OnMusicVolumeChanged;

    #endregion
}
