using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class WinVideoPanel_Game : MovePanel
{
    [SerializeField] private SkeletonGraphic skeletonGraphic;

    protected override void OnStartShow()
    {
        base.OnStartShow();

        skeletonGraphic.AnimationState.SetAnimation(0, "win", false);
    }

    protected override void OnStartHide()
    {
        base.OnStartHide();
    }
}
