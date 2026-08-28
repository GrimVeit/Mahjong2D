using System;

public class LevelVisualModel
{
    private readonly ILevelInfoProvider _levelInfoProvider;

    public LevelVisualModel(ILevelInfoProvider levelInfoProvider)
    {
        _levelInfoProvider = levelInfoProvider;
    }

    public void Initialize()
    {
        SetLevel(_levelInfoProvider.Level);
    }

    public void Dispose()
    {

    }

    private void SetLevel(int level)
    {
        OnSetLevel?.Invoke(level);
    }

    #region Output

    public event Action<int> OnSetLevel;

    #endregion
}
