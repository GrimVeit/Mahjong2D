using System;

public class TimerPresenter : ITimerProvider, ITimerListener, ITimerInfo
{
    private readonly TimerModel _model;


    public TimerPresenter(TimerModel model)
    {
        _model = model;
    }


    public void Initialize()
    {

    }


    public void Dispose()
    {
        _model.DeactivateTimer();
    }

    #region Input

    public void ActivateTimer(
        int seconds,
        TimerDirection direction)
    {
        _model.ActivateTimer(seconds, direction);
    }


    public void DeactivateTimer()
    {
        _model.DeactivateTimer();
    }


    public void ResetTimer()
    {
        _model.ResetTimer();
    }

    #endregion


    #region Info

    public bool IsActive => _model.IsActive;

    public int TotalTime => _model.TotalTime;

    public int CurrentTime => _model.CurrentTime;

    public int ElapsedTime => _model.ElapsedTime;

    public TimerDirection Direction => _model.Direction;

    #endregion


    #region Output

    public event Action<int> OnTimeChanged
    {
        add => _model.OnTimeChanged += value;
        remove => _model.OnTimeChanged -= value;
    }


    public event Action<int> OnElapsedTimeChanged
    {
        add => _model.OnElapsedTimeChanged += value;
        remove => _model.OnElapsedTimeChanged -= value;
    }

    public event Action OnStartTimer
    {
        add => _model.OnStartTimer += value;
        remove => _model.OnStartTimer -= value;
    }

    public event Action OnStopTimer
    {
        add => _model.OnStopTimer += value;
        remove => _model.OnStopTimer -= value;
    }

    #endregion
}

public interface ITimerProvider
{
    void ActivateTimer(int seconds, TimerDirection direction);
    void DeactivateTimer();
    void ResetTimer();
}

public interface ITimerInfo
{
    bool IsActive { get; }

    int TotalTime { get; }

    int CurrentTime { get; }

    int ElapsedTime { get; }

    TimerDirection Direction { get; }
}

public interface ITimerListener
{
    event Action<int> OnTimeChanged;
    event Action<int> OnElapsedTimeChanged;

    event Action OnStartTimer;
    event Action OnStopTimer;
}
