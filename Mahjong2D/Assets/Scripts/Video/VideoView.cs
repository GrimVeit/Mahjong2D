using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoView : View
{
    [SerializeField] private VideoPlayers videoPlayers;

    public async UniTask Initialize()
    {
        await videoPlayers.Initialize(destroyCancellationToken);
    }

    public void Prepare(string id)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay == null)
        {
            Debug.LogWarning($"Video with id: {id} not found!");
            return;
        }

        if (videoPlay.VideoPlayer == null)
        {
            Debug.LogWarning($"VideoPlayer with id: {id} not found!");
            return;
        }

        var vp = videoPlay.VideoPlayer;

        videoPlay.Image.texture = videoPlay.Texture;
        videoPlay.Image.enabled = false;

        vp.Stop();
        vp.frame = 0;
        vp.time = 0;

        vp.Prepare();
    }

    public void Play(string id, Action onComplete = null)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay == null)
        {
            Debug.LogWarning($"Video with id: {id} not found!");
            return;
        }

        if (videoPlay.VideoPlayer == null)
        {
            Debug.LogWarning($"VideoPlayer with id: {id} not found!");
            return;
        }

        var vp = videoPlay.VideoPlayer;

        videoPlay.Image.texture = videoPlay.Texture;
        videoPlay.Image.enabled = false;

        vp.Stop();
        vp.frame = 0;
        vp.time = 0;

        vp.loopPointReached -= OnVideoEnd;
        vp.loopPointReached += OnVideoEnd;

        void OnVideoEnd(VideoPlayer player)
        {
            player.loopPointReached -= OnVideoEnd;
            onComplete?.Invoke();
        }

        StartVideo();

        void StartVideo()
        {
            StartCoroutine(StartRoutine());
        }

        IEnumerator StartRoutine()
        {
            vp.frame = 0;

            // Запускаем VideoPlayer, чтобы Unity отрендерила первый кадр
            vp.Play();

            yield return null;

            // Останавливаемся на первом кадре
            vp.Pause();

            yield return null;

            // Теперь первый кадр уже находится в RenderTexture
            videoPlay.Image.enabled = true;

            // Продолжаем воспроизведение
            vp.Play();
        }
    }

    public void Stop(string id)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay == null || videoPlay.VideoPlayer == null)
            return;

        videoPlay.VideoPlayer.Stop();
        videoPlay.VideoPlayer.frame = 0;
        videoPlay.VideoPlayer.time = 0;

        videoPlay.Image.enabled = false;
    }
}

[Serializable]
public class VideoPlayers
{
    [SerializeField] private List<VideoPlay> videoPlays = new();

    public async UniTask Initialize(CancellationToken cancellationToken)
    {
        var prepareTasks = new List<UniTask>();

        foreach (var videoPlay in videoPlays)
        {
            if (!videoPlay.IsAwakePrepare)
                continue;

            if (videoPlay.VideoPlayer == null)
            {
                Debug.LogWarning(
                    $"VideoPlayer with id: {videoPlay.Id} not found!"
                );

                continue;
            }

            prepareTasks.Add(
                PrepareVideo(videoPlay, cancellationToken)
            );
        }

        if (prepareTasks.Count == 0)
            return;

        await UniTask.WhenAll(prepareTasks);
    }

    private async UniTask PrepareVideo(
        VideoPlay videoPlay,
        CancellationToken cancellationToken)
    {
        var vp = videoPlay.VideoPlayer;

        videoPlay.Image.texture = videoPlay.Texture;
        videoPlay.Image.enabled = false;

        vp.Stop();
        vp.frame = 0;
        vp.time = 0;

        // Запускаем подготовку
        vp.Prepare();

        // Ждём, пока VideoPlayer полностью подготовится
        await UniTask.WaitUntil(
            () => vp.isPrepared,
            cancellationToken: cancellationToken
        );
    }

    public VideoPlay GetVideoPlayById(string id)
    {
        return videoPlays.FirstOrDefault(data => data.Id == id);
    }
}

[Serializable]
public class VideoPlay
{
    [SerializeField] private string id;
    [SerializeField] private RawImage image;
    [SerializeField] private Texture texture;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private bool isAwakePrepare;

    public string Id => id;
    public VideoPlayer VideoPlayer => videoPlayer;
    public RawImage Image => image;
    public Texture Texture => texture;
    public bool IsAwakePrepare => isAwakePrepare;
}
