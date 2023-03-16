using System;
using UnityEngine;

public class BuildState : State
{
    private Raycaster raycaster;
    private RaycastHit hit;
    private LayerMask groundLayer;
    private Vector3 mouseHitPos;
    private GameObject phantomObject;
    private bool gridEnabled;
    private Action<Vector3> PlaceObject; 

    public BuildState(LayerMask _groundLayer)
    {
        raycaster = Raycaster.Instance;
        groundLayer = _groundLayer;
    }

    public override void OnEnter()
    {
        Debug.Log("Builder");
        gridEnabled = scratchPad.Get<bool>("gridEnabled");
        PlaceObject = scratchPad.Get<Action<Vector3>>("PlaceObjectFunc");
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
        phantomObject = scratchPad.Get<GameObject>("phantomObject");

        if (raycaster.GetRaycastHit(out hit, groundLayer))
        {
            phantomObject.SetActive(true);
            mouseHitPos = hit.point;
            mouseHitPos.y = 0;
            if (!gridEnabled) phantomObject.transform.position = mouseHitPos;
            else phantomObject.transform.position = Grid.ToGridPos(mouseHitPos);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceObject?.Invoke(Grid.ToGridPos(mouseHitPos));
                if (PlaceObject == null) Debug.Log("Method not Found");
            }
        }

    }
}
