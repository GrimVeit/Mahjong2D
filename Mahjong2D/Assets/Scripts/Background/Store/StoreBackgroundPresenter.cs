using System;
using System.Collections.Generic;

public sealed class StoreBackgroundPresenter : IBackgroundProvider, IBackgroundInfoProvider, IBackgroundListener
{
    private readonly StoreBackgroundModel _model;

    public StoreBackgroundPresenter(StoreBackgroundModel model)
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

    #region Provider

    public void OpenBackground(int index)
    {
        _model.OpenBackground(index);
    }

    public void SelectBackground(int index)
    {
        _model.SelectBackground(index);
    }

    #endregion

    #region Info

    public Background GetBackground(int index)
    {
        return _model.GetBackground(index);
    }

    public IReadOnlyList<Background> GetBackgrounds()
    {
        return _model.GetBackgrounds();
    }

    public Background GetCurrentBackground()
    {
        return _model.GetCurrentBackground();
    }

    public int CurrentBackgroundIndex =>
        _model.GetCurrentBackgroundIndex();

    public bool IsBackgroundOpened(int index)
    {
        return _model.IsBackgroundOpened(index);
    }

    public bool IsBackgroundSelected(int index)
    {
        return _model.IsBackgroundSelected(index);
    }

    #endregion

    #region Listener

    public event Action<Background, bool> OnOpenBackground
    {
        add => _model.OnOpenBackground += value;
        remove => _model.OnOpenBackground -= value;
    }

    public event Action<Background> OnSelectBackground
    {
        add => _model.OnSelectBackground += value;
        remove => _model.OnSelectBackground -= value;
    }

    #endregion
}

public interface IBackgroundProvider
{
    void OpenBackground(int index);
    void SelectBackground(int index);
}

public interface IBackgroundInfoProvider
{
    Background GetBackground(int index);

    IReadOnlyList<Background> GetBackgrounds();

    Background GetCurrentBackground();

    int CurrentBackgroundIndex { get; }

    bool IsBackgroundOpened(int index);

    bool IsBackgroundSelected(int index);
}

public interface IBackgroundListener
{
    event Action<Background, bool> OnOpenBackground;
    event Action<Background> OnSelectBackground;
}


