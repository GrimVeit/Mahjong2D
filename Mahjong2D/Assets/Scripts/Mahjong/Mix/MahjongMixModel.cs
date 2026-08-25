using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MahjongMixModel
{
    private readonly IMahjongProvider _mahjongProvider;

    public MahjongMixModel(IMahjongProvider mahjongProvider)
    {
        _mahjongProvider = mahjongProvider;
    }

    public void Mix()
    {
        _mahjongProvider.Mix();
    }
}
