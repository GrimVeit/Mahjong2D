using System.Collections;
using UnityEngine;

public abstract class UIRoot : MonoBehaviour
{
    protected ISoundProvider _soundProvider { get; private set; }

    public virtual void Initialize() { }
    public virtual void Dispose() { }

    public void SetSoundProvider(ISoundProvider soundProvider)
    {
        _soundProvider = soundProvider;
    }

    protected void ShowPanel(Panel panel)
    {
        panel.Show();
    }

    protected void HidePanel(Panel panel)
    {
        panel.Hide();
    }
}
