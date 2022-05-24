namespace Infrastructure
{
    public class EnemyTurn : IState
    {
        private readonly BattleStateMachine _battleStateMachine;

        public EnemyTurn(BattleStateMachine battleStateMachine)
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