using System;
using UnityEngine;

public class BuildState : State
{
    // Raycast
    private Raycaster raycaster;
    private RaycastHit hit;
    private Vector3 mouseHitPos;
    private LayerMask groundLayer;

    // Building
    private GameObject phantomObject;
    private bool gridEnabled;
    private Quaternion currentObjectRotation;
    private Action<Vector3, Quaternion> PlaceObject;

    public BuildState(LayerMask _groundLayer)
    {
        raycaster = Raycaster.Instance;
        groundLayer = _groundLayer;
    }

    public override void OnEnter()
    {
        Debug.Log("Builder");
        gridEnabled = scratchPad.Get<bool>("gridEnabled");
        PlaceObject = scratchPad.Get<Action<Vector3, Quaternion>>("PlaceObjectFunc");
        CursorManager.Instance.isAllowedOnScreen = false;
    }

    public override void OnExit()
    {
        phantomObject.SetActive(false);
        phantomObject = null;
        CursorManager.Instance.isAllowedOnScreen = true;
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        GameObject scratchPadPhantom = scratchPad.Get<GameObject>("phantomObject");
        if (scratchPadPhantom != phantomObject)
        {
            phantomObject = scratchPadPhantom;
            currentObjectRotation = phantomObject.transform.rotation;
            phantomObject.SetActive(true);
        }

        if (raycaster.GetRaycastHit(out hit, groundLayer))
        {
            mouseHitPos = hit.point;
            mouseHitPos.y = 0;
            if (!gridEnabled) phantomObject.transform.position = mouseHitPos;
            else phantomObject.transform.position = Grid.ToGridPos(mouseHitPos);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceObject?.Invoke(Grid.ToGridPos(mouseHitPos), currentObjectRotation);
                if (PlaceObject == null) Debug.Log("Method not Found");
            }
        }

        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Rotate");
            currentObjectRotation = phantomObject.RotateYToRight(90);
        }
    }
}
