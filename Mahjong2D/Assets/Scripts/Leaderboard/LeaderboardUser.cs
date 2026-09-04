using TMPro;
using UnityEngine;

public class LeaderboardUser : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private UIEffect uIEffect;

    public void SetData(PlayerData data)
    {
        textNickname.text = data.Nickname;
        textLevel.text = data.Level.ToString();
    }

    public void ResetClear()
    {
        uIEffect.ResetEffect();

        transform.localScale = Vector3.zero;
    }

    public void Show()
    {
        uIEffect.PlayShow();
    }

    public void Hide()
    {
        uIEffect.PlayHide();
    }
}
