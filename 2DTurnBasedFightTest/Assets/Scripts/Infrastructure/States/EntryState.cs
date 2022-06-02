using Infrastructure.AssetManagement;
using Infrastructure.Battle;
using Infrastructure.Factory;
using Infrastructure.Services;

namespace Infrastructure.States
{
    public class EntryState : IState
    {
        private const string Initial = "Initial";
        private readonly BattleStateMachine _battleStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly ICoroutineRunner _coroutineRunner;

        public EntryState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial,EnterLoadLevel);
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>()));
            _services.RegisterSingle<IBattleController>(new BattleController(_coroutineRunner));
        }

        private void EnterLoadLevel()
        {
            _battleStateMachine.Enter<LoadLevelState, string>("Main");
        }

        public void Exit()
        {
            
        }
    }
}