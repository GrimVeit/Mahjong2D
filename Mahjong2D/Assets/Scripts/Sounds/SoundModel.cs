using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundModel
{
    public float VolumeSound => volumeSound;
    public float VolumeMusic => volumeMusic;

    public event Action OnMuteSounds;
    public event Action OnUnmuteSounds;

    public Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

    private string KEY_MUTE;
    private string KEY_VOLUME_SOUND;
    private string KEY_VOLUME_MUSIC;

    private bool isMute = false;

    private float volumeSound;
    private float volumeMusic;

    public SoundModel(List<Sound> sounds, string KEY_MUTE, string KEY_VOLUME_SOUND, string KEY_VOLUME_MUSIC)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            this.sounds[sounds[i].ID] = sounds[i];
        }

        this.KEY_MUTE = KEY_MUTE;
        this.KEY_VOLUME_SOUND = KEY_VOLUME_SOUND;
        this.KEY_VOLUME_MUSIC = KEY_VOLUME_MUSIC;
    }

    public void Initialize()
    {
        isMute = PlayerPrefs.GetInt(KEY_MUTE, 1) == 0;

        volumeSound = PlayerPrefs.GetFloat(KEY_VOLUME_SOUND, 0.5f);
        volumeMusic = PlayerPrefs.GetFloat(KEY_VOLUME_MUSIC, 0.7f);

        SetVolume(volumeSound, AudioType.Sound);
        SetVolume(volumeMusic, AudioType.Music);

        foreach (var sound in sounds.Values)
        {
            sound.Initialize();
        }

        CheckMuteUnmute();
    }

    public void Dispose()
    {
        int value;

        if (isMute) value = 0;
        else value = 1;

        PlayerPrefs.SetInt(KEY_MUTE, value);
        PlayerPrefs.SetFloat(KEY_VOLUME_SOUND, volumeSound);
        PlayerPrefs.SetFloat(KEY_VOLUME_MUSIC, volumeMusic);

        foreach (var sound in sounds.Values)
        {
            sound.Dispose();
        }
    }

    public void MuteUnmute()
    {
        isMute = !isMute;
        CheckMuteUnmute();
    }

    private void CheckMuteUnmute()
    {
        if (isMute)
        {
            MuteAll();
            OnMuteSounds?.Invoke();
        }
        else
        {
            UnmuteAll();
            OnUnmuteSounds?.Invoke();
        }
    }

    private void MuteAll()
    {
        foreach (var sound in sounds.Values)
        {
            sound.MainMute();
        }
    }

    private void UnmuteAll()
    {
        foreach (var sound in sounds.Values)
        {
            sound.MainUnmute();
        }
    }

    public ISound GetSound(string id)
    {
        if (sounds.ContainsKey(id))
        {
            return sounds[id];
        }

        Debug.LogError("Нет звукового файла с идентификатором " + id);
        return null;
    }

    public void Play(string id)
    {
        if (sounds.ContainsKey(id))
        {
            sounds[id].Play();
            return;
        }

        Debug.LogError("Нет звукового файла с идентификатором " + id);
    }

    public void PlayOneShot(string id)
    {
        if (sounds.ContainsKey(id))
        {
            sounds[id].PlayOneShot();
            return;
        }

        Debug.LogError("Нет звукового файла с идентификатором " + id);
    }

    #region Output

    public event Action<float> OnChangeVolumeSound;
    public event Action<float> OnChangeVolumeMusic;

    #endregion

    #region Input

    public void SetVolume(float value, AudioType type)
    {
        foreach (var sound in sounds.Values)
        {
            if (sound.AudioType == type)
            {
                sound.SetMainRatio(value);
            }
        }

        if (type == AudioType.Sound)
        {
            OnChangeVolumeSound?.Invoke(value);
            volumeSound = value;
        }
        else
        {
            volumeMusic = value;
            OnChangeVolumeMusic?.Invoke(value);
        }
    }

    #endregion
}
