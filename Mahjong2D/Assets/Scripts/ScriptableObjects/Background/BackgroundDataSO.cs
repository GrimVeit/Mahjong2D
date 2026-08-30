using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "Game/Background Data"
)]
public sealed class BackgroundDataSO : ScriptableObject
{
    public int Index => index;
    public Sprite Sprite => sprite;
    public int Price => price;

    [SerializeField] private int index;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;
}

