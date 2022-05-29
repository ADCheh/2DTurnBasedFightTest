using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure
{
    public class BattleStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        //public UnityEvent<IState> BattleStateChanged = new UnityEvent<IState>();

        public BattleStateMachine(SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(EntryState)] = new EntryState(this,sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this,sceneLoader),
                [typeof(PlayerTurnState)] = new PlayerTurnState(this),
                [typeof(EnemyTurnState)] = new EnemyTurnState(this)
            };
        }
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
            
            //BattleStateChanged?.Invoke(state);
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
            
            //BattleStateChanged?.Invoke(state);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}