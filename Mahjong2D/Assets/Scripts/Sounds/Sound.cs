using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class Sound : ISound
{
    public string ID => id;
    public AudioType AudioType => audioType;

    public float Volume => localVolume;

    [SerializeField] private string id;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;
    [SerializeField] private float pitch = 1f;
    [SerializeField] private bool isLoop;
    [SerializeField] private bool isPlayAwake;
    [SerializeField] private AudioType audioType;

    private float localVolume;
    private float globalVolume = 1f;

    private bool isMuted;

    private Tween volumeTween;
    private Tween delayedPlayTween;

    public void Initialize()
    {
        if (audioSource == null)
        {
            Debug.LogError($"AudioSource is missing for sound '{id}'.");
            return;
        }

        audioSource.clip = audioClip;
        audioSource.pitch = pitch;
        audioSource.loop = isLoop;

        localVolume = Mathf.Clamp01(volume);

        ApplyVolume();

        if (isPlayAwake)
        {
            audioSource.volume = 0;
            audioSource.Play();
            SetVolumeEnd(localVolume, 0.4f);
        }
    }

    public void Dispose()
    {
        volumeTween?.Kill();
        delayedPlayTween?.Kill();

        volumeTween = null;
        delayedPlayTween = null;

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    // --------------------------------------------------
    // LOCAL
    // --------------------------------------------------

    public void SetVolume(float value)
    {
        volumeTween?.Kill();

        localVolume = Mathf.Clamp01(value);

        ApplyVolume();
    }

    // --------------------------------------------------
    // GLOBAL
    // --------------------------------------------------

    public void SetGlobalVolume(float value)
    {
        globalVolume = Mathf.Clamp01(value);

        ApplyVolume();
    }

    public void SetMuted(bool value)
    {
        isMuted = value;

        ApplyVolume();
    }

    // --------------------------------------------------
    // VOLUME
    // --------------------------------------------------

    public void SetVolume(
        float startVolume,
        float endVolume,
        Action onComplete = null)
    {
        SetVolume(
            startVolume,
            endVolume,
            0.4f,
            0f,
            onComplete);
    }

    public void SetVolume(
        float startVolume,
        float endVolume,
        float duration,
        Action onComplete = null)
    {
        SetVolume(
            startVolume,
            endVolume,
            duration,
            0f,
            onComplete);
    }

    public void SetVolume(
        float startVolume,
        float endVolume,
        float duration,
        float delay,
        Action onComplete = null)
    {
        volumeTween?.Kill();

        startVolume = Mathf.Clamp01(startVolume);
        endVolume = Mathf.Clamp01(endVolume);

        localVolume = startVolume;

        ApplyVolume();

        volumeTween = DOTween
            .To(
                () => localVolume,
                value =>
                {
                    localVolume = value;
                    ApplyVolume();
                },
                endVolume,
                duration)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                localVolume = endVolume;
                ApplyVolume();

                onComplete?.Invoke();
            });
    }

    public void SetVolumeEnd(
        float endVolume,
        float duration,
        Action onComplete = null)
    {
        SetVolume(
            localVolume,
            endVolume,
            duration,
            0f,
            onComplete);
    }

    // --------------------------------------------------
    // PLAY
    // --------------------------------------------------

    public void Play()
    {
        if (audioSource == null)
            return;

        audioSource.Play();
    }

    public void Play(float delay)
    {
        delayedPlayTween?.Kill();

        delayedPlayTween = DOVirtual
            .DelayedCall(
                delay,
                Play)
            .SetUpdate(true);
    }

    public void PlayOneShot()
    {
        if (audioSource == null || audioClip == null)
            return;

        audioSource.pitch = pitch;

        audioSource.PlayOneShot(
            audioClip,
            GetFinalVolume());
    }

    public void Stop()
    {
        delayedPlayTween?.Kill();

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    // --------------------------------------------------
    // PITCH
    // --------------------------------------------------

    public void SetPitch(float value)
    {
        pitch = value;

        if (audioSource != null)
        {
            audioSource.pitch = value;
        }
    }

    // --------------------------------------------------
    // INTERNAL
    // --------------------------------------------------

    private float GetFinalVolume()
    {
        if (isMuted)
            return 0f;

        return localVolume * globalVolume;
    }

    private void ApplyVolume()
    {
        if (audioSource == null)
            return;

        audioSource.volume = GetFinalVolume();
    }
}

public enum AudioType
{
    Sound, Music
}

public interface ISound
{
    string ID { get; }
    AudioType AudioType { get; }

    float Volume { get; }

    void Play();
    void Play(float delay);
    void PlayOneShot();

    void Stop();

    void SetVolume(float value);

    void SetPitch(float value);

    void SetVolume(
        float startVolume,
        float endVolume,
        Action onComplete = null);

    void SetVolume(
        float startVolume,
        float endVolume,
        float duration,
        Action onComplete = null);

    void SetVolume(
        float startVolume,
        float endVolume,
        float duration,
        float delay,
        Action onComplete = null);

    void SetVolumeEnd(
        float endVolume,
        float duration,
        Action onComplete = null);
}
