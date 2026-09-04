using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class FirebaseDatabasePresenter : IDatabaseProvider
{
    private readonly FirebaseDatabaseModel _model;

    public FirebaseDatabasePresenter(FirebaseDatabaseModel model)
    {
        _model = model;
    }

    public void Cancel() => _model.Dispose();

    #region Output

    public event Action OnGetTopPlayersStarted
    {
        add => _model.OnGetTopPlayersStarted += value;
        remove => _model.OnGetTopPlayersStarted -= value;
    }

    public event Action<DatabaseResult, List<PlayerData>> OnGetTopPlayers
    {
        add => _model.OnGetTopPlayers += value;
        remove => _model.OnGetTopPlayers -= value;
    }

    public event Action OnGetPlayerByPlaceStarted
    {
        add => _model.OnGetPlayerByPlaceStarted += value;
        remove => _model.OnGetPlayerByPlaceStarted -= value;
    }

    public event Action<DatabaseResult, PlayerData> OnGetPlayerByPlace
    {
        add => _model.OnGetPlayerByPlace += value;
        remove => _model.OnGetPlayerByPlace -= value;
    }

    #endregion

    #region Input

    public UniTask<(DatabaseResult Result, List<PlayerData> Players)> GetTopPlayers(int count)
    {
        return _model.GetTopPlayers(count);
    }

    public UniTask<(DatabaseResult Result, PlayerData Player)> GetPlayerByPlace(int place)
    {
        return _model.GetPlayerByPlace(place);
    }

    #endregion
}

public interface IDatabaseProvider
{
    UniTask<(DatabaseResult Result, List<PlayerData> Players)> GetTopPlayers(int count);

    UniTask<(DatabaseResult Result, PlayerData Player)> GetPlayerByPlace(int place);

    event Action OnGetTopPlayersStarted;
    event Action<DatabaseResult, List<PlayerData>> OnGetTopPlayers;

    event Action OnGetPlayerByPlaceStarted;
    event Action<DatabaseResult, PlayerData> OnGetPlayerByPlace;
}
