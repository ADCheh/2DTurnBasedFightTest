using Infrastructure.Services;
using Infrastructure.States;

namespace Infrastructure
{
    public class Game
    {
        public BattleStateMachine BattleStateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            BattleStateMachine = new BattleStateMachine(new SceneLoader(coroutineRunner), AllServices.Container);
        }

    }
}
