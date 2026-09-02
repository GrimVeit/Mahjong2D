using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MahjongScoreView : View
{
    [SerializeField] private List<TextMeshProUGUI> textScores;

    public void SetScore(int score)
    {
        textScores.ForEach(data => data.text = score.ToString());
    }
}
