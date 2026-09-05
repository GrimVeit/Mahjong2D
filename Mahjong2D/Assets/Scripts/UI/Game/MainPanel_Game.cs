using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel_Game : FadePanel
{
    [SerializeField] private BoardScale boardScale;

    public override void Initialize()
    {
        base.Initialize();

        boardScale.UpdateScale();
    }

    protected override void OnStartShow()
    {
        boardScale.UpdateScale();

        base.OnStartShow();
    }
}
