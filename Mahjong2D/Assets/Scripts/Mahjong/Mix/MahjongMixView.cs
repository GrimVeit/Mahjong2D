using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MahjongMixView : View
{
    [SerializeField] private Button buttonMix;

    public void Initialize()
    {
        buttonMix.onClick.AddListener(ClickMix);
    }

    public void Dispose()
    {
        buttonMix.onClick.RemoveListener(ClickMix);
    }

    #region Ouput

    public event Action OnClickMix;

    private void ClickMix()
    {
        OnClickMix?.Invoke();
    }

    #endregion
}
