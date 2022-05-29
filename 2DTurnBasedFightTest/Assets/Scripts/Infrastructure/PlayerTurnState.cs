using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure
{
    public class PlayerTurnState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;
        

        public PlayerTurnState(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
        }
        public void Enter()
        {

        }

        public void Exit()
        {
            
        }
        
        
    }
}