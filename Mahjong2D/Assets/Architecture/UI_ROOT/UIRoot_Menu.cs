using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot_Menu : UIRoot
{
    [SerializeField] private MainPanel_Menu mainPanel;
    [SerializeField] private SettingsPanel_Menu settingsPanel;
    [SerializeField] private WalletPanel_Menu walletPanel;
    [SerializeField] private LeaderboardPanel_Menu leaderboardPanel;

    public override void Initialize()
    {
        base.Initialize();

        mainPanel.Initialize();
        settingsPanel.Initialize();
        walletPanel.Initialize();
        leaderboardPanel.Initialize();

        ActivateEvents();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        mainPanel.Dispose();
        settingsPanel.Dispose();
        walletPanel.Dispose();
        leaderboardPanel.Dispose();

        base.Dispose();
    }

    private void ActivateEvents()
    {
        mainPanel.OnClickSettings += ClickSettings_Main;
        mainPanel.OnClickWallet += ClickWallet_Main;
        mainPanel.OnClickStore += ClickStore_Main;
        mainPanel.OnClickLeaderboard += ClickLeaderboard_Main;

        settingsPanel.OnClickExit += ClickExit_Settings;
        walletPanel.OnClickExit += ClickExit_Wallet;
        leaderboardPanel.OnClickExit += ClickExit_Leader;
    }

    private void DeactivateEvents()
    {
        mainPanel.OnClickSettings -= ClickSettings_Main;
        mainPanel.OnClickWallet -= ClickWallet_Main;
        mainPanel.OnClickStore -= ClickStore_Main;
        mainPanel.OnClickLeaderboard -= ClickLeaderboard_Main;

        settingsPanel.OnClickExit -= ClickExit_Settings;
        walletPanel.OnClickExit -= ClickExit_Wallet;
        leaderboardPanel.OnClickExit -= ClickExit_Leader;
    }

    #region Input

    public void ShowMainPanel()
    {
        ShowPanel(mainPanel);
    }

    public void HideMainPanel()
    {
        HidePanel(mainPanel);
    }



    public void ShowSettingsPanel()
    {
        ShowPanel(settingsPanel);
    }

    public void HideSettingsPanel()
    {
        HidePanel(settingsPanel);
    }




    public void ShowWalletPanel()
    {
        ShowPanel(walletPanel);
    }

    public void HideWalletPanel()
    {
        HidePanel(walletPanel);
    }




    public void ShowLeaderboardPanel()
    {
        ShowPanel(leaderboardPanel);
    }

    public void HideLeaderboardPanel()
    {
        HidePanel(leaderboardPanel);
    }

    #endregion

    #region Output



    #region Output_Main

    public event Action OnClickSettings_Main;
    public event Action OnClickWallet_Main;
    public event Action OnClickStore_Main;
    public event Action OnClickLeaderboard_Main;

    private void ClickSettings_Main()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickSettings_Main?.Invoke();
    }

    private void ClickWallet_Main()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickWallet_Main?.Invoke();
    }

    private void ClickStore_Main()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickStore_Main?.Invoke();
    }

    private void ClickLeaderboard_Main()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickLeaderboard_Main?.Invoke();
    }

    #endregion




    #region Output_Settings

    public event Action OnClickExit_Settings;

    private void ClickExit_Settings()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickExit_Settings?.Invoke();
    }

    #endregion



    #region Output_Wallet

    public event Action OnClickExit_Wallet;

    private void ClickExit_Wallet()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickExit_Wallet?.Invoke();
    }

    #endregion



    #region Output_Leader

    public event Action OnClickExit_Leader;

    private void ClickExit_Leader()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickExit_Leader?.Invoke();
    }

    #endregion

    #endregion
}
