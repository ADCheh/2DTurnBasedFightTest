using Infrastructure.Battle;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.States
{
    public class EnemyTurnState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;
        private readonly IBattleController _battleController;
        public EnemyTurnState(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _battleController = AllServices.Container.Single<IBattleController>();
        }
        public void Enter()
        {
            _battleController.GetPlayerCharacter();
            _battleController.GetEnemyCharacter();
            
            _battleController.FightHandled.AddListener(EndEnemyTurn);
            _battleController.HandleFight(this);
        }

        public void Exit()
        {
            
        }
        
        private void EndEnemyTurn()
        {
            _battleController.ClearActiveCharacters();
            _battleController.FightHandled.RemoveAllListeners();
            _battleStateMachine.Enter<PlayerTurnState>();
        }
    }
}