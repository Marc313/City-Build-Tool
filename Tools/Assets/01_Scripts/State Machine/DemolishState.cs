using System;
using UnityEngine;

public class DemolishState : State
{
    // Raycast
    private Raycaster raycaster;
    private RaycastHit hit;
    private LayerMask buildingLayer;

    // Demolish
    private Action<GameObject> DemolishObject;


    public DemolishState(LayerMask _buildingLayer) 
    { 
        buildingLayer = _buildingLayer;
        raycaster = Raycaster.Instance;
    }


    public override void OnEnter()
    {
        CursorManager.Instance.isAllowedOnScreen = true;
        DemolishObject = scratchPad.Get<Action<GameObject>>("DemolishFunc");

        Debug.Log("Demolish");
    }

    public override void OnExit()
    {
        CursorManager.Instance.isAllowedOnScreen = false;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        if (raycaster.GetRaycastHit(out hit, buildingLayer))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject building = hit.collider.gameObject;
                Debug.Log(building.name);
                DemolishObject?.Invoke(building);
            }
        }
    }
}
