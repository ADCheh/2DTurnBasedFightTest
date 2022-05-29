namespace Infrastructure
{
    public class EnemyTurnState : IState
    {
        private readonly BattleStateMachine _battleStateMachine;

        public EnemyTurnState(BattleStateMachine battleStateMachine)
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