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

        PrepareAsync(
            videoPlay,
            destroyCancellationToken
        ).Forget();
    }

    private async UniTask PrepareAsync(
        VideoPlay videoPlay,
        CancellationToken cancellationToken)
    {
        var vp = videoPlay.VideoPlayer;

        videoPlay.Image.texture = videoPlay.Texture;
        videoPlay.Image.enabled = false;

        vp.Stop();
        vp.frame = 0;
        vp.time = 0;

        vp.sendFrameReadyEvents = true;

        vp.Prepare();

        await UniTask.WaitUntil(
            () => vp.isPrepared,
            cancellationToken: cancellationToken
        );

        vp.frame = 0;
        vp.time = 0;
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

        StartCoroutine(
            PlayRoutine(videoPlay, onComplete)
        );
    }

    private IEnumerator PlayRoutine(
        VideoPlay videoPlay,
        Action onComplete)
    {
        var vp = videoPlay.VideoPlayer;

        videoPlay.Image.texture = videoPlay.Texture;

        // Главное:
        // RawImage скрыт, пока первый нормальный кадр
        // реально не попадёт в RenderTexture.
        videoPlay.Image.enabled = false;

        // Если видео ещё не подготовлено,
        // сначала ждём подготовку.
        if (!vp.isPrepared)
        {
            vp.Prepare();

            yield return new WaitUntil(
                () => vp.isPrepared
            );
        }

        // Ставим видео в начало.
        vp.frame = 0;
        vp.time = 0;

        bool firstFrameReady = false;

        void OnFrameReady(VideoPlayer player, long frame)
        {
            if (frame == 0)
            {
                firstFrameReady = true;
            }
        }

        void OnVideoEnd(VideoPlayer player)
        {
            player.loopPointReached -= OnVideoEnd;
            onComplete?.Invoke();
        }

        vp.sendFrameReadyEvents = true;

        vp.frameReady += OnFrameReady;

        vp.loopPointReached -= OnVideoEnd;
        vp.loopPointReached += OnVideoEnd;

        // Запускаем видео.
        vp.Play();

        // Ждём именно реального первого кадра,
        // а не просто следующий кадр Unity.
        yield return new WaitUntil(
            () => firstFrameReady
        );

        vp.frameReady -= OnFrameReady;

        // Теперь RenderTexture содержит первый кадр видео.
        // Только после этого показываем RawImage.
        videoPlay.Image.enabled = true;

        // Видео уже играет и продолжает воспроизведение.
    }

    public void Stop(string id)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay == null || videoPlay.VideoPlayer == null)
            return;

        var vp = videoPlay.VideoPlayer;

        // Сначала скрываем изображение,
        // чтобы пользователь не увидел старый/переходный кадр.
        videoPlay.Image.enabled = false;

        vp.Stop();

        vp.frame = 0;
        vp.time = 0;
    }
}


[Serializable]
public class VideoPlayers
{
    [SerializeField] private List<VideoPlay> videoPlays = new();

    public async UniTask Initialize(
        CancellationToken cancellationToken)
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
                PrepareVideo(
                    videoPlay,
                    cancellationToken
                )
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

        vp.sendFrameReadyEvents = true;

        vp.Stop();
        vp.frame = 0;
        vp.time = 0;

        vp.Prepare();

        await UniTask.WaitUntil(
            () => vp.isPrepared,
            cancellationToken: cancellationToken
        );

        vp.frame = 0;
        vp.time = 0;
    }

    public VideoPlay GetVideoPlayById(string id)
    {
        return videoPlays.FirstOrDefault(
            data => data.Id == id
        );
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


