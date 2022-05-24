using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

public class Game : MonoBehaviour
{
    public BattleStateMachine BattleStateMachine;
    void Awake()
    {
        BattleStateMachine = new BattleStateMachine();
        BattleStateMachine.Enter<EntryState>();
    }

}
