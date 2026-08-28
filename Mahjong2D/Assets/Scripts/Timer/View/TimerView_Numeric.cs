using TMPro;
using UnityEngine;

public class TimerView_Numeric : View
    //, ITimerView
    , IIdentify
{
    [SerializeField] private string id;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private UIEffect effectText;

    public string GetID() => id;

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void ChangeTime(int sec)
    {
        textCount.text = sec.ToString();
    }

    public void ActivateTimer()
    {
        textCount.gameObject.SetActive(true);
    }

    public void DeactivateTimer()
    {
        textCount.gameObject.SetActive(false);
    }
}
