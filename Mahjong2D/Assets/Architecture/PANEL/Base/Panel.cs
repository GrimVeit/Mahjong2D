using System;
using UnityEngine;

[RequireComponent(typeof(Panel_UIEffectGroup))]
public abstract class Panel : MonoBehaviour
{
    [SerializeField] private Panel_UIEffectGroup effectGroup;
    public bool IsOpen { get; private set; }

    public event Action Shown;
    public event Action Hidden;

    public virtual void Initialize() { }
    public virtual void Dispose() { }

    public void Show()
    {
        if (IsOpen)
            return;

        IsOpen = true;

        gameObject.SetActive(true);

        effectGroup.PlayShow();

        OnStartShow();
    }

    public void Hide()
    {
        if (!IsOpen)
            return;

        IsOpen = false;

        effectGroup.PlayHide();

        OnStartHide();
    }

    protected void CompleteShow()
    {
        Shown?.Invoke();
    }

    protected void CompleteHide()
    {
        gameObject.SetActive(false);

        Hidden?.Invoke();
    }

    protected virtual void OnStartShow()
    {
        CompleteShow();
    }

    protected virtual void OnStartHide()
    {
        CompleteHide();
    }
}
