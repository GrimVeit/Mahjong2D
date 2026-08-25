using System;
using UnityEngine;
using UnityEngine.UI;

public class MahjongHintView : View
{
    [SerializeField] private Button buttonHint;

    public void Initialize()
    {
        buttonHint.onClick.AddListener(ClickHint);
    }

    public void Dispose()
    {
        buttonHint.onClick.RemoveListener(ClickHint);
    }

    #region Ouput

    public event Action OnClickHint;

    private void ClickHint()
    {
        OnClickHint?.Invoke();
    }

    #endregion
}
