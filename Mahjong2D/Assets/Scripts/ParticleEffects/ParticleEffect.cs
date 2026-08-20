using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParticleEffect : IParticleEffect
{
    public string ID;

    //public event Action OnStartParticleEffect;
    //public event Action OnEndParticleEffect;

    private bool isPlaying;

    public List<Particle> particles = new List<Particle>();

    private IEnumerator coroutineParticles;

    public void Initialize()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Initialize();
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Dispose();
        }
    }

    public void Play()
    {
        if (coroutineParticles != null)
            Coroutines.Instance.StartCoroutine(coroutineParticles);

        coroutineParticles = PlayParticles_Coroutine();
        Coroutines.Instance.StartCoroutine(coroutineParticles);
    }

    private IEnumerator PlayParticles_Coroutine()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
            yield return new WaitForSeconds(particles[i].TimeToInterval);
        }
    }
}

public interface IParticleEffect 
{ 
    void Play();
}


[Serializable]
public class Particle
{
    public float TimeToInterval => timeToInterval;

    [SerializeField] private float timeToInterval;
    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private bool isPlayAwake;

    public void Initialize()
    {
        float randomSize = UnityEngine.Random.Range(minSize, maxSize);

        particleSystem.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        if (isPlayAwake)
            Play();
    }

    public void Dispose()
    {

    }

    public void Play()
    {
        if(particleSystem != null)
           particleSystem.Play();
    }
}
