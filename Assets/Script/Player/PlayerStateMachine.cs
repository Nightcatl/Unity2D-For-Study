using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    public PlayerState lastState { get; private set; }

    public void Initialize(PlayerState _strtState)
    {
        if(currentState != null)
            currentState.Exit();

        currentState = _strtState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        lastState = currentState;
        currentState = _newState;
        currentState.Enter();
    }
}
