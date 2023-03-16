using MarcoHelpers;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    // Switch to FSM
    public enum Mode
    {
        None = 0,
        Building = 1,
        Editing = 2,
        Bulldozing = 3
    }

    public PresetLibrary library;

    public List<GameObject> allObjects { get; private set; }    // Obsolete?
    public List<PlacedObject> buildings { get; private set; }

    public bool isGridEnabled;

    [SerializeField] private LayerMask buildingLayers;
    [SerializeField] private LayerMask objectLayers;

    private Camera mainCam;
    private Preset currentGamePreset;
    private GameObject phantomObject;
    private Quaternion currentObjectRotation;
    private Mode buildingMode = Mode.Building;
    private bool isEnabled = true;
    private BuildingModeFSM fsm;
    [HideInInspector] public Vector3 mouseHitPos;


    private void Awake()
    {
        fsm = new BuildingModeFSM();
        allObjects = new List<GameObject>();
        buildings = new List<PlacedObject>();
        mainCam = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        PresetCatalogue.SetDefaultPresets(library.presets, true);
        OnStart();
    }

    private void OnEnable()
    {
        EventSystem.Subscribe(EventName.MENU_OPENED, DisableSelf);
        EventSystem.Subscribe(EventName.MENU_CLOSED, EnableSelf);
    }

    private void OnDisable()
    {
        EventSystem.Unsubscribe(EventName.MENU_OPENED, DisableSelf);
        EventSystem.Unsubscribe(EventName.MENU_CLOSED, EnableSelf);
    }

    private void OnStart()
    {

        /*      Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;*/

        if (PresetCatalogue.allPresets.Count > 0)
        {
            currentGamePreset = PresetCatalogue.allPresets[0];
            //currentPrefabID = 1;

            phantomObject = currentGamePreset.LoadInstance();
        }

    }

    private void Update()
    {
        if (phantomObject == null || !isEnabled) return;

        fsm?.OnUpdate();
        HandleSwitchInput();

        if (buildingMode == Mode.Building && !CursorManager.IsMouseOverUI())
        {
            Vector2 mousePos = Input.mousePosition;

            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(mousePos), out hit, 100, buildingLayers))
            {
                phantomObject.SetActive(true);
                mouseHitPos = hit.point;
                mouseHitPos.y = 0;
                if (!isGridEnabled) phantomObject.transform.position = mouseHitPos;
                else phantomObject.transform.position = Grid.ToGridPos(mouseHitPos);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PlaceObject(mouseHitPos);
                }
            }
            else
            {
                phantomObject.SetActive(false);
            }
        }

        else if (buildingMode == Mode.Editing)
        {
            Vector2 mousePos = Input.mousePosition;
            RaycastHit hit;

            if (Physics.Raycast(mainCam.ScreenPointToRay(mousePos), out hit, 100, objectLayers))
            {

            }
        }

        if (CursorManager.IsMouseOverUI()) phantomObject.SetActive(false);

        HandleRotationInput();
    }

    private void FixedUpdate()
    {
        fsm?.OnFixedUpdate();
    }

    public void Reconstruct(List<PlacedObject> _gameObjects)
    {
        foreach (GameObject placedObject in allObjects)
        {
            Destroy(placedObject);
        }

        // Build all Buildings
        foreach (PlacedObject building in _gameObjects)
        {
            GameObject placedBuilding = null;
            if (building.preset != null)
            {
                placedBuilding = building.preset.LoadInstance(building.buildingPos, transform);
                /*if(building.rotation != default) */placedBuilding.transform.rotation = building.rotation;
            }

            if (placedBuilding != null)
            {
                allObjects.Add(placedBuilding);
            }
        }

        // buildings.Intersect(_gameObjects);

        buildings = _gameObjects;
        Debug.Log("Done Reconstructing");
    }

    private void EnableSelf(object _value = null)
    {
        isEnabled = true;
    }

    private void DisableSelf(object _value = null)
    {
        isEnabled = false;
    }

    private void HandleSwitchInput()
    {
        State currentState = fsm.GetCurrentState();
        if (currentState == null)
        {
            Debug.Log("CurrentState not found");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) 
            && fsm != null
            && currentState.GetType() != typeof(BuildState))
        {
            fsm.SwitchState(typeof(BuildState));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3)
            && fsm != null
            && currentState.GetType() != typeof(DemolishState)) 
        {
            fsm.SwitchState(typeof(DemolishState));
        }
    }

    public void SetCurrentPreset(Preset _preset)
    {
        currentGamePreset = _preset;
        if (phantomObject != null)
        {
            phantomObject.SetActive(false);
        }
        phantomObject = currentGamePreset.LoadInstance();
        currentObjectRotation = phantomObject.transform.rotation;
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Rotate");
            currentObjectRotation = phantomObject.RotateYToRight(90);
        }
    }

    private void PlaceObject(Vector3 _groundPos)
    {
        allObjects.Add(phantomObject);
        buildings.Add(new PlacedObject(currentGamePreset, phantomObject.transform.position, currentObjectRotation));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = currentGamePreset.LoadInstance(_groundPos, transform);
        phantomObject.transform.rotation = currentObjectRotation;
    }
}
