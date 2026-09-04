using Cysharp.Threading.Tasks;
using System.Threading;

public class RegistrationState_Game : AsyncState
{
    private readonly IStateProvider _stateProvider;

    private readonly IPlayerProfileInfoProvider _profileInfo;
    private readonly IAuthenticationProvider _authentication;
    private readonly IPlayerDatabaseProvider _database;
    private readonly IMoneyProvider _moneyProvider;
    private readonly ILevelProvider _levelProvider;

    private readonly UIRoot_Menu _sceneRoot;

    public RegistrationState_Game(
        IStateProvider stateProvider,
        IPlayerProfileInfoProvider profileInfo,
        IAuthenticationProvider authentication,
        IPlayerDatabaseProvider database,
        UIRoot_Menu sceneRoot,
        IMoneyProvider moneyProvider,
        ILevelProvider levelProvider)
    {
        _stateProvider = stateProvider;
        _profileInfo = profileInfo;
        _authentication = authentication;
        _database = database;
        _sceneRoot = sceneRoot;
        _moneyProvider = moneyProvider;
        _levelProvider = levelProvider;
    }

    protected override async UniTask EnterAsync(CancellationToken token)
    {
        _sceneRoot.ShowRegistrationLoadingPanel();

        PlayerProfile profile = _profileInfo.Profile;

        AuthenticationResult authResult = await _authentication.Register(profile.Nickname);

        if (authResult != AuthenticationResult.Success)
        {
            ChangeStateToProfileInput();
            return;
        }

        _levelProvider.SetZero();
        _moneyProvider.SetZero();

        PlayerData playerData = new(profile.Nickname, 0);

        //DatabaseResult databaseResult = 
        await _database.CreateOrUpdatePlayer(playerData);

        //if (databaseResult != DatabaseResult.Success)
        //{
        //    ChangeStateToProfileInput();
        //    return;
        //}

        ChangeStateToStartMain();
    }

    public override void Exit()
    {
        base.Exit();

        _sceneRoot.HideRegistrationLoadingPanel();
    }

    private void ChangeStateToProfileInput()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_NicknameProfileUnputState_Menu>());
    }

    private void ChangeStateToStartMain()
    {
        _stateProvider.SetState(_stateProvider.GetState<Registration_StartMainState_Menu>());
    }
}
