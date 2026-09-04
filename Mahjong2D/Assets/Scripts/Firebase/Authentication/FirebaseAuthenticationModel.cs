using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using System;
using UnityEngine;

public sealed class FirebaseAuthenticationModel
{
    private const string Password = "123456";
    private const int RequestTimeoutSeconds = 10;

    private readonly FirebaseAuth _auth;

    public FirebaseAuthenticationModel(FirebaseAuth auth)
    {
        _auth = auth;
    }

    public bool IsAuthorized => _auth.CurrentUser != null;
    public string UserId => _auth.CurrentUser?.UserId;

    public event Action<AuthenticationResult> OnAuthenticationResult;

    public async UniTask<AuthenticationResult> Register(string nickname)
    {
        string email = $"{nickname}@gmail.com";

        AuthenticationResult result;

        try
        {
            await _auth
                .CreateUserWithEmailAndPasswordAsync(email, Password)
                .AsUniTask()
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            result = AuthenticationResult.Success;
        }
        catch (TimeoutException)
        {
            result = AuthenticationResult.Timeout;
        }
        catch (FirebaseException exception)
        {
            result = MapException(exception);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            result = AuthenticationResult.UnknownError;
        }

        OnAuthenticationResult?.Invoke(result);

        return result;
    }

    public async UniTask<AuthenticationResult> SignIn(string nickname)
    {
        string email = $"{nickname}@gmail.com";

        AuthenticationResult result;

        try
        {
            await _auth
                .SignInWithEmailAndPasswordAsync(email, Password)
                .AsUniTask()
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            result = AuthenticationResult.Success;
        }
        catch (TimeoutException)
        {
            result = AuthenticationResult.Timeout;
        }
        catch (FirebaseException exception)
        {
            result = MapException(exception);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            result = AuthenticationResult.UnknownError;
        }

        OnAuthenticationResult?.Invoke(result);

        return result;
    }

    public void SignOut()
    {
        _auth.SignOut();
    }

    public async UniTask<AuthenticationResult> DeleteAccount()
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user == null)
        {
            AuthenticationResult result = AuthenticationResult.NotAuthorized;

            OnAuthenticationResult?.Invoke(result);

            return result;
        }

        try
        {
            Credential credential = EmailAuthProvider.GetCredential(
                user.Email,
                Password);

            await user
                .ReauthenticateAsync(credential)
                .AsUniTask()
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            await user
                .DeleteAsync()
                .AsUniTask()
                .Timeout(TimeSpan.FromSeconds(RequestTimeoutSeconds));

            AuthenticationResult result = AuthenticationResult.Success;

            OnAuthenticationResult?.Invoke(result);

            return result;
        }
        catch (TimeoutException)
        {
            AuthenticationResult result = AuthenticationResult.Timeout;

            OnAuthenticationResult?.Invoke(result);

            return result;
        }
        catch (FirebaseException exception)
        {
            AuthenticationResult result = MapException(exception);

            OnAuthenticationResult?.Invoke(result);

            return result;
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);

            AuthenticationResult result = AuthenticationResult.UnknownError;

            OnAuthenticationResult?.Invoke(result);

            return result;
        }
    }

    private static AuthenticationResult MapException(FirebaseException exception)
    {
        AuthError error = (AuthError)exception.ErrorCode;

        switch (error)
        {
            case AuthError.EmailAlreadyInUse:
                return AuthenticationResult.NicknameAlreadyUsed;

            case AuthError.NetworkRequestFailed:
                return AuthenticationResult.NetworkError;

            case AuthError.InvalidEmail:
                return AuthenticationResult.InvalidNickname;

            default:
                Debug.LogException(exception);
                return AuthenticationResult.UnknownError;
        }
    }
}

public enum AuthenticationResult
{
    Success,
    NicknameAlreadyUsed,
    InvalidNickname,
    NotAuthorized,
    NetworkError,
    Timeout,
    UnknownError
}
