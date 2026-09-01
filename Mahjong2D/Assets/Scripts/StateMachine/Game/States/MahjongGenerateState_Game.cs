using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MahjongGenerateState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;
    private readonly IMahjongProvider _mahjongProvider;
    private readonly UIRoot_Game _sceneRoot;
    private readonly ILevelInfoProvider _levelInfoProvider;
    private readonly IMahjongTilesSpritesProvider _spritesProvider;
    private readonly ICardDesignInfoProvider _cardDesignInfoProvider;

    public MahjongGenerateState_Game(IStateProvider stateProvider, IMahjongProvider mahjongProvider, UIRoot_Game sceneRoot, ILevelInfoProvider levelInfoProvider, IMahjongTilesSpritesProvider spritesProvider, ICardDesignInfoProvider cardDesignInfoProvider)
    {
        _stateProvider = stateProvider;
        _mahjongProvider = mahjongProvider;
        _sceneRoot = sceneRoot;
        _levelInfoProvider = levelInfoProvider;
        _spritesProvider = spritesProvider;
        _cardDesignInfoProvider = cardDesignInfoProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowMainPanel();

        await _mahjongProvider.GenerateBoard(_spritesProvider.GetRandomTiles(_cardDesignInfoProvider.CurrentCardDesignIndex, MahjongTileCountHelper.GetTileCount(_levelInfoProvider.Level + 1)).ToList());

        ChangeStateToMain();
    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Game>());
    }
}
