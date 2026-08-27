using System;
using UnityEngine;
using UnityEngine.UI;

public class MainHeaderPanel_Game : MoveFadePanel
{
    [SerializeField] private Button buttonMenu;

    public override void Initialize()
    {
        base.Initialize();

        buttonMenu.onClick.AddListener(ClickMenu);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonMenu.onClick.RemoveListener(ClickMenu);
    }

    #region Output

    public event Action OnClickMenu;

    private void ClickMenu()
    {
        OnClickMenu?.Invoke();
    }

    #endregion
}
