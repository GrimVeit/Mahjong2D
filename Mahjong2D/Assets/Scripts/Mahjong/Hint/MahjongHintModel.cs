using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongHintModel
{
    private readonly IMahjongProvider _mahjongProvider;

    public MahjongHintModel(IMahjongProvider mahjongProvider)
    {
        _mahjongProvider = mahjongProvider;
    }

    public void Hint()
    {
        _mahjongProvider.Hint();
    }
}
