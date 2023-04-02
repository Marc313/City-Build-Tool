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
    private PhantomObject phantomObject;
    private bool gridEnabled;
    private Quaternion currentObjectRotation;
    private Action<Vector3, Quaternion> PlaceObject;
    private BuildingCursor cursorInd;
    private Vector3 previousCursorPos;

    public BuildState(LayerMask _groundLayer, BuildingCursor _cursor)
    {
        raycaster = Raycaster.Instance;
        groundLayer = _groundLayer;
        cursorInd = _cursor;
    }

    public override void OnEnter()
    {
        gridEnabled = scratchPad.Get<bool>("gridEnabled");
        PlaceObject = scratchPad.Get<Action<Vector3, Quaternion>>("PlaceObjectFunc");
        CursorManager.Instance.isAllowedOnScreen = false;
        cursorInd.EnableColoring();
    }

    public override void OnExit()
    {
        phantomObject.phantom.SetActive(false);
        phantomObject = null;
        CursorManager.Instance.isAllowedOnScreen = true;
        cursorInd.DisableColoring();
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        PhantomObject scratchPadPhantom = scratchPad.Get<PhantomObject>("phantomObject");
        Preset.Category category = scratchPad.Get<Preset>("currentGamePreset").category;
        if (scratchPadPhantom != phantomObject)
        {
            if (phantomObject != null && !phantomObject.isPlaced) phantomObject.phantom.SetActive(false);

            phantomObject = scratchPadPhantom;
            currentObjectRotation = phantomObject.phantom.transform.rotation;
            phantomObject.phantom.SetActive(true);
        }
        else if (scratchPadPhantom == null && phantomObject == null)
        {
            Preset currentGamePreset = scratchPad.Get<Preset>("currentGamePreset");
            phantomObject = new PhantomObject(currentGamePreset.LoadInstance());
            scratchPad.RegisterOrUpdate("phantomObject", phantomObject);
        }

        if (phantomObject == null) return;

        if (raycaster.GetRaycastHit(out hit, groundLayer))
        {
            // Set Cursor & Phantom to mouse pos
            mouseHitPos = hit.point;
            mouseHitPos.y = 0;
            if (!gridEnabled)
            {
                //cursorInd.transform.position = mouseHitPos;
                phantomObject.phantom.transform.position = mouseHitPos;
            }
            else
            {
                //cursorInd.transform.position = Grid.ToGridPos(mouseHitPos);
                phantomObject.phantom.transform.position = Grid.ToGridPos(mouseHitPos);
            }

            // Click Event
            if (!CursorManager.IsMouseOverUI() && category == Preset.Category.Road && Input.GetKey(KeyCode.Mouse0))
            {
                // Extra safety
                if (phantomObject.phantom.transform.position != previousCursorPos)
                {
                    previousCursorPos = phantomObject.phantom.transform.position;
                    PlacePhantom();
                }
            }
            else if (!CursorManager.IsMouseOverUI() && Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlacePhantom();
            }
        }

        HandleRotationInput();
        if (CursorManager.IsMouseOverUI() && phantomObject != null) phantomObject.phantom.SetActive(false);
        else if (phantomObject != null) phantomObject.phantom.SetActive(true);
    }

    private void PlacePhantom()
    {
        PlaceObject?.Invoke(Grid.ToGridPos(mouseHitPos), currentObjectRotation);
        if (PlaceObject == null) Debug.Log("Method not Found");
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Rotate");
            currentObjectRotation = phantomObject.phantom.RotateYToRight(90);
        }
    }
}
