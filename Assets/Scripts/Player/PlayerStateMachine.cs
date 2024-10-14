using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public PlayerState lastState { get; private set; }

    public void Initialize(PlayerState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        lastState = currentState;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
