using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundModel
{
    private readonly Dictionary<string, Sound> _sounds;

    private readonly ISoundSettingsInfoProvider _settingsInfo;
    private readonly ISoundSettingsEventsProvider _settingsEvents;

    public SoundModel(List<Sound> sounds, ISoundSettingsInfoProvider settingsInfo, ISoundSettingsEventsProvider settingsEvents)
    {
        _sounds = new Dictionary<string, Sound>();

        foreach (Sound sound in sounds)
        {
            if (sound == null)
                continue;

            if (_sounds.ContainsKey(sound.ID))
            {
                Debug.LogError($"Duplicate sound ID: '{sound.ID}'.");
                continue;
            }

            _sounds.Add(sound.ID, sound);
        }

        _settingsInfo = settingsInfo;
        _settingsEvents = settingsEvents;
    }

    public void Initialize()
    {
        foreach (Sound sound in _sounds.Values)
        {
            sound.Initialize();
        }

        _settingsEvents.OnChangeSoundVolume += HandleChangeSoundVolume;
        _settingsEvents.OnChangeMusicVolume += HandleChangeMusicVolume;
        _settingsEvents.OnChangeMute += HandleChangeMute;

        HandleChangeSoundVolume(_settingsInfo.SoundVolume);
        HandleChangeMusicVolume(_settingsInfo.MusicVolume);
        HandleChangeMute(_settingsInfo.IsMuted);
    }

    public void Dispose()
    {
        _settingsEvents.OnChangeSoundVolume -= HandleChangeSoundVolume;
        _settingsEvents.OnChangeMusicVolume -= HandleChangeMusicVolume;
        _settingsEvents.OnChangeMute -= HandleChangeMute;

        foreach (Sound sound in _sounds.Values)
        {
            sound.Dispose();
        }
    }

    // --------------------------------------------------
    // SOUND
    // --------------------------------------------------

    public ISound GetSound(string id)
    {
        if (_sounds.TryGetValue(id, out Sound sound))
            return sound;

        Debug.LogError(
            $"Sound with ID '{id}' was not found.");

        return null;
    }

    public void Play(string id)
    {
        GetSound(id)?.Play();
    }

    public void PlayOneShot(string id)
    {
        GetSound(id)?.PlayOneShot();
    }

    // --------------------------------------------------
    // SETTINGS
    // --------------------------------------------------

    private void HandleChangeSoundVolume(float value)
    {
        foreach (Sound sound in _sounds.Values)
        {
            if (sound.AudioType != AudioType.Sound)
                continue;

            sound.SetGlobalVolume(value);
        }
    }

    private void HandleChangeMusicVolume(float value)
    {
        foreach (Sound sound in _sounds.Values)
        {
            if (sound.AudioType != AudioType.Music)
                continue;

            sound.SetGlobalVolume(value);
        }
    }

    private void HandleChangeMute(bool value)
    {
        foreach (Sound sound in _sounds.Values)
        {
            sound.SetMuted(value);
        }
    }
}