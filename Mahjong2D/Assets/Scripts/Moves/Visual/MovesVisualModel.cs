using System;

public class MovesVisualModel
{
    private readonly IMovesEventsProvider _movesEventsProvider;

    public MovesVisualModel(IMovesEventsProvider movesEventsProvider)
    {
        _movesEventsProvider = movesEventsProvider;
    }

    public void Initialize()
    {
        _movesEventsProvider.OnChangeMoves += SetMoves;

        SetMoves(0);
    }

    public void Dispose()
    {
        _movesEventsProvider.OnChangeMoves -= SetMoves;
    }

    private void SetMoves(int moves)
    {
        OnSetMove?.Invoke(moves);
    }

    #region Ouput

    public event Action<int> OnSetMove;

    #endregion
}
