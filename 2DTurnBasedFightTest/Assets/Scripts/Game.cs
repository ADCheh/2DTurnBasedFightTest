using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Object = UnityEngine.Object;

public class Game
{
    public BattleStateMachine BattleStateMachine;
    public BattleController BattleController;

    

    public Game(ICoroutineRunner coroutineRunner)
    {
        BattleStateMachine = new BattleStateMachine(new SceneLoader(coroutineRunner));
        BattleController = new BattleController(BattleStateMachine, new UiController());
    }

}
