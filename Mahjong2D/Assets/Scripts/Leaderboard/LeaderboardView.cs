using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class LeaderboardView : View
{
    [SerializeField] private List<LeaderboardUser> users = new();

    [Header("Hold On")]
    [SerializeField] private TextMeshProUGUI textHoldOn;
    [SerializeField] private UIEffect uIEffect_HoldOn;

    [Header("Error")]
    [SerializeField] private TextMeshProUGUI textError;
    [SerializeField] private UIEffect uIEffect_Error;

    [Header("Retry")]
    [SerializeField] private UIEffect uIEffect_Retry;
    [SerializeField] private Button buttonRetry;

    public void Initialize()
    {
        buttonRetry.onClick.AddListener(Retry);

        HideUsers();

        buttonRetry.interactable = false;

        uIEffect_HoldOn.Initialize();
        uIEffect_Error.Initialize();
        uIEffect_Retry.Initialize();
    }

    public void Dispose()
    {
        buttonRetry.onClick.RemoveListener(Retry);

        uIEffect_HoldOn.Dispose();
        uIEffect_Error.Dispose();
        uIEffect_Retry.Dispose();
    }

    public void SetData(List<PlayerData> playerDatas)
    {
        HideUsers();

        if (uIEffect_Error.IsActive)
            uIEffect_Error.PlayHide();

        if (uIEffect_HoldOn.IsActive)
            uIEffect_HoldOn.PlayHide();

        if (uIEffect_Retry.IsActive)
            uIEffect_Retry.PlayHide();

        buttonRetry.interactable = false;

        for (int i = 0; i < playerDatas.Count; i++)
        {
            users[i].SetData(playerDatas[i]);
            users[i].Show();
        }
    }

    public void SetHoldOn(string message)
    {
        textHoldOn.text = message;

        buttonRetry.interactable = false;

        if (uIEffect_Error.IsActive)
            uIEffect_Error.PlayHide();

        if (!uIEffect_HoldOn.IsActive)
            uIEffect_HoldOn.PlayShow();

        if (uIEffect_Retry.IsActive)
            uIEffect_Retry.PlayHide();
    }

    public void SetError(string message)
    {
        textError.text = message;

        buttonRetry.interactable = true;

        if (uIEffect_HoldOn.IsActive)
            uIEffect_HoldOn.PlayHide();

        if (!uIEffect_Error.IsActive)
            uIEffect_Error.PlayShow();

        if (!uIEffect_Retry.IsActive)
            uIEffect_Retry.PlayShow();
    }

    private void HideUsers()
    {
        foreach (LeaderboardUser user in users)
            user.ResetClear();
    }

    #region Output

    public event Action OnRetry;

    private void Retry()
    {
        OnRetry?.Invoke();
    }

    #endregion
}
