using System;
using UnityEngine;

public class EditState : State
{
    private Raycaster raycaster;
    private RaycastHit hit;
    private LayerMask groundLayer;
    private LayerMask buildingLayer;
    private Vector3 mouseHitPos;

    private Func<GameObject, GameObject> Pickup;
    private Action<Vector3, Quaternion> ReplaceObject;

    private GameObject pickedUpObject;
    private bool gridEnabled;

    public EditState(LayerMask _groundLayer, LayerMask _buildingLayer)
    {
        groundLayer = _groundLayer;
        buildingLayer = _buildingLayer;
        raycaster = Raycaster.Instance;
    }

    public override void OnEnter()
    {
        Debug.Log("Edit");
        gridEnabled = scratchPad.Get<bool>("gridEnabled");
        Pickup = scratchPad.Get<Func<GameObject, GameObject>>("EditFunc");
        ReplaceObject = scratchPad.Get<Action<Vector3, Quaternion>>("ReplaceFunc");
    }

    public override void OnExit()
    {
        // Restore original position
        pickedUpObject = null;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        if (pickedUpObject == null)
        {
            if (raycaster.GetRaycastHit(out hit, buildingLayer))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameObject building = hit.collider.gameObject;
                    Debug.Log(building.name);
                    if (Pickup != null)
                    pickedUpObject = Pickup.Invoke(building);
                }
            }
        }
        else if (raycaster.GetRaycastHit(out hit, groundLayer))
        {
            mouseHitPos = hit.point;
            mouseHitPos.y = 0;
            if (!gridEnabled) pickedUpObject.transform.position = mouseHitPos;
            else pickedUpObject.transform.position = Grid.ToGridPos(mouseHitPos);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ReplaceObject?.Invoke(Grid.ToGridPos(mouseHitPos), pickedUpObject.transform.rotation);
                pickedUpObject = null;
                if (ReplaceObject == null) Debug.Log("Method not Found");
            }
        }
    }

}
