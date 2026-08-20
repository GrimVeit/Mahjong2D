using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    public event Action Shown;
    public event Action Hidden;

    public virtual void Initialize() { }
    public virtual void Dispose() { }

    public void Show()
    {
        if (IsOpen) return; IsOpen = true;

        gameObject.SetActive(true);

        OnShow();
        Shown?.Invoke();
    }

    public void Hide()
    {
        if (!IsOpen) return; IsOpen = false;

        OnHide();
    }

    protected void CompleteHide()
    {
        gameObject.SetActive(false);
        Hidden?.Invoke();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }
}
