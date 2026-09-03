using System.Text.RegularExpressions;
using System;

public sealed class ProfileNicknameInputModel
{
    private static readonly Regex MainRegex =
        new("^[a-zA-Z0-9./]*$");

    private static readonly Regex InvalidRegex =
        new(@"(\.{2,}|/{2,})");

    private readonly IPlayerProfileProvider _playerProfileProvider;

    private string _possibleNickname;

    public ProfileNicknameInputModel(
        IPlayerProfileProvider playerProfileProvider)
    {
        _playerProfileProvider = playerProfileProvider;
    }

    public void SetNickname(string value)
    {
        if (value == null)
        {
            SetNotValid("Nickname cannot be empty");
            return;
        }

        if (value.Length < 5)
        {
            SetNotValid(
                "Nickname must be at least 5 characters long"
            );

            return;
        }

        if (value.Length > 17)
        {
            SetNotValid(
                "Nickname must not exceed 17 characters"
            );

            return;
        }

        if (!MainRegex.IsMatch(value))
        {
            SetNotValid(
                "Nickname can only contain english letters, numbers, periods and slashes"
            );

            return;
        }

        if (InvalidRegex.IsMatch(value))
        {
            SetNotValid(
                "Nickname cannot contain consecutive periods and slashes"
            );

            return;
        }

        if (value.EndsWith("."))
        {
            SetNotValid(
                "Nickname cannot end with a period"
            );

            return;
        }

        _possibleNickname = value;

        OnSetValidate?.Invoke();
    }

    public void SubmitNickname()
    {
        if (string.IsNullOrEmpty(_possibleNickname))
            return;

        _playerProfileProvider.SetNickname(
            _possibleNickname
        );
    }

    private void SetNotValid(string error)
    {
        OnSetNotValidate?.Invoke(error);
    }

    public event Action<string> OnSetNotValidate;
    public event Action OnSetValidate;
}
