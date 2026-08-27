using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MahjongGenerateState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly IMahjongProvider _mahjongProvider;
    private readonly List<Sprite> _sprites = new();
    private readonly UIRoot_Game _sceneRoot;

    public MahjongGenerateState_Game(IStateProvider stateProvider, IMahjongProvider mahjongProvider, List<Sprite> sprites, UIRoot_Game sceneRoot)
    {
        _stateProvider = stateProvider;
        _mahjongProvider = mahjongProvider;
        _sprites = sprites;
        _sceneRoot = sceneRoot;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowMainPanel();

        await _mahjongProvider.GenerateBoard(_sprites);

        ChangeStateToMain();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Game>());
    }
}
