using TMPro;
using UnityEngine;

public class TimerVisualView_CurrentAndElapsedTime : View, ITimerVisualView
{
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI elapsedTimeText;

    public void Initialize() { }
    public void Dispose() { }

    public void ChangeTime(TimerVisualData data)
    {
        currentTimeText.text = data.CurrentTime.ToTimeFormat();
        elapsedTimeText.text = data.ElapsedTime.ToTimeFormat();
    }

    public void Show() { }
    public void Hide() { }
}
