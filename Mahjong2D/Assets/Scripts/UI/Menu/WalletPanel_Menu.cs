using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WalletPanel_Menu : MoveFadePanel
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

    public event UnityAction OnClickExit;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    #endregion
}
