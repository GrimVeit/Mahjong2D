using System;
using UnityEngine;

public abstract class UIEffect : MonoBehaviour
{
    public bool IsActive => isActive;

    protected bool isActive;

    public virtual void Initialize() { }
    public virtual void Dispose() { }

    public virtual void PlayShow(Action onComplete = null) { }
    public virtual void PlayHide(Action onComplete = null) { }

    public virtual void ResetEffect() { }
}
