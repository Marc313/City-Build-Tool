using System;
using UnityEngine;

public class EditState : State
{
    private Raycaster raycaster;
    private RaycastHit hit;
    private LayerMask groundLayer;
    private LayerMask buildingLayer;
    private Vector3 mouseHitPos;

    private Func<GameObject, PhantomObject> Pickup;
    private Action<Vector3, Quaternion> ReplaceObject;

    private Quaternion currentObjectRotation;
    private PhantomObject pickedUpObject;
    private bool gridEnabled;
    private BuildingCursor cursorInd;

    public EditState(LayerMask _groundLayer, LayerMask _buildingLayer, BuildingCursor _cursor)
    {
        groundLayer = _groundLayer;
        buildingLayer = _buildingLayer;
        raycaster = Raycaster.Instance;
        cursorInd = _cursor;
    }

    public override void OnEnter()
    {
        Debug.Log("Edit");
        gridEnabled = scratchPad.Get<bool>("gridEnabled");
        Pickup = scratchPad.Get<Func<GameObject, PhantomObject>>("EditFunc");
        ReplaceObject = scratchPad.Get<Action<Vector3, Quaternion>>("ReplaceFunc");
        cursorInd.SetColor("blue");
    }

    public override void OnExit()
    {
        // Restore original position
        pickedUpObject = null;
        cursorInd.SetColor("white");
    }

    public override void OnFixedUpdate()
    {
    }

    // More inheritence! All states now alike
    public override void OnUpdate()
    {
        raycaster.GetRaycastHit(out hit, groundLayer);
        mouseHitPos = hit.point;
        mouseHitPos.y = 0;

        if (!gridEnabled) cursorInd.transform.position = mouseHitPos;
        else cursorInd.transform.position = Grid.ToGridPos(mouseHitPos);

        if (pickedUpObject == null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Collider[] cursorColliders = cursorInd.colliders;
                if (cursorColliders != null && cursorColliders.Length > 0)
                {
                    pickedUpObject = Pickup?.Invoke(cursorColliders[0].gameObject);
                }
            }
        }
        else if (raycaster.GetRaycastHit(out hit, groundLayer))
        {

            if (!gridEnabled) pickedUpObject.phantom.transform.position = mouseHitPos;
            else pickedUpObject.phantom.transform.position = Grid.ToGridPos(mouseHitPos);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ReplaceObject?.Invoke(Grid.ToGridPos(mouseHitPos), pickedUpObject.phantom.transform.rotation);
                pickedUpObject = null;
                if (ReplaceObject == null) Debug.Log("Method not Found");
            }
        }

        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        if (pickedUpObject != null && Input.GetKeyDown(KeyCode.Mouse1))
        {
            currentObjectRotation = pickedUpObject.phantom.RotateYToRight(90);
            cursorInd.gameObject.RotateYToRight(90);
        }
    }

}
