using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePanel : Panel
{
    protected override void OnHide()
    {
        CompleteHide();
    }
}
