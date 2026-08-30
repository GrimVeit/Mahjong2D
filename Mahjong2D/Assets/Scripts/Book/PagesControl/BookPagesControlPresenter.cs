public sealed class BookPagesControlPresenter
{
    private readonly BookPagesControlModel _model;
    private readonly BookPagesControlView _view;

    public BookPagesControlPresenter(
        BookPagesControlModel model,
        BookPagesControlView view)
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

        _view.Dispose();
        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClickNext += _model.NextPage;
        _view.OnClickPrevious += _model.PreviousPage;

        _model.OnCanMoveLeft += _view.EnablePrevious;
        _model.OnCanMoveRight += _view.EnableNext;

        _model.OnCannotMoveLeft += _view.DisablePrevious;
        _model.OnCannotMoveRight += _view.DisableNext;
    }

    private void DeactivateEvents()
    {
        _view.OnClickNext -= _model.NextPage;
        _view.OnClickPrevious -= _model.PreviousPage;

        _model.OnCanMoveLeft -= _view.EnablePrevious;
        _model.OnCanMoveRight -= _view.EnableNext;

        _model.OnCannotMoveLeft -= _view.DisablePrevious;
        _model.OnCannotMoveRight -= _view.DisableNext;
    }
}
