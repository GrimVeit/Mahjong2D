using System;

public sealed class BookPagesControlModel
{
    private readonly IBookPageProvider _bookPageProvider;
    private readonly IBookPageInfoProvider _bookPageInfoProvider;
    private readonly IBookPageEventsProvider _bookPageEventsProvider;

    public BookPagesControlModel(
        IBookPageProvider bookPageProvider,
        IBookPageInfoProvider bookPageInfoProvider,
        IBookPageEventsProvider bookPageEventsProvider)
    {
        _bookPageProvider = bookPageProvider;
        _bookPageInfoProvider = bookPageInfoProvider;
        _bookPageEventsProvider = bookPageEventsProvider;
    }

    public void Initialize()
    {
        _bookPageEventsProvider.OnCanMoveLeft += HandleCanMoveLeft;
        _bookPageEventsProvider.OnCanMoveRight += HandleCanMoveRight;

        _bookPageEventsProvider.OnCannotMoveLeft += HandleCannotMoveLeft;
        _bookPageEventsProvider.OnCannotMoveRight += HandleCannotMoveRight;

        UpdateNavigation();
    }

    public void Dispose()
    {
        _bookPageEventsProvider.OnCanMoveLeft -= HandleCanMoveLeft;
        _bookPageEventsProvider.OnCanMoveRight -= HandleCanMoveRight;

        _bookPageEventsProvider.OnCannotMoveLeft -= HandleCannotMoveLeft;
        _bookPageEventsProvider.OnCannotMoveRight -= HandleCannotMoveRight;
    }

    public void NextPage()
    {
        _bookPageProvider.OpenNextPage();
    }

    public void PreviousPage()
    {
        _bookPageProvider.OpenPreviousPage();
    }

    public void OpenPage(int index)
    {
        _bookPageProvider.OpenPage(index);
    }

    private void UpdateNavigation()
    {
        if (_bookPageInfoProvider.CanMoveLeft)
            OnCanMoveLeft?.Invoke();
        else
            OnCannotMoveLeft?.Invoke();

        if (_bookPageInfoProvider.CanMoveRight)
            OnCanMoveRight?.Invoke();
        else
            OnCannotMoveRight?.Invoke();
    }

    private void HandleCanMoveLeft()
    {
        OnCanMoveLeft?.Invoke();
    }

    private void HandleCanMoveRight()
    {
        OnCanMoveRight?.Invoke();
    }

    private void HandleCannotMoveLeft()
    {
        OnCannotMoveLeft?.Invoke();
    }

    private void HandleCannotMoveRight()
    {
        OnCannotMoveRight?.Invoke();
    }

    #region Output

    public event Action OnCanMoveLeft;
    public event Action OnCanMoveRight;

    public event Action OnCannotMoveLeft;
    public event Action OnCannotMoveRight;

    #endregion
}
