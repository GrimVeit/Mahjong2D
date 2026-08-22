using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPresenter : ISoundProvider
{
    private readonly SoundModel model;

    public SoundPresenter(SoundModel model)
    {
        this.model = model;
    }

    public void Initialize()
    {
        model.Initialize();
    }

    public void Dispose()
    {
        model.Dispose();
    }

    public void Play(string id)
    {
        model.Play(id);
    }

    public void PlayOneShot(string id)
    {
        model.PlayOneShot(id);
    }

    public ISound GetSound(string id)
    {
        return model.GetSound(id);
    }
}

public interface ISoundProvider
{
    void Play(string id);
    void PlayOneShot(string id);
    ISound GetSound(string id);
}
