using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeFSM : FSM
{
    public BuildingModeFSM() : FSM()
    {
    }

    protected override Dictionary<Type, State> CreateStatesDic()
    {
        states.Add(typeof(BuildState), new BuildState());
        states.Add(typeof(BuildState), new BuildState());
        states.Add(typeof(BuildState), new BuildState());
    }

    protected override State GetDefaultState()
    {
        return states[typeof(BuildState)];
    }
}
