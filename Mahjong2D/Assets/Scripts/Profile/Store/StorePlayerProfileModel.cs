using System;
using UnityEngine;

public sealed class StorePlayerProfileModel : IDisposable
{
    private readonly string _nicknameKey;

    private readonly string _defaultNickname;

    public PlayerProfile Profile { get; private set; }

    public event Action<PlayerProfile> OnProfileChanged;

    public StorePlayerProfileModel(string nicknameKey, string defaultNickname = "ABCD123")
    {
        _nicknameKey = nicknameKey;

        _defaultNickname = defaultNickname;
    }

    public void Initialize()
    {
        string nickname = PlayerPrefs.GetString(_nicknameKey, _defaultNickname);

        Profile = new PlayerProfile(nickname);
    }

    public void SetNickname(string nickname)
    {
        Profile = new PlayerProfile(nickname);

        OnProfileChanged?.Invoke(Profile);
    }

    public void Dispose()
    {
        PlayerPrefs.SetString(_nicknameKey, Profile.Nickname);
        PlayerPrefs.Save();
    }
}

public sealed class PlayerProfile
{
    public string Nickname { get; }

    public PlayerProfile(string nickname)
    {
        Nickname = nickname;
    }
}
