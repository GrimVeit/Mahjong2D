using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot_Menu : UIRoot
{
    [Header("Background")]
    [SerializeField] private BackgroundPanel_Menu backgroundPanel;
    [Header("Intro")]
    [SerializeField] private IntroVideoPanel_Menu introVideoPanel;
    [SerializeField] private IntroStartPanel introStartPanel;

    [Header("Main")]
    [SerializeField] private MainPanel_Menu mainPanel;
    [SerializeField] private SettingsPanel_Menu settingsPanel;
    [SerializeField] private WalletPanel_Menu walletPanel;
    [SerializeField] private LeaderboardPanel_Menu leaderboardPanel;

    [Header("Store")]
    [SerializeField] private StoreChooseTypePanel_Menu storeChooseTypePanel;
    [SerializeField] private StoreBackgroundPanel_Menu storeBackgroundPanel;

    public override void Initialize()
    {
        base.Initialize();

        backgroundPanel.Initialize();

        introVideoPanel.Initialize();
        introStartPanel.Initialize();

        mainPanel.Initialize();
        settingsPanel.Initialize();
        walletPanel.Initialize();
        leaderboardPanel.Initialize();

        storeChooseTypePanel.Initialize();
        storeBackgroundPanel.Initialize();

        ActivateEvents();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        backgroundPanel.Dispose();

        introVideoPanel.Dispose();
        introStartPanel.Dispose();

        mainPanel.Dispose();
        settingsPanel.Dispose();
        walletPanel.Dispose();
        leaderboardPanel.Dispose();

        storeChooseTypePanel.Dispose();
        storeBackgroundPanel.Dispose();

        HideMainPanel();

        base.Dispose();
    }

    private void ActivateEvents()
    {
        introStartPanel.OnClickStart += ClickStart_IntroStart;

        mainPanel.OnClickSettings += ClickSettings_Main;
        mainPanel.OnClickWallet += ClickWallet_Main;
        mainPanel.OnClickStore += ClickStore_Main;
        mainPanel.OnClickLeaderboard += ClickLeaderboard_Main;
        mainPanel.OnClickPlay += ClickPlay_Main;

        settingsPanel.OnClickExit += ClickExit_Settings;
        walletPanel.OnClickExit += ClickExit_Wallet;
        leaderboardPanel.OnClickExit += ClickExit_Leader;

        //store
        storeChooseTypePanel.OnClickExit += ClickExit_StoreChooseType;
        storeChooseTypePanel.OnClickBackgrounds += ClickBackgrounds_StoreChooseType;
        storeChooseTypePanel.OnClickCards += ClickCards_StoreChooseType;

        storeBackgroundPanel.OnClickExit += ClickExit_StoreBackground;
    }

    private void DeactivateEvents()
    {
        introStartPanel.OnClickStart -= ClickStart_IntroStart;

        mainPanel.OnClickSettings -= ClickSettings_Main;
        mainPanel.OnClickWallet -= ClickWallet_Main;
        mainPanel.OnClickStore -= ClickStore_Main;
        mainPanel.OnClickLeaderboard -= ClickLeaderboard_Main;
        mainPanel.OnClickPlay -= ClickPlay_Main;

        settingsPanel.OnClickExit -= ClickExit_Settings;
        walletPanel.OnClickExit -= ClickExit_Wallet;
        leaderboardPanel.OnClickExit -= ClickExit_Leader;

        storeChooseTypePanel.OnClickExit -= ClickExit_StoreChooseType;
        storeChooseTypePanel.OnClickBackgrounds -= ClickBackgrounds_StoreChooseType;
        storeChooseTypePanel.OnClickCards -= ClickCards_StoreChooseType;

        storeBackgroundPanel.OnClickExit -= ClickExit_StoreBackground;
    }

    #region Input

    #region Background

    public void ShowBackgroundPanel()
    {
        ShowPanel(backgroundPanel);
    }

    public void HideBackgroundPanel()
    {
        HidePanel(backgroundPanel);
    }

    #endregion

    #region INTRO

    public void ShowIntroVideoPanel()
    {
        ShowPanel(introVideoPanel);
    }

    public void HideIntroVideoPanel()
    {
        HidePanel(introVideoPanel);
    }



    public void ShowIntroStartPanel()
    {
        ShowPanel(introStartPanel);
    }

    public void HideIntroStartPanel()
    {
        HidePanel(introStartPanel);
    }

    #endregion

    #region MAIN

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

    #region STORE

    public void ShowStoreChooseTypePanel()
    {
        ShowPanel(storeChooseTypePanel);
    }

    public void HideStoreChooseTypePanel()
    {
        HidePanel(storeChooseTypePanel);
    }


    public void ShowStoreBackgroundPanel()
    {
        ShowPanel(storeBackgroundPanel);
    }

    public void HideStoreBackgroundPanel()
    {
        HidePanel(storeBackgroundPanel);
    }

    #endregion

    #endregion

    #region Output

    #region Output_Intro_Start

    public event Action OnClickStart_IntroStart;

    private void ClickStart_IntroStart()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickStart_IntroStart?.Invoke();
    }

    #endregion


    #region Output_Main

    public event Action OnClickPlay_Main;
    public event Action OnClickSettings_Main;
    public event Action OnClickWallet_Main;
    public event Action OnClickStore_Main;
    public event Action OnClickLeaderboard_Main;

    private void ClickPlay_Main()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickPlay_Main?.Invoke();
    }

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

    #region Output_StoreChooseType

    public event Action OnClickExit_StoreChooseType;
    public event Action OnClickBackgrounds_StoreChooseType;
    public event Action OnClickCards_StoreChooseType;

    private void ClickExit_StoreChooseType()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickExit_StoreChooseType?.Invoke();
    }

    private void ClickBackgrounds_StoreChooseType()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickBackgrounds_StoreChooseType?.Invoke();
    }

    private void ClickCards_StoreChooseType()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickCards_StoreChooseType?.Invoke();
    }

    #endregion

    #region StoreBackground

    public event Action OnClickExit_StoreBackground;

    private void ClickExit_StoreBackground()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickExit_StoreBackground?.Invoke();
    }

    #endregion

    #endregion
}
