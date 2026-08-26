using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroStartPanel : Move3PointPanel
{
    [SerializeField] private Button buttonStart;

    public override void Initialize()
    {
        base.Initialize();

        buttonStart.onClick.AddListener(ClickStart);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonStart.onClick.RemoveListener(ClickStart);
    }

    #region Output

    public event Action OnClickStart;

    private void ClickStart()
    {
        OnClickStart?.Invoke();
    }

    #endregion
}
