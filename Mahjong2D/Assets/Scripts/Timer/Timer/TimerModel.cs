using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class TimerModel
{
    public bool IsActive => _isActive;

    /// <summary>
    /// Полная длительность таймера.
    /// </summary>
    public int TotalTime => _totalTime;

    /// <summary>
    /// Текущее значение таймера.
    ///
    /// Forward:
    /// 0 -> 1 -> 2 -> ...
    ///
    /// Backward:
    /// 20 -> 19 -> 18 -> ...
    /// </summary>
    public int CurrentTime => _currentTime; 

    /// <summary>
    /// Сколько секунд прошло с момента запуска таймера.
    /// </summary>
    public int ElapsedTime => _elapsedTime;

    public TimerDirection Direction => _direction;


    private bool _isActive;

    private int _totalTime;
    private int _currentTime;
    private int _elapsedTime;

    private TimerDirection _direction;

    private CancellationTokenSource _cancellationTokenSource;


    public event Action OnActivateTimer;
    public event Action OnDeactivateTimer;

    public event Action OnStartTimer;
    public event Action OnStopTimer;

    /// <summary>
    /// Изменилось текущее отображаемое значение таймера.
    /// </summary>
    public event Action<int> OnTimeChanged;

    /// <summary>
    /// Изменилось количество прошедших секунд.
    /// </summary>
    public event Action<int> OnElapsedTimeChanged;


    public void ActivateTimer(
        int seconds,
        TimerDirection direction)
    {
        if (seconds < 0)
            throw new ArgumentOutOfRangeException(nameof(seconds));

        StopTimerInternal();

        _isActive = true;

        _totalTime = seconds;
        _direction = direction;

        _elapsedTime = 0;

        _currentTime = direction == TimerDirection.Backward
            ? seconds
            : 0;

        OnActivateTimer?.Invoke();

        RunTimerAsync().Forget();
    }


    public void DeactivateTimer()
    {
        if (!_isActive)
            return;

        _isActive = false;

        StopTimerInternal();

        OnDeactivateTimer?.Invoke();
    }


    public void ResetTimer()
    {
        _isActive = false;

        StopTimerInternal();

        _totalTime = 0;
        _currentTime = 0;
        _elapsedTime = 0;

        OnTimeChanged?.Invoke(_currentTime);
        OnElapsedTimeChanged?.Invoke(_elapsedTime);
    }


    private async UniTaskVoid RunTimerAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            OnStartTimer?.Invoke();

            // Сразу отправляем начальное состояние.
            OnTimeChanged?.Invoke(_currentTime);
            OnElapsedTimeChanged?.Invoke(_elapsedTime);

            while (_isActive)
            {
                if (IsFinished())
                    break;

                await UniTask.Delay(
                    TimeSpan.FromSeconds(1),
                    cancellationToken: _cancellationTokenSource.Token);

                if (!_isActive)
                    break;

                UpdateTime();
            }

            if (!_isActive)
                return;

            _isActive = false;

            OnStopTimer?.Invoke();
            OnDeactivateTimer?.Invoke();
        }
        catch (OperationCanceledException)
        {
            // Нормальная ситуация при остановке/перезапуске таймера.
        }
        finally
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }


    private void UpdateTime()
    {
        _elapsedTime++;

        if (_direction == TimerDirection.Backward)
        {
            _currentTime--;
        }
        else
        {
            _currentTime++;
        }

        OnTimeChanged?.Invoke(_currentTime);
        OnElapsedTimeChanged?.Invoke(_elapsedTime);
    }


    private bool IsFinished()
    {
        return _elapsedTime >= _totalTime;
    }


    private void StopTimerInternal()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        _cancellationTokenSource = null;
    }
}


public enum TimerDirection
{
    Forward,
    Backward
}
