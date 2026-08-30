using UnityEngine;

public sealed class Background
{
    public int Index { get; }

    public Sprite Sprite { get; }
    public int Price { get; }

    public bool IsOpened { get; private set; }

    public Background(
        int index,
        Sprite sprite,
        int price,
        bool isOpened)
    {
        Index = index;
        Sprite = sprite;
        Price = price;
        IsOpened = isOpened;
    }

    public void Open()
    {
        IsOpened = true;
    }
}

