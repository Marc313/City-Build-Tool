using UnityEngine;

public class DemolishState : State
{
    private LayerMask buildingLayer;

    public DemolishState(LayerMask _buildingLayer) 
    { 
        buildingLayer = _buildingLayer;
    }


    public override void OnEnter()
    {
        Debug.Log("Demolish");
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
