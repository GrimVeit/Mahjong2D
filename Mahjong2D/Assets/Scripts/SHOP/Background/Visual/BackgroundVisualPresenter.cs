using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundVisualPresenter
{
    private readonly IBackgroundInfoProvider _backgroundInfoProvider;
    private readonly BackgroundVisualView _view;

    public BackgroundVisualPresenter(IBackgroundInfoProvider backgroundInfoProvider, BackgroundVisualView view)
    {
        _backgroundInfoProvider = backgroundInfoProvider;
        _view = view;
    }

    public void Initialize()
    {
        _view.SetBackground(_backgroundInfoProvider.GetCurrentBackground());
    }

    public void Dispose()
    {

    }
}
