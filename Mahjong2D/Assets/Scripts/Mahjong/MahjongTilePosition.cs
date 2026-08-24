public readonly struct MahjongTilePosition
{
    public int Layer { get; }

    public int GridX { get; }

    public int GridY { get; }


    public MahjongTilePosition(
        int layer,
        int gridX,
        int gridY)
    {
        Layer = layer;

        GridX = gridX;

        GridY = gridY;
    }
}
