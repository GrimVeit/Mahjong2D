using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundVisualView : View
{
    [SerializeField] private Image imageBackground;

    public void SetBackground(Background background)
    {
        imageBackground.sprite = background.Sprite;
    }
}
