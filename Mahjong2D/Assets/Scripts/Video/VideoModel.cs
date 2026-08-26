using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoModel
{
    public void Play(string id, Action onComplete)
    {
        OnPlay?.Invoke(id, onComplete);
    }

    public void Prepare(string id)
    {
        OnPrepare?.Invoke(id);
    }

    #region Output

    public event Action<string> OnPrepare;
    public event Action<string, Action> OnPlay;

    #endregion
}
