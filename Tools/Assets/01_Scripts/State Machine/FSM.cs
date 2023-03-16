using System;
using System.Collections.Generic;

public abstract class FSM
{
    protected Dictionary<Type, State> states = new Dictionary<Type, State>();
    protected State currentState;
    protected IFSMOwner owner;

    // FSM inject zichzelf in States, nu laten we 
    public FSM (IFSMOwner _owner)
    {
        owner = _owner;
    }

    public void Start()
    {
        states = CreateStatesDic();
        currentState = GetDefaultState();
        currentState.OnEnter();
    }

    protected abstract Dictionary<Type, State> CreateStatesDic();
    protected abstract State GetDefaultState();

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void ExitCurrentState()
    {
        currentState?.OnExit();
        currentState = null;
    }

    public void SwitchState(Type stateType)
    {
        if (states.ContainsKey(stateType))
        {
            currentState?.OnExit();
            State newState = states[stateType];
            currentState = newState;
            currentState?.OnEnter();
        }
    }

    public State GetCurrentState() { return currentState; }
}
