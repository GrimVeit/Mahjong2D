using UnityEngine;

public sealed class BookPage : MonoBehaviour
{
    public int Index => _index;

    public RectTransform RectTransform { get; private set; }

    [SerializeField] private int _index;

    public void Initialize()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void ShowInstant()
    {
        gameObject.SetActive(true);

        RectTransform.anchoredPosition = Vector2.zero;
        RectTransform.localScale = Vector3.one;
        RectTransform.localRotation = Quaternion.identity;
    }

    public void HideInstant()
    {
        gameObject.SetActive(false);
    }
}
