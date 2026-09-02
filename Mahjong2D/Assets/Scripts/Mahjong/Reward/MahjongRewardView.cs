using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MahjongRewardView : View
{
    [SerializeField] private List<TextMeshProUGUI> textRewards;

    public void SetReward(int reward)
    {
        textRewards.ForEach(data => data.text = reward.ToString());
    }
}
