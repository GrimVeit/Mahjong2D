public class TimerVisualPresenter
{
    private readonly TimerVisualModel _model;
    private readonly ITimerVisualView _view;

    public TimerVisualPresenter(
        TimerVisualModel model,
        ITimerVisualView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnChangeVisual += _view.ChangeTime;
    }

    private void DeactivateEvents()
    {
        _model.OnChangeVisual -= _view.ChangeTime;
    }

    #region Input

    public void Show() => _view.Show();
    public void Hide() => _view.Hide();

    #endregion
}

public interface ITimerVisualProvider
{
    public void Show();
    public void Hide();
}

public interface ITimerVisualView
{
    void Initialize();
    void Dispose();

    void ChangeTime(TimerVisualData data);

    void Show();
    void Hide();
}
