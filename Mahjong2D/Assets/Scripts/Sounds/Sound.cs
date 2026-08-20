using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Sound : ISound
{
    public AudioType AudioType => audioType;
    public string ID => id;
    public float Volume => _baseVolume;


    [SerializeField] private string id;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float volume;
    [SerializeField] private float pitch;
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isPlayAwake;
    [SerializeField] private AudioType audioType;

    private readonly float durationChangeVolume = 0.4f;

    private float _baseVolume = 1f;
    private float _currentRatio = 1f;

    private bool _isMuted;
    private bool isMainControl;

    private IEnumerator setVolume_Coroutine;
    private IEnumerator play_Coroutine;

    public void Initialize()
    {
        audioSource.clip = audioClip;
        audioSource.pitch = pitch;
        audioSource.loop = isLoop;

        _baseVolume = Mathf.Clamp01(volume);

        ApplyVolume();

        if (isPlayAwake)
        {
            audioSource.Play();
        }
    }

    // -------------------------
    // CORE
    // -------------------------

    private void ApplyVolume()
    {
        if (audioSource == null) return;

        if (_isMuted)
        {
            audioSource.volume = 0f;
            return;
        }

        audioSource.volume = _baseVolume * _currentRatio;
    }

    public void SetMainRatio(float ratio)
    {
        _currentRatio = Mathf.Clamp01(ratio);
        ApplyVolume();
    }

    // -------------------------
    // MAIN CONTROL
    // -------------------------

    public void MainMute()
    {
        isMainControl = true;
        audioSource.mute = true;
    }

    public void MainUnmute()
    {
        audioSource.mute = false;
        isMainControl = false;
        ApplyVolume();
    }

    public void Mute()
    {
        if (isMainControl) return;

        _isMuted = true;
        ApplyVolume();
    }

    public void Unmute()
    {
        if (isMainControl) return;

        _isMuted = false;
        ApplyVolume();
    }

    // -------------------------
    // BASIC CONTROL
    // -------------------------

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public void SetVolume(float volume)
    {
        _baseVolume = Mathf.Clamp01(volume);
        ApplyVolume();
    }

    // -------------------------
    // PLAY
    // -------------------------

    public void Play()
    {
        Debug.Log("PLAY");

        audioSource.Play();
    }

    public void Play(float await)
    {
        if (play_Coroutine != null)
            Coroutines.Instance.StopCoroutine(play_Coroutine);

        play_Coroutine = Play_Coroutine(await);
        Coroutines.Instance.StartCoroutine(play_Coroutine);
    }

    public void PlayOneShot()
    {
        audioSource.pitch = pitch;

        float finalVol = _isMuted ? 0f : _baseVolume * _currentRatio;
        audioSource.PlayOneShot(audioClip, finalVol);
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    // -------------------------
    // DISPOSE
    // -------------------------

    public void Dispose()
    {
        SetVolume(_baseVolume, 0, () =>
        {
            if (setVolume_Coroutine != null)
                Coroutines.Instance.StopCoroutine(setVolume_Coroutine);
        });
    }

    // -------------------------
    // VOLUME API (UNCHANGED)
    // -------------------------

    public void SetVolume(float startVolume, float endVolume, Action action = null)
    {
        StartVolumeRoutine(startVolume, endVolume, durationChangeVolume, 0, action);
    }

    public void SetVolume_End(float endVolume, float time, Action action = null)
    {
        StartVolumeRoutine(audioSource.volume, endVolume, time, 0, action);
    }

    public void SetVolume(float startVolume, float endVolume, float time, Action action = null)
    {
        StartVolumeRoutine(startVolume, endVolume, time, 0, action);
    }

    public void SetVolume(float startVolume, float endVolume, float time, float awate, Action action = null)
    {
        StartVolumeRoutine(startVolume, endVolume, time, awate, action);
    }

    private void StartVolumeRoutine(float startVolume, float endVolume, float time, float awate, Action action)
    {
        if (setVolume_Coroutine != null)
            Coroutines.Instance.StopCoroutine(setVolume_Coroutine);

        setVolume_Coroutine = ChangeVolume_Coroutine(startVolume, endVolume, time, awate, action);
        Coroutines.Instance.StopCoroutine(setVolume_Coroutine);
    }

    // -------------------------
    // COROUTINES
    // -------------------------

    private IEnumerator Play_Coroutine(float await)
    {
        yield return new WaitForSeconds(await);
        audioSource.Play();
    }

    private IEnumerator ChangeVolume_Coroutine(float startVolume, float endVolume, float time, float awate, Action actionOnend)
    {
        if (audioSource == null) yield break;

        yield return new WaitForSeconds(awate);

        float elapsedTime = 0f;

        float initialStart = Mathf.Clamp01(startVolume);
        float targetEnd = Mathf.Clamp01(endVolume);

        _baseVolume = initialStart;
        ApplyVolume();

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            _baseVolume = Mathf.Lerp(initialStart, targetEnd, elapsedTime / time);
            ApplyVolume();

            yield return null;
        }

        _baseVolume = targetEnd;
        ApplyVolume();

        actionOnend?.Invoke();
    }
}

public enum AudioType
{
    Sound, Music
}

public interface ISound
{
    public float Volume { get; }
    public void Play();
    public void Play(float await);
    public void PlayOneShot();
    public void Stop();
    public void SetVolume(float vol);
    public void SetVolume(float startVolume, float endVolume, Action action = null);
    public void SetVolume(float startVolume, float endVolume, float time, Action action = null);
    public void SetVolume(float startVolume, float endVolume, float time, float timeAwait, Action action = null);
    public void SetVolume_End(float endVolume, float time, Action action = null);
    public void SetPitch(float pitch);
}
