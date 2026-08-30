using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class BookPagesControlView : View
{
    [SerializeField] private Button buttonPrevious;
    [SerializeField] private Button buttonNext;

    [SerializeField] private UIEffect effectPrevious;
    [SerializeField] private UIEffect effectNext;

    public void Initialize()
    {
        buttonPrevious.onClick.AddListener(ClickPrevious);
        buttonNext.onClick.AddListener(ClickNext);

        effectNext.Initialize();
        effectPrevious.Initialize();
    }

    public void Dispose()
    {
        buttonPrevious.onClick.RemoveListener(ClickPrevious);
        buttonNext.onClick.RemoveListener(ClickNext);

        effectNext.Dispose();
        effectPrevious.Dispose();
    }

    public void EnablePrevious()
    {
        buttonPrevious.interactable = true;

        effectPrevious.PlayShow();
    }

    public void DisablePrevious()
    {
        buttonPrevious.interactable = false;

        effectPrevious.PlayHide();
    }

    public void EnableNext()
    {
        buttonNext.interactable = true;
        
        effectNext.PlayShow();
    }

    public void DisableNext()
    {
        buttonNext.interactable = false;

        effectNext.PlayHide();
    }

    #region Output

    public event Action OnClickPrevious;
    public event Action OnClickNext;

    private void ClickPrevious()
    {
        OnClickPrevious?.Invoke();
    }

    private void ClickNext()
    {
        OnClickNext?.Invoke();
    }

    #endregion
}
