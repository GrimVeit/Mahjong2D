using System;
using UnityEngine;

public class UIRoot_Game : UIRoot
{
    [Header("Background")]
    [SerializeField] private BackgroundPanel_Game backgroundPanel;
    [SerializeField] private BackgroundPanel_Game backgroundResultPanel;
    [Header("Main")]
    [SerializeField] private MainPanel_Game mainPanel;
    [SerializeField] private MainHeaderPanel_Game mainHeaderPanel;
    [SerializeField] private MainFooterPanel_Game mainFooterPanel;
    [Header("Win")]
    [SerializeField] private WinVideoPanel_Game winVideoPanel;
    [SerializeField] private WinPanel_Game winPanel;
    [Header("Lose")]
    [SerializeField] private LoseVideoPanel_Game loseVideoPanel;
    [SerializeField] private LosePanel_Game losePanel;

    public override void Initialize()
    {
        base.Initialize();

        backgroundPanel.Initialize();
        backgroundResultPanel.Initialize();

        mainPanel.Initialize();
        mainHeaderPanel.Initialize();
        mainFooterPanel.Initialize();

        winVideoPanel.Initialize();
        winPanel.Initialize();

        loseVideoPanel.Initialize();
        losePanel.Initialize();

        ActivateEvents();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        backgroundPanel.Dispose();
        backgroundResultPanel.Dispose();

        mainPanel.Dispose();
        mainHeaderPanel.Dispose();
        mainFooterPanel.Dispose();

        winVideoPanel.Dispose();
        winPanel.Dispose();

        loseVideoPanel.Dispose();
        losePanel.Dispose();

        HideMainPanel();
        HideMainFooterPanel();
        HideMainHeaderPanel();
        HideLosePanel();
        HideWinPanel();

        base.Dispose();
    }

    private void ActivateEvents()
    {
        mainHeaderPanel.OnClickMenu += ClickMenu_MainHeader;

        winPanel.OnClickMenu += ClickMenu_Win;
        winPanel.OnClickGame += ClickGame_Win;

        losePanel.OnClickMenu += ClickMenu_Lose;
        losePanel.OnClickGame += ClickGame_Lose;
    }

    private void DeactivateEvents()
    {
        mainHeaderPanel.OnClickMenu -= ClickMenu_MainHeader;

        winPanel.OnClickMenu -= ClickMenu_Win;
        winPanel.OnClickGame -= ClickGame_Win;

        losePanel.OnClickMenu -= ClickMenu_Lose;
        losePanel.OnClickGame -= ClickGame_Lose;
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



    public void ShowBackgroundResultPanel()
    {
        ShowPanel(backgroundResultPanel);
    }

    public void HideBackgroundResultPanel()
    {
        HidePanel(backgroundResultPanel);
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




    public void ShowMainHeaderPanel()
    {
        ShowPanel(mainHeaderPanel);
    }

    public void HideMainHeaderPanel()
    {
        HidePanel(mainHeaderPanel);
    }



    public void ShowMainFooterPanel()
    {
        ShowPanel(mainFooterPanel);
    }

    public void HideMainFooterPanel()
    {
        HidePanel(mainFooterPanel);
    }

    #endregion

    #region WIn

    public void ShowWinVideoPanel()
    {
        ShowPanel(winVideoPanel);
    }

    public void HideWinVideoPanel()
    {
        HidePanel(winVideoPanel);
    }



    public void ShowWinPanel()
    {
        ShowPanel(winPanel);
    }

    public void HideWinPanel()
    {
        HidePanel(winPanel);
    }

    #endregion



    #region Lose

    public void ShowLoseVideoPanel()
    {
        ShowPanel(loseVideoPanel);
    }

    public void HideLoseVideoPanel()
    {
        HidePanel(loseVideoPanel);
    }



    public void ShowLosePanel()
    {
        ShowPanel(losePanel);
    }

    public void HideLosePanel()
    {
        HidePanel(losePanel);
    }

    #endregion

    #endregion

    #region Output

    #region MainHeader

    public event Action OnClickMenu_MainHeader;

    private void ClickMenu_MainHeader()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickMenu_MainHeader?.Invoke();
    }

    #endregion


    #region Win

    public event Action OnClickMenu_Win;
    public event Action OnClickGame_Win;

    private void ClickMenu_Win()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickMenu_Win?.Invoke();
    }

    private void ClickGame_Win()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickGame_Win?.Invoke();
    }

    #endregion


    #region Lose

    public event Action OnClickMenu_Lose;
    public event Action OnClickGame_Lose;

    private void ClickMenu_Lose()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickMenu_Lose?.Invoke();
    }

    private void ClickGame_Lose()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickGame_Lose?.Invoke();
    }

    #endregion

    #endregion
}
