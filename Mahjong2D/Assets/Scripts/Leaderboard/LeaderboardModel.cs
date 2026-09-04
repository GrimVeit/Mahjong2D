using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LeaderboardModel
{
    private readonly Dictionary<DatabaseResult, string> _descriptions = new()
    {
        [DatabaseResult.NotAuthorized] = "You are not authorized.",
        [DatabaseResult.NotFound] = "Leaderboard data was not found.",
        [DatabaseResult.Timeout] = "The connection is taking too long. Please check your internet connection and try again.",
        [DatabaseResult.UnknownError] = "Something went wrong. Please try again."
    };

    private const string LoadingDescription = "Hold on...";

    private readonly IDatabaseProvider _databaseProvider;

    public LeaderboardModel(IDatabaseProvider databaseProvider)
    {
        _databaseProvider = databaseProvider;
    }

    public void Initialize()
    {
        _databaseProvider.OnGetTopPlayersStarted += SetLoading;
        _databaseProvider.OnGetTopPlayers += SetData;
    }

    public void Dispose()
    {
        _databaseProvider.OnGetTopPlayersStarted -= SetLoading;
        _databaseProvider.OnGetTopPlayers -= SetData;
    }

    private void SetLoading()
    {
        OnGetTopPlayersStarted?.Invoke(LoadingDescription);
    }

    public void Refresh()
    {
        _databaseProvider.GetTopPlayers(10);
    }

    private void SetData(DatabaseResult result, List<PlayerData> playerDatas)
    {
        if (result != DatabaseResult.Success)
        {
            if (_descriptions.TryGetValue(result, out string description))
                OnErrorGetTopPlayers?.Invoke(description);

            return;
        }

        OnGetTopPlayers?.Invoke(playerDatas);
    }

    #region Output

    public event Action<string> OnGetTopPlayersStarted;
    public event Action<string> OnErrorGetTopPlayers;
    public event Action<List<PlayerData>> OnGetTopPlayers;

    #endregion
}
