using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public sealed class BookPagesView : View
{
    [SerializeField] private List<BookPage> pages = new();
    [SerializeField] private RectTransform viewport;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private float startScale = 0.94f;
    [SerializeField] private float startRotation = 8f;

    private BookPage currentPage;

    private bool isAnimating;

    private float Width => viewport.rect.width;

    public void Initialize()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].Initialize();
            pages[i].HideInstant();
        }

        pages = pages
            .OrderBy(page => page.Index)
            .ToList();

        if (pages.Count == 0)
            return;

        currentPage = pages[0];

        currentPage.ShowInstant();

        ResetPage(currentPage);

        isAnimating = false;
    }

    public void Dispose()
    {
        isAnimating = false;
    }

    public void OpenPage(int targetIndex)
    {
        if (isAnimating)
            return;

        if (targetIndex < 0 || targetIndex >= pages.Count)
            return;

        if (currentPage.Index == targetIndex)
            return;

        MoveToPageAsync(targetIndex).Forget();
    }

    private async UniTask MoveToPageAsync(int targetIndex)
    {
        isAnimating = true;

        int currentIndex = currentPage.Index;

        if (targetIndex > currentIndex)
            await MoveNextAsync(targetIndex);
        else
            await MovePreviousAsync(targetIndex);

        isAnimating = false;

        OnPageOpened?.Invoke(targetIndex);
    }

    private async UniTask MoveNextAsync(int targetIndex)
    {
        BookPage oldPage = currentPage;
        BookPage newPage = pages[targetIndex];

        ResetPage(oldPage);

        newPage.gameObject.SetActive(true);

        newPage.RectTransform.anchoredPosition =
            new Vector2(Width, 0f);

        newPage.RectTransform.localScale =
            Vector3.one * startScale;

        newPage.RectTransform.localRotation =
            Quaternion.Euler(0f, -startRotation, 0f);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / transitionDuration
            );

            float moveT = EaseOutCubic(t);
            float scaleT = EaseOutBack(t);

            oldPage.RectTransform.anchoredPosition =
                Vector2.Lerp(
                    Vector2.zero,
                    new Vector2(-Width, 0f),
                    moveT
                );

            oldPage.RectTransform.localScale =
                Vector3.Lerp(
                    Vector3.one,
                    Vector3.one * 0.97f,
                    moveT
                );

            newPage.RectTransform.anchoredPosition =
                Vector2.Lerp(
                    new Vector2(Width, 0f),
                    Vector2.zero,
                    moveT
                );

            newPage.RectTransform.localScale =
                Vector3.Lerp(
                    Vector3.one * startScale,
                    Vector3.one,
                    scaleT
                );

            newPage.RectTransform.localRotation =
                Quaternion.Slerp(
                    Quaternion.Euler(0f, -startRotation, 0f),
                    Quaternion.identity,
                    moveT
                );

            await UniTask.Yield();
        }

        oldPage.HideInstant();

        ResetPage(oldPage);
        ResetPage(newPage);

        currentPage = newPage;
    }

    private async UniTask MovePreviousAsync(int targetIndex)
    {
        BookPage oldPage = currentPage;
        BookPage newPage = pages[targetIndex];

        ResetPage(oldPage);

        newPage.gameObject.SetActive(true);

        newPage.RectTransform.anchoredPosition =
            new Vector2(-Width, 0f);

        newPage.RectTransform.localScale =
            Vector3.one * startScale;

        newPage.RectTransform.localRotation =
            Quaternion.Euler(0f, startRotation, 0f);

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / transitionDuration
            );

            float moveT = EaseOutCubic(t);
            float scaleT = EaseOutBack(t);

            oldPage.RectTransform.anchoredPosition =
                Vector2.Lerp(
                    Vector2.zero,
                    new Vector2(Width, 0f),
                    moveT
                );

            oldPage.RectTransform.localScale =
                Vector3.Lerp(
                    Vector3.one,
                    Vector3.one * 0.97f,
                    moveT
                );

            newPage.RectTransform.anchoredPosition =
                Vector2.Lerp(
                    new Vector2(-Width, 0f),
                    Vector2.zero,
                    moveT
                );

            newPage.RectTransform.localScale =
                Vector3.Lerp(
                    Vector3.one * startScale,
                    Vector3.one,
                    scaleT
                );

            newPage.RectTransform.localRotation =
                Quaternion.Slerp(
                    Quaternion.Euler(0f, startRotation, 0f),
                    Quaternion.identity,
                    moveT
                );

            await UniTask.Yield();
        }

        oldPage.HideInstant();

        ResetPage(oldPage);
        ResetPage(newPage);

        currentPage = newPage;
    }

    private void ResetPage(BookPage page)
    {
        page.RectTransform.anchoredPosition = Vector2.zero;
        page.RectTransform.localScale = Vector3.one;
        page.RectTransform.localRotation = Quaternion.identity;
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f)
                   + c1 * Mathf.Pow(t - 1f, 2f);
    }

    #region Output

    public event Action<int> OnPageOpened;

    #endregion
}
