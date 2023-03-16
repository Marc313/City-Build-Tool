public abstract class State
{
    protected FSM fsm;
    protected ScratchPad scratchPad;

    public void InjectFSM(FSM _fsm, ScratchPad _scratchPad)
    {
        fsm = _fsm;
        scratchPad = _scratchPad;
    }

    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnEnter();
    public abstract void OnExit();
}
