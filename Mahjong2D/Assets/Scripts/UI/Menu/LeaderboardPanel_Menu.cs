using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel_Menu : MoveFadePanel
{
    [Header("Buttons")]
    [SerializeField] private Button buttonExit;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickExit);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickExit);
    }

    #region Output

    public event Action OnClickExit;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    #endregion
}
