using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected FSM fsm;

    public void InjectFSM(FSM fsm)
    {
        this.fsm = fsm;
    }

    public abstract void onUpdate();
    public abstract void onFixedUpdate();
    public abstract void onEnter();
    public abstract void onExit();
}
