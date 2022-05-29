using System;
using Spine.Unity;
using UnityEngine;
using Object = System.Object;

namespace Infrastructure
{
    public class EntryState : IState
    {
        private const string Initial = "Initial";
        private readonly BattleStateMachine _battleStateMachine;
        private readonly SceneLoader _sceneLoader;

        public EntryState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            InitializeCharacters();
            _sceneLoader.Load(Initial,EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _battleStateMachine.Enter<LoadLevelState, string>("Main");
        }

        public void Exit()
        {
            
        }

        private void InitializeCharacters()
        {
            
        }
    }
}