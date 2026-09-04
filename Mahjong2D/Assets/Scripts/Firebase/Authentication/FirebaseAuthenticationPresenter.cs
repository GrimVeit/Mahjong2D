using System;
using Cysharp.Threading.Tasks;

public sealed class FirebaseAuthenticationPresenter : IAuthenticationProvider, IAuthenticationInfoProvider, IAuthenticationEventsProvider
{
    private readonly FirebaseAuthenticationModel _model;

    public FirebaseAuthenticationPresenter(FirebaseAuthenticationModel model)
    {
        _model = model;
    }

    public bool IsAuthorized => _model.IsAuthorized;

    public string UserId => _model.UserId;

    public UniTask<AuthenticationResult> Register(string nickname)
    {
        return _model.Register(nickname);
    }

    public UniTask<AuthenticationResult> SignIn(string nickname)
    {
        return _model.SignIn(nickname);
    }

    public void SignOut()
    {
        _model.SignOut();
    }

    public UniTask<AuthenticationResult> DeleteAccount()
    {
        return _model.DeleteAccount();
    }

    #region Output

    public event Action<AuthenticationResult> OnAuthenticationResult
    {
        add => _model.OnAuthenticationResult += value;
        remove => _model.OnAuthenticationResult -= value;
    }

    #endregion
}

public interface IAuthenticationInfoProvider
{
    bool IsAuthorized { get; }
    string UserId { get; }
}

public interface IAuthenticationEventsProvider
{
    public event Action<AuthenticationResult> OnAuthenticationResult;
}

public interface IAuthenticationProvider
{
    UniTask<AuthenticationResult> Register(string nickname);
    UniTask<AuthenticationResult> SignIn(string nickname);
    void SignOut();
    UniTask<AuthenticationResult> DeleteAccount();
}
