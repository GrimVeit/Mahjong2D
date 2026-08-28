using System;

public sealed class StoreMovesModel
{
    public int Moves { get; private set; }

    public event Action<int> OnChangeMoves;

    public void Initialize()
    {
        Moves = 0;

        OnChangeMoves?.Invoke(Moves);
    }

    public void Dispose()
    {
    }

    public void Increase()
    {
        Moves++;

        OnChangeMoves?.Invoke(Moves);
    }
}
