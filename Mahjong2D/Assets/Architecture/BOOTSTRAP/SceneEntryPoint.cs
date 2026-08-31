using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Common scene entry point. Each scene configures its own sounds in the Inspector.
/// </summary>
/// <summary>
/// Common scene entry point.
/// Handles common scene systems and application lifecycle.
/// </summary>
public abstract class SceneEntryPoint : MonoBehaviour, ISceneEntry
{
    [Header("Scene sounds")]
    [SerializeField] private List<Sound> sounds = new();
    [SerializeField] private List<BackgroundDataSO> backgroundDatas = new();
    [SerializeField] private List<CardsDesignDataSO> cardsDesignDatas = new();

    // SOUND
    protected StoreSoundSettingsPresenter _storeSoundSettingsPresenter;
    protected SoundPresenter _soundPresenter;

    // MONEY
    protected StoreMoneyPresenter _storeMoneyPresenter;

    //LEVEL
    protected StoreLevelPresenter _storeLevelPresenter;

    //BACKGROUND
    protected StoreBackgroundPresenter _storeBackgroundPresenter;

    //CARD DESIGN
    protected StoreCardsDesignPresenter _storeCardsDesignPresenter;

    public virtual async UniTask Initialize(DIContainer container)
    {
        // -------------------------------------------------
        // SOUND SETTINGS
        // -------------------------------------------------

        _storeSoundSettingsPresenter = new StoreSoundSettingsPresenter(new StoreSoundSettingsModel(
                PlayerPrefsKeys.AUDIO_VOLUME_SOUND,
                PlayerPrefsKeys.AUDIO_VOLUME_MUSIC,
                PlayerPrefsKeys.AUDIO_MUTED
            )
        );

        container.RegisterInstance<ISoundSettingsEventsProvider>(_storeSoundSettingsPresenter);
        container.RegisterInstance<ISoundSettingsInfoProvider>(_storeSoundSettingsPresenter);
        container.RegisterInstance<ISoundSettingsProvider>(_storeSoundSettingsPresenter);
        _storeSoundSettingsPresenter.Initialize();

        // -------------------------------------------------
        // SOUND
        // -------------------------------------------------

        _soundPresenter = new SoundPresenter(new SoundModel(sounds, _storeSoundSettingsPresenter, _storeSoundSettingsPresenter));

        container.RegisterInstance<ISoundProvider>(_soundPresenter);

        _soundPresenter.Initialize();

        // -------------------------------------------------
        // MONEY
        // -------------------------------------------------

        _storeMoneyPresenter = new StoreMoneyPresenter(new StoreMoneyModel(PlayerPrefsKeys.MONEY_BALANCE));

        container.RegisterInstance<IMoneyEventsProvider>(_storeMoneyPresenter);
        container.RegisterInstance<IMoneyInfoProvider>(_storeMoneyPresenter);
        container.RegisterInstance<IMoneyProvider>(_storeMoneyPresenter);
        _storeMoneyPresenter.Initialize();

        // -------------------------------------------------
        // LEVEL
        // -------------------------------------------------

        _storeLevelPresenter = new StoreLevelPresenter(new StoreLevelModel(PlayerPrefsKeys.LEVEL));

        container.RegisterInstance<ILevelEventsProvider>(_storeLevelPresenter);
        container.RegisterInstance<ILevelInfoProvider>(_storeLevelPresenter);
        container.RegisterInstance<ILevelProvider>(_storeLevelPresenter);
        _storeLevelPresenter.Initialize();


        // -------------------------------------------------
        // BACKGROUND
        // -------------------------------------------------

        _storeBackgroundPresenter = new StoreBackgroundPresenter(new StoreBackgroundModel(backgroundDatas));

        container.RegisterInstance<IBackgroundInfoProvider>(_storeBackgroundPresenter);
        container.RegisterInstance<IBackgroundListener>(_storeBackgroundPresenter);
        container.RegisterInstance<IBackgroundProvider>(_storeBackgroundPresenter);
        _storeBackgroundPresenter.Initialize();


        // -------------------------------------------------
        // CARD DESIGNS
        // -------------------------------------------------

        _storeCardsDesignPresenter = new StoreCardsDesignPresenter(new StoreCardsDesignModel(cardsDesignDatas));

        container.RegisterInstance<ICardDesignInfoProvider>(_storeCardsDesignPresenter);
        container.RegisterInstance<ICardDesignListener>(_storeCardsDesignPresenter);
        container.RegisterInstance<ICardDesignProvider>(_storeCardsDesignPresenter);
        _storeCardsDesignPresenter.Initialize();

        await OnBaseInitialized(container);
    }

    public virtual async UniTask ShutDown()
    {
        _soundPresenter?.Dispose();
        _storeSoundSettingsPresenter?.Dispose();
        _storeLevelPresenter?.Dispose();
        _storeBackgroundPresenter?.Dispose();
        _storeCardsDesignPresenter?.Dispose();

        await OnBaseShutdown();
    }

    // -----------------------------------------------------
    // APPLICATION LIFECYCLE
    // -----------------------------------------------------

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveApplicationData();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveApplicationData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveApplicationData();
    }

    private void SaveApplicationData()
    {
        OnApplicationSaving();

        PlayerPrefs.Save();
    }

    // -----------------------------------------------------
    // LIFECYCLE HOOKS
    // -----------------------------------------------------

    public virtual UniTask BeforeShutdown()
        => UniTask.CompletedTask;
    protected virtual UniTask OnBaseInitialized(DIContainer container)
        => UniTask.CompletedTask;

    protected virtual UniTask OnBaseShutdown()
        => UniTask.CompletedTask;

    protected virtual UniTask OnSceneInitialized(DIContainer container)
        => UniTask.CompletedTask;

    protected virtual UniTask OnSceneShuttingDown()
        => UniTask.CompletedTask;

    /// <summary>
    /// Called when application data should be saved.
    /// </summary>
    protected virtual void OnApplicationSaving()
    {
        _storeSoundSettingsPresenter?.Dispose();
        _storeMoneyPresenter?.Dispose();
        _storeLevelPresenter?.Dispose();
        _storeBackgroundPresenter?.Dispose();
        _storeCardsDesignPresenter?.Dispose();
    }
}
