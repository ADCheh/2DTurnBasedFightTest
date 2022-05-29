using System.Collections.Generic;
using Infrastructure.Factory;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly BattleStateMachine _battleStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;

        public LoadLevelState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
        }
        
        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            List<GameObject> playerCharacters = _gameFactory.CreatePlayerCharacters();
            List<GameObject> enemyCharacters = _gameFactory.CreateEnemyCharacters();

            _gameFactory.CreateHud();
            
            _battleStateMachine.Enter<PlayerTurnState>();
        }
    }
}