using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationDescriptionModel
{
    private readonly Dictionary<AuthenticationResult, string> _descriptions = new()
    {
        [AuthenticationResult.Success] = string.Empty,
        [AuthenticationResult.NicknameAlreadyUsed] = "This nickname is already registered.",
        [AuthenticationResult.InvalidNickname] = "Please enter a valid nickname.",
        [AuthenticationResult.NetworkError] = "No internet connection. Please try again.",
        [AuthenticationResult.UnknownError] = "Something went wrong. Please try again."
    };

    private readonly IAuthenticationEventsProvider _authenticationEventsProvider;

    public AuthenticationDescriptionModel(IAuthenticationEventsProvider authenticationEventsProvider)
    {
        _authenticationEventsProvider = authenticationEventsProvider;
    }

    public void Initialize()
    {
        _authenticationEventsProvider.OnAuthenticationResult += SetDescription;
    }

    public void Dispose()
    {
        _authenticationEventsProvider.OnAuthenticationResult -= SetDescription;
    }

    private void SetDescription(AuthenticationResult result)
    {
        if (!_descriptions.TryGetValue(result, out string description))
            return;

        OnSetDescription?.Invoke(description);
    }

    #region Output

    public event Action<string> OnSetDescription;

    #endregion
}
