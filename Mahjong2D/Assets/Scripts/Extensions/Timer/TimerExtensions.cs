using UnityEngine;

public static class TimerExtensions
{
    public static string ToTimeFormat(this int seconds)
    {
        seconds = Mathf.Max(0, seconds);

        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        return $"{minutes:00}:{remainingSeconds:00}";
    }
}
