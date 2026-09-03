using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registration_StartMainState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IPlayerDatabaseProvider _playerDatabaseProvider;
    private readonly IAuthenticationInfoProvider _authenticationInfoProvider;
    private readonly IPlayerProfileInfoProvider _profileInfoProvider;
    private readonly ILevelInfoProvider _levelInfoProvider;

    public Registration_StartMainState_Menu(IStateProvider stateProvider, IPlayerDatabaseProvider playerDatabaseProvider, IAuthenticationInfoProvider authenticationInfoProvider, IPlayerProfileInfoProvider playerProfileInfoProvider, ILevelInfoProvider levelInfoProvider)
    {
        _stateProvider = stateProvider;
        _playerDatabaseProvider = playerDatabaseProvider;
        _authenticationInfoProvider = authenticationInfoProvider;
        _profileInfoProvider = playerProfileInfoProvider;
        _levelInfoProvider = levelInfoProvider;
    }

    public void Enter()
    {
        if (_authenticationInfoProvider.IsAuthorized)
        {
            _playerDatabaseProvider.CreateOrUpdatePlayer(new PlayerData(_profileInfoProvider.Profile.Nickname, _levelInfoProvider.Level));
        }

        ChangeStateToMain();
    }

    public void Exit()
    {

    }

    private void ChangeStateToMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<MainState_Menu>());
    }
}
