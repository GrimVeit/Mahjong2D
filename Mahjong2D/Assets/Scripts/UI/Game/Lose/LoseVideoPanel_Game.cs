using Spine.Unity;
using UnityEngine;

public class LoseVideoPanel_Game : MovePanel
{
    [SerializeField] private SkeletonGraphic skeletonGraphic;

    protected override void OnStartShow()
    {
        base.OnStartShow();

        skeletonGraphic.AnimationState.SetAnimation(0, "fail", false);
    }

    protected override void OnStartHide()
    {
        base.OnStartHide();
    }
}
