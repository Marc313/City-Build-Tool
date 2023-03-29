using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingModeFSM : FSM
{
    public LayerMask groundLayers;
    public LayerMask buildingLayers;
    private BuildingCursor cursor;

    public BuildingModeFSM(IFSMOwner _owner, LayerMask _groundLayers, LayerMask _buildingLayers, BuildingCursor _cursor) : base(_owner)
    {
        groundLayers = _groundLayers;
        buildingLayers = _buildingLayers;
        cursor = _cursor;
    }

    protected override Dictionary<Type, State> CreateStatesDic()
    {
        states.Add(typeof(BuildState), new BuildState(groundLayers, cursor));
        states.Add(typeof(EditState), new EditState(groundLayers, buildingLayers, cursor));
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