using System.Collections.Generic;
using BaCon;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Common scene entry point. Each scene configures its own sounds in the Inspector.
/// </summary>
public abstract class SceneEntryPoint : MonoBehaviour, ISceneEntry
{
    [Header("Scene sounds")]
    [SerializeField] private List<Sound> sounds = new();

    protected SceneService SceneService { get; private set; }

    private StoreSoundSettingsPresenter storeSoundSettingsPresenter;
    private SoundPresenter soundPresenter;

    public virtual async UniTask Initialize(DIContainer container)
    {
        SceneService = container.Resolve<SceneService>();

        storeSoundSettingsPresenter = new StoreSoundSettingsPresenter(
            new StoreSoundSettingsModel(
                PlayerPrefsKeys.AUDIO_VOLUME_SOUND,
                PlayerPrefsKeys.AUDIO_VOLUME_MUSIC,
                PlayerPrefsKeys.AUDIO_MUTED
            )
        );
        container.RegisterInstance<ISoundSettingsEventsProvider>(storeSoundSettingsPresenter);
        container.RegisterInstance<ISoundSettingsInfoProvider>(storeSoundSettingsPresenter);
        container.RegisterInstance<ISoundSettingsProvider>(storeSoundSettingsPresenter);

        soundPresenter = new SoundPresenter(new SoundModel(sounds, storeSoundSettingsPresenter, storeSoundSettingsPresenter));
        container.RegisterInstance<ISoundProvider>(soundPresenter);

        await OnSceneInitialized(container);
    }

    public virtual async UniTask ShutDown()
    {
        await OnSceneShuttingDown();

        soundPresenter?.Dispose();
        soundPresenter = null;

        storeSoundSettingsPresenter?.Dispose();
        storeSoundSettingsPresenter = null;

        SceneService = null;
    }

    protected virtual UniTask OnSceneInitialized(DIContainer container) => UniTask.CompletedTask;
    protected virtual UniTask OnSceneShuttingDown() => UniTask.CompletedTask;
}
