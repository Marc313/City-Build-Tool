using System;
using System.Windows.Forms;
using UnityEngine;

public class DemolishState : State
{
    // Raycast
    private Raycaster raycaster;
    private RaycastHit hit;
    private LayerMask buildingLayer;

    // Demolish
    private Action<GameObject> DemolishObject;
    private BuildingCursor cursorInd;

    public DemolishState(LayerMask _buildingLayer, BuildingCursor _cursor) 
    { 
        buildingLayer = _buildingLayer;
        raycaster = Raycaster.Instance;
        cursorInd = _cursor;
    }


    public override void OnEnter()
    {
        CursorManager.Instance.isAllowedOnScreen = true;
        DemolishObject = scratchPad.Get<Action<GameObject>>("DemolishFunc");
        cursorInd.SetColor("red");
    }

    public override void OnExit()
    {
        CursorManager.Instance.isAllowedOnScreen = false;
        cursorInd.SetColor("white");
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (cursorInd.colliders != null)
            {
                foreach (Collider collider in cursorInd.colliders)
                {
                    DemolishObject?.Invoke(collider.gameObject);
                }
                cursorInd.colliders = null;
            }
        }

        /*        if (raycaster.GetRaycastHit(out hit, buildingLayer))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        GameObject building = hit.collider.gameObject;
                        Debug.Log(building.name);
                        DemolishObject?.Invoke(building);
                    }
                }*/
    }
}
