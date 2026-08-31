using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public sealed class StoreCardsDesignModel
{
    public event Action<CardsDesign> OnOpenCardDessign;
    public event Action<CardsDesign> OnSelectCardDesign;

    private readonly Dictionary<int, CardsDesign> _cardsDesigns;

    private readonly string _filePath;
    private readonly string _selectedCardsDesignKey;
    private readonly string _xorKey;

    private int _currentCardDesignIndex;

    public CardsDesign CurrentCardDesign => _cardsDesigns.TryGetValue(_currentCardDesignIndex, out var background) ? background : null;
    
    public StoreCardsDesignModel(IEnumerable<CardsDesignDataSO> data, string saveFileName = "CardsDesigns.json", string selectedDesignKey = PlayerPrefsKeys.CARDS_DESIGNS, string xorKey = "cx4xnm69ut4k7v1pzjbyejilnu7c3p5h8dudvqth")
    {
        _cardsDesigns = new Dictionary<int, CardsDesign>();

        _filePath = Path.Combine(Application.persistentDataPath, saveFileName);

        _selectedCardsDesignKey = selectedDesignKey;
        _xorKey = xorKey;

        foreach (var designData in data)
        {
            if (designData == null)
                continue;

            if (_cardsDesigns.ContainsKey(designData.Index))
            {
                Debug.LogError(
                    $"Duplicate background index: {designData.Index}"
                );

                continue;
            }

            _cardsDesigns.Add(
                designData.Index,
                new CardsDesign(
                    designData.Index,
                    designData.Name,
                    designData.Sprite,
                    designData.Price,
                    false
                )
            );
        }
    }

    #region INIT

    public void Initialize()
    {
        Load();

        int defaultIndex = GetDefaultDesignIndex();

        if (_cardsDesigns.TryGetValue(defaultIndex, out var defaultDesign))
            defaultDesign.Open();

        _currentCardDesignIndex = PlayerPrefs.GetInt(_selectedCardsDesignKey, defaultIndex);

        // Если выбранный фон отсутствует — используем дефолтный.
        if (!_cardsDesigns.ContainsKey(_currentCardDesignIndex))
            _currentCardDesignIndex = defaultIndex;

        foreach (var item in _cardsDesigns)
        {
            Debug.Log($"CARDS DESIGN INDEX - {item.Key}, IS OPEN - {item.Value.IsOpened}");
        }
    }

    public void Dispose()
    {
        Save();

        PlayerPrefs.SetInt(_selectedCardsDesignKey, _currentCardDesignIndex);
        PlayerPrefs.Save();
    }

    #endregion

    #region INPUT

    public void OpenCardDesign(int index)
    {
        if (!_cardsDesigns.TryGetValue(index, out var design))
        {
            Debug.LogError($"Card design not found: {index}");
            return;
        }

        if (design.IsOpened)
            return;

        design.Open();

        OnOpenCardDessign?.Invoke(design);
    }

    public void SelectCardDesign(int index)
    {
        if (!_cardsDesigns.TryGetValue(index, out var design))
        {
            Debug.LogError(
                $"Card design not found: {index}"
            );

            return;
        }

        if (!design.IsOpened)
            return;

        if (_currentCardDesignIndex == index)
            return;

        _currentCardDesignIndex = index;

        OnSelectCardDesign?.Invoke(design);
    }

    #endregion

    #region INFO

    public CardsDesign GetCardDesign(int index)
    {
        return _cardsDesigns.TryGetValue(index, out var background) ? background : null;
    }

    public IReadOnlyList<CardsDesign> GetCardDesigns()
    {
        return _cardsDesigns.Values
            .OrderBy(background => background.Index)
            .ToList();
    }

    public CardsDesign GetCurrentCardDesign()
    {
        return CurrentCardDesign;
    }

    public int GetCurrentCardDesignIndex()
    {
        return _currentCardDesignIndex;
    }

    public bool IsCardDesignOpened(int index)
    {
        return _cardsDesigns.TryGetValue(index, out var background) && background.IsOpened;
    }

    public bool IsCardDesignSelected(int index)
    {
        return _currentCardDesignIndex == index;
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

            var wrapper = JsonUtility.FromJson<CardsDesignSaveWrapper>(json);

            if (wrapper?.Entries == null)
                throw new Exception("Invalid save data.");

            foreach (var entry in wrapper.Entries)
            {
                if (entry == null)
                    continue;

                if (!_cardsDesigns.TryGetValue(entry.Index, out var background))
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
        var wrapper = new CardsDesignSaveWrapper();

        foreach (var background in _cardsDesigns.Values)
        {
            wrapper.Entries.Add(
                new CardsDesignSaveEntry
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

    private int GetDefaultDesignIndex()
    {
        if (_cardsDesigns.Count == 0)
            return 0;

        return _cardsDesigns.Keys.Min();
    }

    private void ResetToDefaultState()
    {
        int defaultIndex = GetDefaultDesignIndex();

        // Закрываем абсолютно все фоны.
        foreach (var design in _cardsDesigns.Values)
        {
            design.Close();
        }

        // Открываем только базовый.
        if (_cardsDesigns.TryGetValue(defaultIndex, out var defaultDesign))
            defaultDesign.Open();

        // Выбираем базовый.
        _currentCardDesignIndex = defaultIndex;

        // Сбрасываем сохранённый выбор.
        PlayerPrefs.SetInt(_selectedCardsDesignKey, defaultIndex);
        PlayerPrefs.Save();
    }

    #endregion

}

[Serializable]
public sealed class CardsDesignSaveWrapper
{
    public List<CardsDesignSaveEntry> Entries = new();
}

[Serializable]
public sealed class CardsDesignSaveEntry
{
    public int Index;
    public bool IsOpened;
}
