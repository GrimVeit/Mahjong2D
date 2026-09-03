using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registration_NicknameProfileUnputState_Menu : IState
{
    private readonly IStateProvider _stateProvider;
    private readonly IPlayerProfileEventsProvider _profileEvents;

    private readonly UIRoot_Menu _sceneRoot;

    public Registration_NicknameProfileUnputState_Menu(
        IStateProvider stateProvider,
        IPlayerProfileEventsProvider profileEvents,
        UIRoot_Menu sceneRoot)
    {
        _stateProvider = stateProvider;
        _profileEvents = profileEvents;
        _sceneRoot = sceneRoot;
    }

    public void Enter()
    {
        _profileEvents.OnProfileChanged += OnProfileChanged;

        _sceneRoot.ShowRegistrationPanel();
    }

    public void Exit()
    {
        _profileEvents.OnProfileChanged -= OnProfileChanged;

        _sceneRoot.HideRegistrationPanel();
    }

    private void OnProfileChanged(PlayerProfile profile)
    {
        ChangeStateToRegistration();
    }

    private void ChangeStateToRegistration()
    {
        _stateProvider.SetState(_stateProvider.GetState<RegistrationState_Game>());
    }
}
