using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel_Game : MoveFadePanel
{
    [SerializeField] private Button buttonMenu;
    [SerializeField] private Button buttonGame;

    public override void Initialize()
    {
        base.Initialize();

        buttonMenu.onClick.AddListener(ClickMenu);
        buttonGame.onClick.AddListener(ClickGame);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonMenu.onClick.RemoveListener(ClickMenu);
        buttonGame.onClick.RemoveListener(ClickGame);
    }

    #region Output

    public event Action OnClickMenu;
    public event Action OnClickGame;

    private void ClickMenu()
    {
        OnClickMenu?.Invoke();
    }

    private void ClickGame()
    {
        OnClickGame?.Invoke();
    }

    #endregion
}
