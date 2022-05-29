using UnityEngine;

namespace Infrastructure
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerPositionTag = "PlayerPosition";
        private const string EnemyPositionTag = "EnemyPosition";
        
        private const string PlayerCharactersPath = "Characters/MinerBase";
        private const string EnemyCharactersPath = "Characters/MinerElite";
        private const string HudPath = "Hud/Hud";
        
        private readonly BattleStateMachine _battleStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(BattleStateMachine battleStateMachine, SceneLoader sceneLoader)
        {
            _battleStateMachine = battleStateMachine;
            _sceneLoader = sceneLoader;
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
            var playerPositions = GameObject.FindGameObjectsWithTag(PlayerPositionTag);
            var enemyPositions = GameObject.FindGameObjectsWithTag(EnemyPositionTag);

            foreach (var position in playerPositions)
            {
                GameObject playerChar = Instantiate(PlayerCharactersPath, position.transform);
            }
            
            foreach (var position in enemyPositions)
            {
                GameObject enemyChar = Instantiate(EnemyCharactersPath,position.transform);
            }
            
            Instantiate(HudPath);
            
            _battleStateMachine.Enter<PlayerTurnState>();
        }
        
        private GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        private GameObject Instantiate(string path, Transform at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab,at);
        }
    }
}