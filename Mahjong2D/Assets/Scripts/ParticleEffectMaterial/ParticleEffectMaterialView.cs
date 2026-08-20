using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParticleEffectMaterialView : View
{
    [SerializeField] private List<ParticleEffectMaterial> particleEffectMaterials = new List<ParticleEffectMaterial>();
    [SerializeField] private float time;

    public void Activate()
    {
        foreach (var effect in particleEffectMaterials)
        {
            Color c = effect.Material.GetColor(effect.Key);

            DOTween.To(() => c.a, x =>
            {
                c.a = x;
                effect.Material.SetColor(effect.Key, c);
            }, 1f, time);
        }
    }

    public void Deactivate()
    {
        foreach (var effect in particleEffectMaterials)
        {
            Color c = effect.Material.GetColor(effect.Key);

            DOTween.To(() => c.a, x =>
            {
                c.a = x;
                effect.Material.SetColor(effect.Key, c);
            }, 0f, time);
        }
    }
}

[System.Serializable]
public class ParticleEffectMaterial
{
    [SerializeField] private string key;
    [SerializeField] private Material material;

    public string Key => key;
    public Material Material => material;
}
