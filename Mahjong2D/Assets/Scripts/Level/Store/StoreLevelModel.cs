using System;
using UnityEngine;

public class StoreLevelModel
{
    public int Level { get; private set; }

    private readonly string _levelKey;

    public StoreLevelModel(string levelKey)
    {
        _levelKey = levelKey;
    }

    public void Initialize()
    {
        Level = PlayerPrefs.GetInt(_levelKey, 0);

        OnChangeLevel?.Invoke(Level);
    }

    public void Dispose()
    {
        PlayerPrefs.SetInt(_levelKey, Level);
        PlayerPrefs.Save();
    }

    public void IncreaseLevel()
    {
        Level += 1;

        OnChangeLevel?.Invoke(Level);
    }

    public event Action<int> OnChangeLevel;
}
