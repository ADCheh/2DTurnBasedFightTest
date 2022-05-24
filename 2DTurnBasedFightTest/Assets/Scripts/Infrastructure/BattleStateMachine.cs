using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public class BattleStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;

        public BattleStateMachine()
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(EntryState)] = new EntryState(this)
            };
        }
        public void Enter<TState>() where TState : IState
        {
            _activeState?.Exit();
            IState state = _states[typeof(TState)];
            _activeState = state;
            state.Enter();
        }
    }
}