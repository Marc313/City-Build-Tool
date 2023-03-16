using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeFSM : FSM
{
    public LayerMask groundLayers;
    public LayerMask buildingLayers;

    public BuildingModeFSM(IFSMOwner _owner, LayerMask _groundLayers, LayerMask _buildingLayers) : base(_owner)
    {
        groundLayers= _groundLayers;
        buildingLayers= _buildingLayers;
    }

    protected override Dictionary<Type, State> CreateStatesDic()
    {
        states.Add(typeof(BuildState), new BuildState(groundLayers));
        states.Add(typeof(DemolishState), new DemolishState(buildingLayers));

        foreach (State state in states.Values)
        {
            state.InjectFSM(this, owner.sharedData);
        }

        return states;
    }

    protected override State GetDefaultState()
    {
        return states[typeof(BuildState)];
    }
}