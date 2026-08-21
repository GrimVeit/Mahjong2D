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

    private SoundPresenter soundPresenter;

    public virtual async UniTask Initialize(DIContainer container)
    {
        SceneService = container.Resolve<SceneService>();

        soundPresenter = new SoundPresenter(new SoundModel(sounds, "audio.muted", "audio.sound.volume","audio.music.volume"));
        soundPresenter.Initialize();

        container.RegisterInstance<ISoundProvider>(soundPresenter);
        container.RegisterInstance<ISoundVolumeProvider>(soundPresenter);

        await OnSceneInitialized(container);
    }

    public virtual async UniTask ShutDown()
    {
        await OnSceneShuttingDown();
        soundPresenter?.Dispose();
        soundPresenter = null;
    }

    protected virtual UniTask OnSceneInitialized(DIContainer container) => UniTask.CompletedTask;
    protected virtual UniTask OnSceneShuttingDown() => UniTask.CompletedTask;
}
