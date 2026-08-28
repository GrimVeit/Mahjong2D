using TMPro;
using UnityEngine;

public class TimerVisualView_CurrentTime : View, ITimerVisualView
{
    [SerializeField] private TextMeshProUGUI text;

    public void Initialize() { }
    public void Dispose() { }
    public void Show() { }
    public void Hide() { }

    public void ChangeTime(TimerVisualData data)
    {
        text.text = data.CurrentTime.ToTimeFormat();
    }
}
