public class VolumeSettingsPresenter
{
    private readonly VolumeSettingsModel model;
    private readonly VolumeSettingsView view;

    public VolumeSettingsPresenter(VolumeSettingsModel model, VolumeSettingsView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Initialize()
    {
        view.OnChangeSoundVolume += HandleSoundVolumeChanged;
        view.OnChangeMusicVolume += HandleMusicVolumeChanged;

        model.OnSoundVolumeChanged += view.SetSoundVolume;
        model.OnMusicVolumeChanged += view.SetMusicVolume;

        view.Initialize();
        model.Initialize();
    }

    public void Dispose()
    {
        view.OnChangeSoundVolume -= HandleSoundVolumeChanged;
        view.OnChangeMusicVolume -= HandleMusicVolumeChanged;

        model.OnSoundVolumeChanged -= view.SetSoundVolume;
        model.OnMusicVolumeChanged -= view.SetMusicVolume;

        view.Dispose();
        model.Dispose();
    }

    private void HandleSoundVolumeChanged(float value)
    {
        model.SetSoundVolume(value);
    }

    private void HandleMusicVolumeChanged(float value)
    {
        model.SetMusicVolume(value);
    }
}