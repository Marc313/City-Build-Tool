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

    public GameObject roadPrefab;
    public GameObject housePrefab;
    public GameObject treePrefab;

    public List<GameObject> allObjects { get; private set; }    // Obsolete?
    public List<PlacedObject> buildings { get; private set; }

    [SerializeField] private LayerMask buildingLayers;

    private int currentPrefabID;
    private Camera mainCam;
    private GameObject currentGamePrefab;
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
/*        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;*/
        mainCam = Camera.main;
        currentGamePrefab = roadPrefab;
        currentPrefabID = 1;

        phantomObject = Instantiate(currentGamePrefab);
    }

    private void Update()
    {
        if (buildingMode == Mode.Building)
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
            if (building.buildingId == 1)
            {
                placedBuilding = Instantiate(roadPrefab, building.buildingPos, Quaternion.identity);
            }
            if (building.buildingId == 2)
            {
                placedBuilding = Instantiate(housePrefab, building.buildingPos, Quaternion.identity);
            }
            if (building.buildingId == 3)
            {
                placedBuilding = Instantiate(treePrefab, building.buildingPos, Quaternion.identity);
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
            SetCurrentPreset(PresetManager.presets[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            currentGamePrefab = housePrefab;
            currentPrefabID = 2;
            phantomObject.SetActive(false);
            phantomObject = Instantiate(currentGamePrefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            currentGamePrefab = treePrefab;
            currentPrefabID = 3;
            phantomObject.SetActive(false);
            phantomObject = Instantiate(currentGamePrefab);
        }
    }

    private void SetCurrentPreset(Preset _preset)
    {
        currentGamePrefab = _preset.prefab;
        currentPrefabID = 1;
        phantomObject.SetActive(false);
        phantomObject = Instantiate(currentGamePrefab);
    }

    private void PlaceObject(Vector3 _groundPos)
    {
        allObjects.Add(phantomObject);
        buildings.Add(new PlacedObject(currentPrefabID, phantomObject.transform.position));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = Instantiate(currentGamePrefab, _groundPos, Quaternion.identity, transform);
    }
}
