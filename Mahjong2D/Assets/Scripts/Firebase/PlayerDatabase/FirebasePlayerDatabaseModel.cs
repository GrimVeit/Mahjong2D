using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public sealed class FirebasePlayerDatabaseModel : IDisposable
{
    private const string UsersNode = "Users";

    private readonly FirebaseDatabase _database;
    private readonly IAuthenticationInfoProvider _authenticationInfo;

    private readonly CancellationTokenSource _disposeCts = new();

    public FirebasePlayerDatabaseModel(
        FirebaseDatabase database,
        IAuthenticationInfoProvider authenticationInfo)
    {
        _database = database;
        _authenticationInfo = authenticationInfo;
    }

    public async UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data)
    {
        if (!_authenticationInfo.IsAuthorized)
            return DatabaseResult.NotAuthorized;

        try
        {
            DatabaseReference reference = GetPlayerReference();

            Dictionary<string, object> values = CreateValues(data);

            await reference
                .UpdateChildrenAsync(values)
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token);

            return DatabaseResult.Success;
        }
        catch (OperationCanceledException)
        {
            return DatabaseResult.UnknownError;
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
        if (!_authenticationInfo.IsAuthorized)
            return (DatabaseResult.NotAuthorized, null);

        try
        {
            DatabaseReference reference = GetPlayerReference();

            DataSnapshot snapshot = await reference
                .GetValueAsync()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token);

            if (!snapshot.Exists)
                return (DatabaseResult.NotFound, null);

            PlayerData data = ReadPlayerData(snapshot);

            return (DatabaseResult.Success, data);
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.UnknownError, null);
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

    public void Dispose()
    {
        _disposeCts.Cancel();
        _disposeCts.Dispose();
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
    Cancelled,
    Timeout,
    NotAuthorized,
    NotFound,
    UnknownError
}
