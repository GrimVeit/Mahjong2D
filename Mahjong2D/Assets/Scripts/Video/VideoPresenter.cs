using System;
using Cysharp.Threading.Tasks;

public class VideoPresenter : IVideoProvider
{
    private readonly VideoModel _model;
    private readonly VideoView _view;

    public VideoPresenter(VideoModel model, VideoView view)
    {
        _model = model;
        _view = view;
    }

    public async UniTask Initialize()
    {
        ActivateEvents();

        await _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();
    }

    private void ActivateEvents()
    {
        _model.OnPlay += _view.Play;
        _model.OnPrepare += _view.Prepare;
    }

    private void DeactivateEvents()
    {
        _model.OnPlay -= _view.Play;
        _model.OnPrepare -= _view.Prepare;
    }

    #region Input

    public void Play(string id, Action onComplete = null)
    {
        _model.Play(id, onComplete);
    }

    public void Prepare(string id)
    {
        _model.Prepare(id);
    }

    #endregion
}

public interface IVideoProvider
{
    public void Prepare(string id);
    public void Play(string id, Action onComplete = null);
}
