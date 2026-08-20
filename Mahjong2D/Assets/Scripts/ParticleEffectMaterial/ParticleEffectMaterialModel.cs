using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectMaterialModel
{
    public void Activate()
    {
        OnActivate?.Invoke();
    }

    public void Deactivate()
    {
        OnDeactivate?.Invoke();
    }

    #region Output

    public event Action OnActivate;
    public event Action OnDeactivate;

    #endregion
}
