using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreChooseTypePanel_Menu : MoveFadePanel
{
    [SerializeField] private Button buttonExit;
    [SerializeField] private Button buttonBackgrounds;
    [SerializeField] private Button buttonCards;

    public override void Initialize()
    {
        base.Initialize();

        buttonExit.onClick.AddListener(ClickExit);
        buttonBackgrounds.onClick.AddListener(ClickBackgrounds);
        buttonCards.onClick.AddListener(ClickCards);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonExit.onClick.RemoveListener(ClickExit);
        buttonBackgrounds.onClick.RemoveListener(ClickBackgrounds);
        buttonCards.onClick.RemoveListener(ClickCards);
    }

    #region Output

    public event Action OnClickExit;
    public event Action OnClickBackgrounds;
    public event Action OnClickCards;

    private void ClickExit()
    {
        OnClickExit?.Invoke();
    }

    private void ClickBackgrounds()
    {
        OnClickBackgrounds?.Invoke();
    }

    private void ClickCards()
    {
        OnClickCards?.Invoke();
    }

    #endregion
}
