using System;

namespace Infrastructure
{
    public class EntryState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;

        public EntryState(BattleStateMachine battleStateMachine)
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