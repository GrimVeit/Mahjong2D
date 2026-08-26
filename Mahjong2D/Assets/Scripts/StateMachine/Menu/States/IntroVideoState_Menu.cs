using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroVideoState_Menu : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly UIRoot_Menu _sceneRoot;
    private readonly IVideoProvider _videoProvider;

    public IntroVideoState_Menu(IStateProvider stateProvider, UIRoot_Menu sceneRoot, IVideoProvider videoProvider)
    {
        _stateProvider = stateProvider;
        _sceneRoot = sceneRoot;
        _videoProvider = videoProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowIntroVideoPanel();
        _videoProvider.Play("Intro");

        await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: token);

        ChangeStateToIntroStart();
    }

    private void ChangeStateToIntroStart()
    {
        _stateProvider.SetState(_stateProvider.GetState<IntroStartState_Menu>());
    }
}
