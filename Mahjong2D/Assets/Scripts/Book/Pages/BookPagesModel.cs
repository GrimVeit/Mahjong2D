using System;
using UnityEngine;

public sealed class BookPagesModel
{
    public int CurrentPageIndex { get; private set; }

    public int PageCount => _pageCount;

    public bool CanMoveLeft => CurrentPageIndex > 0;
    public bool CanMoveRight => CurrentPageIndex < _pageCount - 1;

    private readonly int _pageCount;

    public BookPagesModel(int pageCount)
    {
        _pageCount = pageCount;
    }

    public void Initialize()
    {
        CurrentPageIndex = 0;

        OnChangePage?.Invoke(CurrentPageIndex);
        UpdateNavigation();
    }

    public void Dispose()
    {
    }

    public void OpenPage(int index)
    {
        if (_pageCount <= 0)
            return;

        index = Mathf.Clamp(index, 0, _pageCount - 1);

        if (CurrentPageIndex == index)
            return;

        OnRequestOpenPage?.Invoke(index);
    }

    public void OpenNextPage()
    {
        if (!CanMoveRight)
            return;

        OpenPage(CurrentPageIndex + 1);
    }

    public void OpenPreviousPage()
    {
        if (!CanMoveLeft)
            return;

        OpenPage(CurrentPageIndex - 1);
    }

    public void CompletePageChange(int index)
    {
        if (index < 0 || index >= _pageCount)
            return;

        CurrentPageIndex = index;

        OnChangePage?.Invoke(CurrentPageIndex);
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        if (CanMoveLeft)
            OnCanMoveLeft?.Invoke();
        else
            OnCannotMoveLeft?.Invoke();

        if (CanMoveRight)
            OnCanMoveRight?.Invoke();
        else
            OnCannotMoveRight?.Invoke();
    }

    #region Output

    public event Action<int> OnRequestOpenPage;

    public event Action<int> OnChangePage;

    public event Action OnCanMoveLeft;
    public event Action OnCanMoveRight;

    public event Action OnCannotMoveLeft;
    public event Action OnCannotMoveRight;

    #endregion
}
