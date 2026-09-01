using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "MahjongTilesGroup",
    menuName = "Game/Mahjong/Tiles Group"
)]
public sealed class MahjongTilesGroupSO : ScriptableObject
{
    [field: SerializeField]
    public int Index { get; private set; }

    [field: SerializeField]
    public List<Sprite> Sprites { get; private set; }
}
