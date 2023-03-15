using UnityEngine;

public class BuildState : State
{
    public BuildState()
    {

    }

    public override void onEnter()
    {
        Debug.Log("Building");
    }

    public override void onExit()
    {
    }

    public override void onFixedUpdate()
    {
    }

    public override void onUpdate()
    {
    }
}
