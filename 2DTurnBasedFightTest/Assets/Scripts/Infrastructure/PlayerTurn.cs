namespace Infrastructure
{
    public class PlayerTurn : IState
    {
        private readonly BattleStateMachine _battleStateMachine;

        public PlayerTurn(BattleStateMachine battleStateMachine)
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