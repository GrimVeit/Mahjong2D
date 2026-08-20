using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectModel
{
    public event Action OnActivateEffect;
    public event Action OnDeactivateEffect;

    public Dictionary<string, ParticleEffect> particleEffects = new Dictionary<string, ParticleEffect>();

    public void Initialize(ParticleEffect[] effects)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            particleEffects.Add(effects[i].ID, effects[i]);
            effects[i].Initialize();
        }
    }

    public void Dispose()
    {
        foreach (var item in particleEffects.Values)
        {
            item.Dispose();
        }
    }

    public void Play(string ID)
    {
        if (particleEffects.ContainsKey(ID))
        {
            OnActivateEffect?.Invoke();
            particleEffects[ID].Play();
        }
    }

    public IParticleEffect GetParticleEffect(string id)
    {
        if (particleEffects.ContainsKey(id))
        {
            return particleEffects[id];
        }

        Debug.Log("Ёффект с идентификатором " + id + "не был найден");
        return null;
    }
}
