using UnityEngine;

public sealed class CardsDesign
{
    public int Index { get; }
    public string Name { get; }

    public Sprite Sprite { get; }
    public int Price { get; }

    public bool IsOpened { get; private set; }

    public CardsDesign(int index, string name, Sprite sprite, int price, bool isOpened)
    {
        Index = index;
        Name = name;
        Sprite = sprite;
        Price = price;
        IsOpened = isOpened;
    }

    public void Open()
    {
        IsOpened = true;
    }

    public void Close()
    {
        IsOpened = false;
    }
}
