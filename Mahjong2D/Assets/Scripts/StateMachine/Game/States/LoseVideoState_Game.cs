using Cysharp.Threading.Tasks;
using System.Threading;

public class LoseVideoState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ISoundProvider _soundProvider;
    private readonly ISound _soundBackground_1;
    private readonly ISound _soundBackground_2;

    public LoseVideoState_Game(IStateProvider stateProvider, UIRoot_Game sceneRoot, ISoundProvider soundProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _soundProvider = soundProvider;
        _soundBackground_1 = _soundProvider.GetSound("Background_Music");
        _soundBackground_2 = _soundProvider.GetSound("Background_Vocals");
    }


    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowBackgroundResultPanel();

        _soundBackground_1.SetVolumeEnd(0, 0.2f);
        _soundBackground_2.SetVolumeEnd(0, 0.2f);

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.ShowLoseVideoPanel();

        _soundProvider.PlayOneShot("Fail_Background");

        await UniTask.Delay(100, cancellationToken: token);

        _soundProvider.PlayOneShot("Fail_Wave");
        _soundProvider.PlayOneShot("Fail_Pipe");

        await UniTask.Delay(2600, cancellationToken: token);

        _soundProvider.PlayOneShot("Fail_Close");

        await UniTask.Delay(200, cancellationToken: token);

        _sceneRoot.HideLoseVideoPanel();

        ChangeStateToLose();
    }

    private void ChangeStateToLose()
    {
        _stateProvider.SetState(_stateProvider.GetState<LoseState_Game>());
    }
}
