using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public sealed class StoreBackgroundModel
{
    public event Action<Background> OnOpenBackground;
    public event Action<Background> OnSelectBackground;

private readonly Dictionary<int, Background> _backgrounds;

    private readonly string _filePath;
    private readonly string _selectedBackgroundKey;
    private readonly string _xorKey;

    private int _currentBackgroundIndex;

    public Background CurrentBackground => _backgrounds.TryGetValue(_currentBackgroundIndex, out var background) ? background : null;

    public StoreBackgroundModel(IEnumerable<BackgroundDataSO> data, string saveFileName = "Backgrounds.json", string selectedBackgroundKey = PlayerPrefsKeys.BACKGROUNDS, string xorKey = "eurghfuirehfisdfioerfywre73647898037uhgdg")
    {
        _backgrounds = new Dictionary<int, Background>();

        _filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        _selectedBackgroundKey = selectedBackgroundKey;
        _xorKey = xorKey;

        foreach (var backgroundData in data)
        {
            if (backgroundData == null)
                continue;

            if (_backgrounds.ContainsKey(backgroundData.Index))
            {
                Debug.LogError(
                    $"Duplicate background index: {backgroundData.Index}"
                );

                continue;
            }

            _backgrounds.Add(
                backgroundData.Index,
                new Background(
                    backgroundData.Index,
                    backgroundData.Name,
                    backgroundData.Sprite,
                    backgroundData.Price,
                    false
                )
            );
        }
    }

    #region INIT

    public void Initialize()
    {
        Load();

        int defaultIndex = GetDefaultBackgroundIndex();

        // Базовый фон всегда должен быть открыт.
        if (_backgrounds.TryGetValue(defaultIndex, out var defaultBackground))
            defaultBackground.Open();

        _currentBackgroundIndex = PlayerPrefs.GetInt(_selectedBackgroundKey, defaultIndex);

        // Если выбранный фон отсутствует — используем дефолтный.
        if (!_backgrounds.ContainsKey(_currentBackgroundIndex))
            _currentBackgroundIndex = defaultIndex;

        foreach (var item in _backgrounds)
        {
            Debug.Log($"BACKGROUND INDEX - {item.Key}, IS OPEN - {item.Value.IsOpened}");
        }
    }

    public void Dispose()
    {
        Save();

        PlayerPrefs.SetInt(
            _selectedBackgroundKey,
            _currentBackgroundIndex
        );

        PlayerPrefs.Save();
    }

    #endregion

    #region INPUT

    public void OpenBackground(int index)
    {
        if (!_backgrounds.TryGetValue(index, out var background))
        {
            Debug.LogError(
                $"Background not found: {index}"
            );

            return;
        }

        if (background.IsOpened)
            return;

        background.Open();

        OnOpenBackground?.Invoke(background);
    }

    public void SelectBackground(int index)
    {
        if (!_backgrounds.TryGetValue(index, out var background))
        {
            Debug.LogError(
                $"Background not found: {index}"
            );

            return;
        }

        if (!background.IsOpened)
            return;

        if (_currentBackgroundIndex == index)
            return;

        _currentBackgroundIndex = index;

        OnSelectBackground?.Invoke(background);
    }

    #endregion

    #region INFO

    public Background GetBackground(int index)
    {
        return _backgrounds.TryGetValue(index, out var background) ? background : null;
    }

    public IReadOnlyList<Background> GetBackgrounds()
    {
        return _backgrounds.Values
            .OrderBy(background => background.Index)
            .ToList();
    }

    public Background GetCurrentBackground()
    {
        return CurrentBackground;
    }

    public int GetCurrentBackgroundIndex()
    {
        return _currentBackgroundIndex;
    }

    public bool IsBackgroundOpened(int index)
    {
        return _backgrounds.TryGetValue(index, out var background) && background.IsOpened;
    }

    public bool IsBackgroundSelected(int index)
    {
        return _currentBackgroundIndex == index;
    }

    #endregion

    #region LOAD / SAVE

    private void Load()
    {
        if (!File.Exists(_filePath))
            return;

        try
        {
            string encrypted = File.ReadAllText(_filePath);
            string json = Xor(encrypted, _xorKey);

            var wrapper = JsonUtility.FromJson<BackgroundSaveWrapper>(json);

            if (wrapper?.Entries == null)
                throw new Exception("Invalid save data.");

            foreach (var entry in wrapper.Entries)
            {
                if (entry == null)
                    continue;

                if (!_backgrounds.TryGetValue(entry.Index, out var background))
                    continue;

                if (entry.IsOpened)
                    background.Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to load backgrounds. Resetting to default state. {exception}");

            ResetToDefaultState();
        }
    }

    private void Save()
    {
        var wrapper = new BackgroundSaveWrapper();

        foreach (var background in _backgrounds.Values)
        {
            wrapper.Entries.Add(
                new BackgroundSaveEntry
                {
                    Index = background.Index,
                    IsOpened = background.IsOpened
                }
            );
        }

        string json = JsonUtility.ToJson(wrapper);
        string encrypted = Xor(json, _xorKey);

        File.WriteAllText(
            _filePath,
            encrypted
        );
    }

    private string Xor(string data, string key)
    {
        var result = new char[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (char)(data[i] ^ key[i % key.Length]);
        }

        return new string(result);
    }

    #endregion

    #region DEFAULT

    private int GetDefaultBackgroundIndex()
    {
        if (_backgrounds.Count == 0)
            return 0;

        return _backgrounds.Keys.Min();
    }

    private void ResetToDefaultState()
    {
        int defaultIndex = GetDefaultBackgroundIndex();

        // Закрываем абсолютно все фоны.
        foreach (var background in _backgrounds.Values)
        {
            background.Close();
        }

        // Открываем только базовый.
        if (_backgrounds.TryGetValue(defaultIndex, out var defaultBackground))
            defaultBackground.Open();

        // Выбираем базовый.
        _currentBackgroundIndex = defaultIndex;

        // Сбрасываем сохранённый выбор.
        PlayerPrefs.SetInt(_selectedBackgroundKey, defaultIndex);
        PlayerPrefs.Save();
    }

#endregion

}

[Serializable]
public sealed class BackgroundSaveWrapper
{
    public List<BackgroundSaveEntry> Entries = new();
}

[Serializable]
public sealed class BackgroundSaveEntry
{
    public int Index;
    public bool IsOpened;
}

