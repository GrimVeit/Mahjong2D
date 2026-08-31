using UnityEngine;

[CreateAssetMenu(fileName = "CardsDesignData", menuName = "Game/Cards Design Data")]
public sealed class CardsDesignDataSO : ScriptableObject
{
    public int Index => index;
    public string Name => nameBack;
    public Sprite Sprite => sprite;
    public int Price => price;

    [SerializeField] private int index;
    [SerializeField] private string nameBack;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;
}
