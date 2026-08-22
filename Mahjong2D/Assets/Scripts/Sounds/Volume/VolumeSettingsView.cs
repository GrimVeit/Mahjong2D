using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingsView : View
{
    [SerializeField] private VolumeSetting soundVolume;
    [SerializeField] private VolumeSetting musicVolume;

    public void Initialize()
    {
        soundVolume.OnChangeVolume += HandleSoundVolumeChanged;
        musicVolume.OnChangeVolume += HandleMusicVolumeChanged;

        soundVolume.Initialize();
        musicVolume.Initialize();
    }

    public void Dispose()
    {
        soundVolume.OnChangeVolume -= HandleSoundVolumeChanged;
        musicVolume.OnChangeVolume -= HandleMusicVolumeChanged;

        soundVolume.Dispose();
        musicVolume.Dispose();
    }

    public void SetSoundVolume(float value)
    {
        soundVolume.SetValue(value);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume.SetValue(value);
    }

    private void HandleSoundVolumeChanged(float value)
    {
        OnChangeSoundVolume?.Invoke(value);
    }

    private void HandleMusicVolumeChanged(float value)
    {
        OnChangeMusicVolume?.Invoke(value);
    }

    #region Output

    public event Action<float> OnChangeSoundVolume;
    public event Action<float> OnChangeMusicVolume;

    #endregion


    [Serializable]
    private class VolumeSetting
    {
        [SerializeField] private Slider sliderVolume;
        [SerializeField] private TextMeshProUGUI textVolume;

        public void Initialize()
        {
            sliderVolume.onValueChanged.AddListener(OnValueChanged);

            UpdateText(sliderVolume.value);
        }

        public void Dispose()
        {
            sliderVolume.onValueChanged.RemoveListener(OnValueChanged);
        }

        public void SetValue(float value)
        {
            sliderVolume.SetValueWithoutNotify(value);

            UpdateText(value);
        }

        private void OnValueChanged(float value)
        {
            UpdateText(value);

            OnChangeVolume?.Invoke(value);
        }

        private void UpdateText(float value)
        {
            textVolume.text = $"{Mathf.RoundToInt(value * 100)}";
        }

        public event Action<float> OnChangeVolume;
    }
}
