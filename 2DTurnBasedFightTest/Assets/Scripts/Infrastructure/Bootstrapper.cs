using Infrastructure.States;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game(this);
            _game.BattleStateMachine.Enter<EntryState>();
        
            DontDestroyOnLoad(this);
        }
    }
}