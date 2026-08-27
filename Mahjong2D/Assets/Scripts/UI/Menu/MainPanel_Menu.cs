using System;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel_Menu : MoveFadePanel
{
    [Header("Buttons")]
    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonWallet;
    [SerializeField] private Button buttonStore;
    [SerializeField] private Button buttonLeaderboard;
    [SerializeField] private Button buttonPlay;

    public override void Initialize()
    {
        base.Initialize();

        buttonSettings.onClick.AddListener(ClickSettings);
        buttonLeaderboard.onClick.AddListener(ClickLeaderboard);
        buttonStore.onClick.AddListener(ClickStore);
        buttonWallet.onClick.AddListener(ClickWallet);
        buttonPlay.onClick.AddListener(ClickPlay);
    }

    public override void Dispose()
    {
        base.Dispose();

        buttonSettings.onClick.RemoveListener(ClickSettings);
        buttonLeaderboard.onClick.RemoveListener(ClickLeaderboard);
        buttonStore.onClick.RemoveListener(ClickStore);
        buttonWallet.onClick.RemoveListener(ClickWallet);
        buttonPlay.onClick.AddListener(ClickPlay);
    }

    #region Output

    public event Action OnClickSettings;
    public event Action OnClickWallet;
    public event Action OnClickLeaderboard;
    public event Action OnClickStore;
    public event Action OnClickPlay;

    private void ClickSettings()
    {
        OnClickSettings?.Invoke();
    }

    private void ClickWallet()
    {
        OnClickWallet?.Invoke();
    }

    private void ClickLeaderboard()
    {
        OnClickLeaderboard?.Invoke();
    }

    private void ClickStore()
    {
        OnClickStore?.Invoke();
    }

    private void ClickPlay()
    {
        OnClickPlay?.Invoke();
    }

    #endregion
}
