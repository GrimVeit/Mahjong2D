public class MahjongTileData
{
    public int Id { get; }

    public int Layer { get; private set; }
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public bool IsActive { get; private set; }
    public bool IsRemoved { get; private set; }

    public MahjongTileData(
        int id,
        int layer,
        int gridX,
        int gridY)
    {
        Id = id;

        Layer = layer;
        GridX = gridX;
        GridY = gridY;
    }

    public void SetPosition(
        int layer,
        int gridX,
        int gridY)
    {
        Layer = layer;
        GridX = gridX;
        GridY = gridY;
    }

    public void SetActive(bool value)
    {
        IsActive = value;
    }

    public void Remove()
    {
        IsRemoved = true;
        IsActive = false;
    }
}