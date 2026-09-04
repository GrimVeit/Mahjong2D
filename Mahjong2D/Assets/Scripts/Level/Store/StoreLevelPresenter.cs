using System;

public class StoreLevelPresenter : ILevelInfoProvider, ILevelEventsProvider, ILevelProvider
{
    private readonly StoreLevelModel _model;

    public StoreLevelPresenter(StoreLevelModel model)
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

    public int Level => _model.Level;

    #endregion

    #region Events

    public event Action<int> OnChangeLevel
    {
        add => _model.OnChangeLevel += value;
        remove => _model.OnChangeLevel -= value;
    }

    #endregion

    #region Input

    public void IncreaseLevel()
    {
        _model.IncreaseLevel();
    }

    public void SetZero()
    {
        _model.SetLevel(0);
    }

    #endregion
}

public interface ILevelInfoProvider
{
    int Level { get; }
}

public interface ILevelEventsProvider
{
    event Action<int> OnChangeLevel;
}

public interface ILevelProvider
{
    void IncreaseLevel();
    void SetZero();
}
