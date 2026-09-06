using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public sealed class FirebaseDatabaseModel : IDisposable
{
    private const string DataNode = "Data";
    private const string UsersNode = "Users";
    private const int RequestTimeoutSeconds = 7;

    private readonly FirebaseDatabase _database;
    private readonly CancellationTokenSource _disposeCts = new();

    public FirebaseDatabaseModel(FirebaseDatabase database)
    {
        _database = database;
    }

    #region Players

    public async UniTask<(DatabaseResult Result, List<PlayerData> Players)> GetTopPlayers(int count)
    {
        OnGetTopPlayersStarted?.Invoke();

        DatabaseResult result;
        List<PlayerData> players = null;

        try
        {
            DataSnapshot snapshot = await _database.RootReference
                .Child(UsersNode)
                .OrderByChild("Level")
                .LimitToLast(count)
                .GetValueAsync()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            players = ReadPlayers(snapshot);

            players.Sort((a, b) => b.Level.CompareTo(a.Level));

            result = DatabaseResult.Success;
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (DatabaseException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetTopPlayers?.Invoke(result, players);

        return (result, players);
    }

    public async UniTask<(DatabaseResult Result, PlayerData Player)> GetPlayerByPlace(int place)
    {
        OnGetPlayerByPlaceStarted?.Invoke();

        DatabaseResult result;
        PlayerData player = null;

        if (place <= 0)
        {
            result = DatabaseResult.NotFound;

            OnGetPlayerByPlace?.Invoke(result, null);

            return (result, null);
        }

        try
        {
            DataSnapshot snapshot = await _database.RootReference
                .Child(UsersNode)
                .OrderByChild("Level")
                .LimitToLast(place)
                .GetValueAsync()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            List<PlayerData> players = ReadPlayers(snapshot);

            players.Sort((a, b) => b.Level.CompareTo(a.Level));

            int index = place - 1;

            if (index >= players.Count)
            {
                result = DatabaseResult.NotFound;
            }
            else
            {
                result = DatabaseResult.Success;
                player = players[index];
            }
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (DatabaseException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetPlayerByPlace?.Invoke(result, player);

        return (result, player);
    }

    private static List<PlayerData> ReadPlayers(DataSnapshot snapshot)
    {
        List<PlayerData> players = new();

        foreach (DataSnapshot child in snapshot.Children)
        {
            players.Add(ReadPlayerData(child));
        }

        return players;
    }

    private static PlayerData ReadPlayerData(DataSnapshot snapshot)
    {
        string nickname = snapshot.Child("Nickname").Value?.ToString();
        int level = Convert.ToInt32(snapshot.Child("Level").Value);

        return new PlayerData(nickname, level);
    }

    #endregion

    #region Others

    public async UniTask<(DatabaseResult Result, LinkGeoData Data)> GetLinkGeoData()
    {
        OnGetLinkConfigStarted?.Invoke();

        DatabaseResult result;
        LinkGeoData data = null;

        try
        {
            DataSnapshot snapshot = await _database.RootReference
                .Child(DataNode)
                .GetValueAsync()
                .AsUniTask()
                .AttachExternalCancellation(_disposeCts.Token)
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            string link = snapshot.Child("Link").Value?.ToString();

            List<string> geo = new();

            foreach (DataSnapshot child in snapshot.Child("Geo").Children)
            {
                string value = child.Value?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    geo.Add(value);
                }
            }

            data = new LinkGeoData(link, geo);
            result = DatabaseResult.Success;
        }
        catch (OperationCanceledException)
        {
            return (DatabaseResult.Cancelled, null);
        }
        catch (TimeoutException)
        {
            result = DatabaseResult.Timeout;
        }
        catch (DatabaseException exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            result = DatabaseResult.UnknownError;
        }

        OnGetLinkConfig?.Invoke(result, data);

        return (result, data);
    }

    #endregion

    public event Action OnGetTopPlayersStarted;
    public event Action<DatabaseResult, List<PlayerData>> OnGetTopPlayers;

    public event Action OnGetPlayerByPlaceStarted;
    public event Action<DatabaseResult, PlayerData> OnGetPlayerByPlace;

    public event Action OnGetLinkConfigStarted; 
    public event Action<DatabaseResult, LinkGeoData> OnGetLinkConfig;

    public void Dispose()
    {
        _disposeCts.Cancel();
        _disposeCts.Dispose();
    }
}

public sealed class LinkGeoData
{
    public string Link { get; }
    public List<string> Geo { get; }

    public LinkGeoData(string link, List<string> geo)
    {
        Link = link;
        Geo = geo;
    }
}
