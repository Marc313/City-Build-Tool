using JetBrains.Annotations;
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

    public bool isNewProject;

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
    }

    private void Start()
    {
        OnStart();
    }

    private void OnStart()
    {
        // Only feed default values when project is made
        PresetCatalogue.SetDefaultPresets(library.presets, isNewProject);

        /*      Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;*/

        mainCam = Camera.main;
        if (PresetCatalogue.presets.Count > 0)
        {
            currentGamePreset = PresetCatalogue.presets[0];
            //currentPrefabID = 1;

            phantomObject = currentGamePreset.LoadInstance();
        }

    }

    private void Update()
    {
        if (phantomObject == null) return;

        if (buildingMode == Mode.Building && !IsMouseOverUI())
        {
            HandleSwitchInput();

            Vector2 mousePos = Input.mousePosition;

            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(mousePos), out hit, 100, buildingLayers))
            {
                Debug.Log(hit.collider.name);
                phantomObject.SetActive(true);
                Vector3 hitPos = hit.point;
                hitPos.y = 0;
                phantomObject.transform.position = hitPos;
                //phantomObject.transform.position = Grid.ToGridPos(hitPos);

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

        if (IsMouseOverUI()) phantomObject.SetActive(false);
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
                placedBuilding = building.preset.LoadInstance(building.buildingPos, Quaternion.identity);
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

    // TODO: Improve, bad performanceCode
    private void HandleSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCurrentPreset(PresetCatalogue.presets[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetCurrentPreset(PresetCatalogue.presets[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetCurrentPreset(PresetCatalogue.presets[2]);
        }
    }

    public void SetCurrentPreset(Preset _preset)
    {
        currentGamePreset = _preset;
        //currentPrefabID = 1;
        if (phantomObject != null)
        {
            phantomObject.SetActive(false);
        }
        phantomObject = currentGamePreset.LoadInstance();
    }

    private void PlaceObject(Vector3 _groundPos)
    {
        allObjects.Add(phantomObject);
        buildings.Add(new PlacedObject(currentGamePreset, phantomObject.transform.position));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = currentGamePreset.LoadInstance(_groundPos, Quaternion.identity, transform);
    }

    public static bool IsMouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
}
