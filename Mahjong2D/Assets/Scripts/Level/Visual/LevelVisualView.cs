using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelVisualView : View
{
    [SerializeField] private List<TextMeshProUGUI> textsLevel;
    [SerializeField] private List<TextMeshProUGUI> textsLevel_Level;
    [SerializeField] private List<TextMeshProUGUI> textsLevelSecond_Arrow;

    public void SetLevel(int level)
    {
        textsLevel.ForEach(data => data.text = (level + 1).ToString());

        textsLevel_Level.ForEach(data => data.text = $"Level {level + 1}");


        textsLevelSecond_Arrow.ForEach(data => data.text = $"Level {level + 2} →");
    }
}
