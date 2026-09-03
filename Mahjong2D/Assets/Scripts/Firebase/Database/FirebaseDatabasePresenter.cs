using Cysharp.Threading.Tasks;

public sealed class FirebaseDatabasePresenter : IPlayerDatabaseProvider
{
    private readonly FirebaseDatabaseModel _model;

    public FirebaseDatabasePresenter(FirebaseDatabaseModel model)
    {
        _model = model;
    }

    public UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer()
    {
        return _model.LoadPlayer();
    }

    public UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data)
    {
        return _model.CreateOrUpdatePlayer(data);
    }
}

public interface IPlayerDatabaseProvider
{
    UniTask<(DatabaseResult Result, PlayerData Data)> LoadPlayer();
    UniTask<DatabaseResult> CreateOrUpdatePlayer(PlayerData data);
}
