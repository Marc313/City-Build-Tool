using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private Mode buildingMode = Mode.Building;


    private void Awake()
    {
        allObjects = new List<GameObject>();
        buildings = new List<PlacedObject>();

        // Only feed default values when project is made
        PresetCatalogue.SetDefaultPresets(library.presets, true);
    }

    private void Start()
    {
        OnStart();
    }

    private void OnStart()
    {

        /*      Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;*/

        mainCam = Camera.main;
        if (PresetCatalogue.allPresets.Count > 0)
        {
            currentGamePreset = PresetCatalogue.allPresets[0];
            //currentPrefabID = 1;

            phantomObject = currentGamePreset.LoadInstance();
        }

    }

    private void Update()
    {
        if (phantomObject == null) return;

        if (buildingMode == Mode.Building && !CursorManager.IsMouseOverUI())
        {
            HandleSwitchInput();

            Vector2 mousePos = Input.mousePosition;

            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(mousePos), out hit, 100, buildingLayers))
            {
                phantomObject.SetActive(true);
                Vector3 hitPos = hit.point;
                hitPos.y = 0;
                if (!isGridEnabled) phantomObject.transform.position = hitPos;
                else phantomObject.transform.position = Grid.ToGridPos(hitPos);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PlaceObject(hitPos);
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
                placedBuilding = building.preset.LoadInstance(building.buildingPos);
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

    private void HandleSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCurrentPreset(PresetCatalogue.allPresets[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetCurrentPreset(PresetCatalogue.allPresets[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetCurrentPreset(PresetCatalogue.allPresets[2]);
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
    }

    private void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Rotate");
            phantomObject.RotateYToRight(90);
        }
    }

    private void PlaceObject(Vector3 _groundPos)
    {
        allObjects.Add(phantomObject);
        buildings.Add(new PlacedObject(currentGamePreset, phantomObject.transform.position));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = currentGamePreset.LoadInstance(_groundPos, transform);
    }
}
