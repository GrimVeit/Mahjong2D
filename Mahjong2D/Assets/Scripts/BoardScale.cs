using UnityEngine;

public class BoardScale : MonoBehaviour
{
    private const float ReferenceWidth = 1080f;
    private const float ReferenceHeight = 1920f;

    public void UpdateScale()
    {
        float scaleX = Screen.width / ReferenceWidth;
        float scaleY = Screen.height / ReferenceHeight;

        float scale = Mathf.Min(scaleX, scaleY);

        transform.localScale = Vector3.one * scale;

        Debug.Log(Vector3.one * scale);
    }
}
