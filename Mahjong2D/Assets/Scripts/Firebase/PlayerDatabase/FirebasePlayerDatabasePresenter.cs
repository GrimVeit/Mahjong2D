using Cysharp.Threading.Tasks;

public sealed class FirebasePlayerDatabasePresenter : IPlayerDatabaseProvider
{
    private readonly FirebasePlayerDatabaseModel _model;

    public FirebasePlayerDatabasePresenter(FirebasePlayerDatabaseModel model)
    {
        _model = model;
    }

    public void Cancel() => _model.Dispose();

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
