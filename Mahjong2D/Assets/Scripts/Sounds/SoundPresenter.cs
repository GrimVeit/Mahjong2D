using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPresenter : ISoundProvider, ISoundVolumeProvider
{
    private readonly SoundModel _model;

    public SoundPresenter(SoundModel soundModel)
    {
        _model = soundModel;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Interface

    public void Play(string id)
    {
        _model.Play(id);
    }

    public void PlayOneShot(string id)
    {
        _model.PlayOneShot(id);
    }

    public ISound GetSound(string id)
    {
        return _model.GetSound(id);
    }

    #endregion

    #region ISoundVolumeProvider

    public float VolumeSound() => _model.VolumeSound;
    public float VolumeMusic() => _model.VolumeMusic;

    public void SetVolume(float value, AudioType type)
    {
        _model.SetVolume(value, type);
    }

    #endregion
}

public interface ISoundProvider
{
    void Play(string id);
    void PlayOneShot(string id);
    ISound GetSound(string id);
}

public interface ISoundVolumeProvider
{
    public float VolumeSound();
    public float VolumeMusic();

    public void SetVolume(float value, AudioType type);
}
