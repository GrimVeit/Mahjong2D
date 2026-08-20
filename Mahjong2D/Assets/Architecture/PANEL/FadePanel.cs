using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanel : Panel
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.25f;

    protected override void OnShow()
    {
        StartCoroutine(Fade(0f, 1f, null));
    }

    protected override void OnHide()
    {
        StartCoroutine(Fade(1f, 0f, CompleteHide));
    }

    private IEnumerator Fade(float from, float to, Action onComplete)
    {
        canvasGroup.alpha = from;

        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
        onComplete?.Invoke();
    }
}
