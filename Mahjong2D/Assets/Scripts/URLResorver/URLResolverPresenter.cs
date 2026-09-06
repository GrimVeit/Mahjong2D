using System;
using Cysharp.Threading.Tasks;

public sealed class URLResolverPresenter : IURLResolverProvider, IURLResolverListen
{
    private readonly UrlResolverModel _model;

    public URLResolverPresenter(UrlResolverModel model)
    {
        _model = model;
    }

    public void Dispose()
    {
        _model.Dispose();
    }

    #region Input

    public UniTask<(UrlResolverResult Result, string Url)> ResolveFromTitle(string sourceUrl)
    {
        return _model.ResolveFromTitle(sourceUrl);
    }

    #endregion

    #region Output

    public event Action<UrlResolverResult, string> OnURLResolve
    {
        add => _model.OnURLResolve += value;
        remove => _model.OnURLResolve -= value;
    }

    public event Action OnStartURLResolve
    {
        add => _model.OnStartURLResolve += value;
        remove => _model.OnStartURLResolve -= value;
    }

    #endregion

}

public interface IURLResolverProvider
{
    UniTask<(UrlResolverResult Result, string Url)> ResolveFromTitle(string sourceUrl);
}

public interface IURLResolverListen
{
    public event Action OnStartURLResolve;
    public event Action<UrlResolverResult, string> OnURLResolve;
}

