using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public sealed class FirebaseDatabaseModel
{
    private const string UsersNode = "Users";

    private readonly FirebaseDatabase _database;
    private readonly IAuthenticationInfoProvider _authenticationInfo;

    public FirebaseDatabaseModel(
        FirebaseDatabase database,
        IAuthenticationInfoProvider authenticationInfo)
    {
        _database = database;
        _authenticationInfo = authenticationInfo;
    }

    public async UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data)
    {
        if (!_authenticationInfo.IsAuthorized) return DatabaseResult.NotAuthorized;

        try
        {
            DatabaseReference reference = GetPlayerReference();

            Dictionary<string, object> values = CreateValues(data);

            await reference.UpdateChildrenAsync(values);

            return DatabaseResult.Success;
        }
        catch (DatabaseException exception)
        {
            Debug.LogException(exception);
            return DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return DatabaseResult.UnknownError;
        }
    }

    public async UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer()
    {
        if (!_authenticationInfo.IsAuthorized) return (DatabaseResult.NotAuthorized, null);

        try
        {
            DatabaseReference reference = GetPlayerReference();

            DataSnapshot snapshot = await reference.GetValueAsync();

            if (!snapshot.Exists)
                return (DatabaseResult.NotFound, null);

            PlayerData data = ReadPlayerData(snapshot);

            return (DatabaseResult.Success, data);
        }
        catch (DatabaseException exception)
        {
            Debug.LogException(exception);
            return (DatabaseResult.UnknownError, null);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return (DatabaseResult.UnknownError, null);
        }
    }

    private DatabaseReference GetPlayerReference()
    {
        return _database.RootReference
            .Child(UsersNode)
            .Child(_authenticationInfo.UserId);
    }

    private static Dictionary<string, object> CreateValues(PlayerData data)
    {
        return new Dictionary<string, object>
        {
            ["Nickname"] = data.Nickname,
            ["Level"] = data.Level
        };
    }

    private static PlayerData ReadPlayerData(DataSnapshot snapshot)
    {
        string nickname = snapshot.Child("Nickname").Value?.ToString();
        int level = Convert.ToInt32(snapshot.Child("Level").Value);

        return new PlayerData(nickname, level);
    }
}

public sealed class PlayerData
{
    public string Nickname { get; }
    public int Level { get; }

    public PlayerData(string nickname, int level)
    {
        Nickname = nickname;
        Level = level;
    }
}

public enum DatabaseResult
{
    Success,
    NotAuthorized,
    NotFound,
    UnknownError
}
