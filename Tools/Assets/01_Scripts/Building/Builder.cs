using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public enum Mode
    {
        None = 0,
        Building = 1,
        Bulldozing = 2
    }

    public GameObject roadPrefab;
    public GameObject housePrefab;
    public GameObject treePrefab;

    public List<GameObject> placedObjects { get; private set; }
    public List<Building> buildings { get; private set; }

    [SerializeField] private LayerMask buildingLayers;

    private int currentPrefabID;
    private Camera mainCam;
    private GameObject currentGamePrefab;
    private GameObject phantomObject;
    private Mode buildingMode = Mode.Building;


    private void Awake()
    {
        placedObjects = new List<GameObject>();
        buildings = new List<Building>();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnStart()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
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
                Vector3 hitPos = hit.point;
                hitPos.y = 0;
                phantomObject.transform.position = Grid.ToGridPos(hitPos);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    PlaceObject(hitPos);
                }
            }
        }
    }

    public void Reconstruct(List<Building> _gameObjects)
    {
        foreach (GameObject placedObject in placedObjects)
        {
            Destroy(placedObject);
        }

        // Build all Buildings
        foreach (Building building in _gameObjects)
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
                placedObjects.Add(placedBuilding);
            }
        }

        // buildings.Intersect(_gameObjects);

        buildings = _gameObjects;
        Debug.Log("Done Reconstructing");
    }

    // TODO: Improve, bad performanceCode
    private void HandleSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            currentGamePrefab = roadPrefab;
            currentPrefabID = 1;
            phantomObject.SetActive(false);
            phantomObject = Instantiate(roadPrefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            currentGamePrefab = housePrefab;
            currentPrefabID = 2;
            phantomObject.SetActive(false);
            phantomObject = Instantiate(housePrefab);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            currentGamePrefab = treePrefab;
            currentPrefabID = 3;
            phantomObject.SetActive(false);
            phantomObject = Instantiate(treePrefab);
        }
    }

    private void PlaceObject(Vector3 _groundPos)
    {
        placedObjects.Add(phantomObject);
        buildings.Add(new Building(currentPrefabID, phantomObject.transform.position));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = Instantiate(currentGamePrefab, _groundPos, Quaternion.identity);
    }
}
