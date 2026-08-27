public class StoreSessionModel
{
    public bool IsFirstLaunch { get; private set; } = true;

    public void CompleteFirstLaunch()
    {
        IsFirstLaunch = false;
    }
}
