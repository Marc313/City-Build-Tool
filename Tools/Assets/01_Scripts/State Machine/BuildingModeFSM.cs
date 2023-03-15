using System;
using System.Collections.Generic;

public class BuildingModeFSM : FSM
{
    public BuildingModeFSM()
    {
    }

    protected override Dictionary<Type, State> CreateStatesDic()
    {
        states.Add(typeof(BuildState), new BuildState());
        states.Add(typeof(DemolishState), new DemolishState());
        return states;
    }

    protected override State GetDefaultState()
    {
        return states[typeof(BuildState)];
    }
}