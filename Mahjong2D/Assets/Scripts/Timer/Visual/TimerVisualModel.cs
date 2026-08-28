using System;

public class TimerVisualModel
{
    private readonly ITimerInfo _timerInfo;
    private readonly ITimerListener _timerListener;

    public TimerVisualModel(
        ITimerInfo timerInfo,
        ITimerListener timerListener)
    {
        _timerInfo = timerInfo;
        _timerListener = timerListener;
    }

    public void Initialize()
    {
        _timerListener.OnTimeChanged += HandleTimeChanged;
        _timerListener.OnStartTimer += HandleStartTimer;
        _timerListener.OnStopTimer += HandleStopTimer;

        SendSnapshot();
    }

    public void Dispose()
    {
        _timerListener.OnTimeChanged -= HandleTimeChanged;
        _timerListener.OnStartTimer -= HandleStartTimer;
        _timerListener.OnStopTimer -= HandleStopTimer;
    }

    private void HandleTimeChanged(int time)
    {
        SendSnapshot();
    }

    private void HandleStartTimer()
    {
        OnStartVisual?.Invoke();
    }

    private void HandleStopTimer()
    {
        OnStopVisual?.Invoke();
    }

    private void SendSnapshot()
    {
        TimerVisualData data = new TimerVisualData(
            _timerInfo.TotalTime,
            _timerInfo.CurrentTime,
            _timerInfo.ElapsedTime,
            _timerInfo.Direction);

        OnChangeVisual?.Invoke(data);
    }

    #region Output

    public event Action<TimerVisualData> OnChangeVisual;

    public event Action OnStartVisual;
    public event Action OnStopVisual;

    #endregion
}

public readonly struct TimerVisualData
{
    public int TotalTime { get; }
    public int CurrentTime { get; }
    public int ElapsedTime { get; }
    public TimerDirection Direction { get; }

    public TimerVisualData(
        int totalTime,
        int currentTime,
        int elapsedTime,
        TimerDirection direction)
    {
        TotalTime = totalTime;
        CurrentTime = currentTime;
        ElapsedTime = elapsedTime;
        Direction = direction;
    }
}
