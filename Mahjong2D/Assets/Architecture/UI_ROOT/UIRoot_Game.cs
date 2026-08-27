using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot_Game : UIRoot
{
    [Header("Background")]
    [SerializeField] private BackgroundPanel_Game backgroundPanel;
    [Header("Main")]
    [SerializeField] private MainPanel_Game mainPanel;
    [SerializeField] private MainHeaderPanel_Game mainHeaderPanel;
    [SerializeField] private MainFooterPanel_Game mainFooterPanel;

    public override void Initialize()
    {
        base.Initialize();

        backgroundPanel.Initialize();

        mainPanel.Initialize();
        mainHeaderPanel.Initialize();
        mainFooterPanel.Initialize();

        ActivateEvents();
    }

    public override void Dispose()
    {
        DeactivateEvents();

        backgroundPanel.Dispose();

        mainPanel.Dispose();
        mainHeaderPanel.Dispose();
        mainFooterPanel.Dispose();

        HideMainPanel();

        base.Dispose();
    }

    private void ActivateEvents()
    {
        mainHeaderPanel.OnClickMenu += ClickMenu_MainHeader;
    }

    private void DeactivateEvents()
    {
        mainHeaderPanel.OnClickMenu -= ClickMenu_MainHeader;
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

    #endregion

    #region Output

    #region Output_MainHeader

    public event Action OnClickMenu_MainHeader;

    private void ClickMenu_MainHeader()
    {
        _soundProvider?.PlayOneShot("Click");

        OnClickMenu_MainHeader?.Invoke();
    }

    #endregion

    #endregion
}
