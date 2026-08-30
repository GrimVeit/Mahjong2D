using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovesVisualView : View
{
    [SerializeField] private List<TextMeshProUGUI> textMovesCount;

    public void SetMoves(int count)
    {
        Debug.Log(count);

        textMovesCount.ForEach(tmc => tmc.text = count.ToString());
    }
}
