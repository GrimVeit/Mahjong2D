using System;

public sealed class BookPagesPresenter :
    IBookPageProvider,
    IBookPageInfoProvider,
    IBookPageEventsProvider
{
    private readonly BookPagesModel _model;
    private readonly BookPagesView _view;

    public BookPagesPresenter(
        BookPagesModel model,
        BookPagesView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        _model.OnRequestOpenPage += _view.OpenPage;
        _view.OnPageOpened += _model.CompletePageChange;

        _view.Initialize();
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.OnRequestOpenPage -= _view.OpenPage;
        _view.OnPageOpened -= _model.CompletePageChange;

        _view.Dispose();
        _model.Dispose();
    }

    #region Input

    public void OpenPage(int index)
    {
        _model.OpenPage(index);
    }

    public void OpenNextPage()
    {
        _model.OpenNextPage();
    }

    public void OpenPreviousPage()
    {
        _model.OpenPreviousPage();
    }

    #endregion

    #region Info

    public int CurrentPageIndex =>
        _model.CurrentPageIndex;

    public int PageCount =>
        _model.PageCount;

    public bool CanMoveLeft =>
        _model.CanMoveLeft;

    public bool CanMoveRight =>
        _model.CanMoveRight;

    #endregion

    #region Output

    public event Action<int> OnChangePage
    {
        add => _model.OnChangePage += value;
        remove => _model.OnChangePage -= value;
    }

    public event Action OnCanMoveLeft
    {
        add => _model.OnCanMoveLeft += value;
        remove => _model.OnCanMoveLeft -= value;
    }

    public event Action OnCanMoveRight
    {
        add => _model.OnCanMoveRight += value;
        remove => _model.OnCanMoveRight -= value;
    }

    public event Action OnCannotMoveLeft
    {
        add => _model.OnCannotMoveLeft += value;
        remove => _model.OnCannotMoveLeft -= value;
    }

    public event Action OnCannotMoveRight
    {
        add => _model.OnCannotMoveRight += value;
        remove => _model.OnCannotMoveRight -= value;
    }

    #endregion
}

public interface IBookPageProvider
{
    void OpenPage(int index);

    void OpenNextPage();
    void OpenPreviousPage();
}

public interface IBookPageInfoProvider
{
    int CurrentPageIndex { get; }
    int PageCount { get; }

    bool CanMoveLeft { get; }
    bool CanMoveRight { get; }
}

public interface IBookPageEventsProvider
{
    event Action<int> OnChangePage;

    event Action OnCanMoveLeft;
    event Action OnCanMoveRight;

    event Action OnCannotMoveLeft;
    event Action OnCannotMoveRight;
}
