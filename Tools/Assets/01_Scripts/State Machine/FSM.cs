using System;
using System.Collections.Generic;

public abstract class FSM
{
    protected Dictionary<Type, State> states = new Dictionary<Type, State>();
    protected State currentState;

    // FSM inject zichzelf in States, nu laten we 
    public FSM ()
    {
        states = CreateStatesDic();
        currentState = GetDefaultState();
    }

    protected abstract Dictionary<Type, State> CreateStatesDic();
    protected abstract State GetDefaultState();

    public void OnUpdate()
    {
        currentState?.onUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState?.onFixedUpdate();
    }

    public void ExitCurrentState()
    {
        currentState?.onExit();
        currentState = null;
    }

    public void SwitchState(Type stateType)
    {
        if (states.ContainsKey(stateType))
        {
            currentState?.onExit();
            State newState = states[stateType];
            currentState = newState;
            currentState?.onEnter();
        }
    }

    public State GetCurrentState() { return currentState; }
}
