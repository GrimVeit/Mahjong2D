using Cysharp.Threading.Tasks;
using System.Threading;

public interface IState
{
    public void Enter();
    public void Exit();
}

public abstract class AsyncState : IState
{
    private CancellationTokenSource _cts;

    public void Enter()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        _cts = new CancellationTokenSource();

        EnterAsync(_cts.Token).Forget();
    }

    public virtual void Exit()
    {
        _cts?.Cancel();
    }

    protected abstract UniTask EnterAsync(CancellationToken token);
}
