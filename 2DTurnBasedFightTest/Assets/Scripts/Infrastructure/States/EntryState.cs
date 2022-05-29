using Infrastructure.AssetManagement;
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

        public EntryState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
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