using System;

public sealed class StoreMovesPresenter : IMovesInfoProvider, IMovesEventsProvider, IMovesProvider
{
    private readonly StoreMovesModel _model;

    public StoreMovesPresenter(StoreMovesModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Info

    public int Moves => _model.Moves;

    #endregion

    #region Events

    public event Action<int> OnChangeMoves
    {
        add => _model.OnChangeMoves += value;
        remove => _model.OnChangeMoves -= value;
    }

    #endregion

    #region Input

    public void Increase()
    {
        _model.Increase();
    }

    #endregion
}

public interface IMovesInfoProvider
{
    int Moves { get; }
}

public interface IMovesEventsProvider
{
    event Action<int> OnChangeMoves;
}

public interface IMovesProvider
{
    void Increase();
}
