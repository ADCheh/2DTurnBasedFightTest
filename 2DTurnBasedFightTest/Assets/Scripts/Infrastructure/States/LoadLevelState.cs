using System.Collections.Generic;
using Infrastructure.Battle;
using Infrastructure.Factory;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly BattleStateMachine _battleStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;

        private IBattleController _battleController;

        public LoadLevelState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader, IGameFactory gameFactory, IBattleController battleController)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _battleController = battleController;
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
            _battleController.GetFightPositions();
            _battleController.InitPlayerCharacters(_gameFactory.CreatePlayerCharacters());
            _battleController.InitEnemyCharacters(_gameFactory.CreateEnemyCharacters());

            _battleStateMachine.Enter<PlayerTurnState>();
        }
    }
}