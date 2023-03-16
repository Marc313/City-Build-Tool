using MarcoHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Builder : MonoBehaviour, IFSMOwner
{
    public ScratchPad sharedData { get; private set; } = new ScratchPad();

    public PresetLibrary library;
    public bool isGridEnabled;

    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask buildingLayers;

    private Dictionary<GameObject, PlacedObject> allObjects;
    private Preset currentGamePreset;
    private GameObject phantomObject;
    private BuildingModeFSM fsm;
    [HideInInspector] public Vector3 mouseHitPos;


    private void Awake()
    {
        fsm = new BuildingModeFSM(this, groundLayers, buildingLayers);
        allObjects = new Dictionary<GameObject, PlacedObject>();
    }

    private void Start()
    {
        OnStart();
    }

/*    private void OnEnable()
    {
        EventSystem.Subscribe(EventName.MENU_OPENED, DisableSelf);
        EventSystem.Subscribe(EventName.MENU_CLOSED, EnableSelf);
    }

    private void OnDisable()
    {
        EventSystem.Unsubscribe(EventName.MENU_OPENED, DisableSelf);
        EventSystem.Unsubscribe(EventName.MENU_CLOSED, EnableSelf);
    }*/

    private void OnStart()
    {
        PresetCatalogue.SetDefaultPresets(library.presets, true);

        if (PresetCatalogue.allPresets.Count > 0)
        {
            currentGamePreset = PresetCatalogue.allPresets[0];
            phantomObject = currentGamePreset.LoadInstance();
        }
        WriteScratchPadStartValues();
        phantomObject.SetActive(false);
        fsm?.Start();
    }

    private void Update()
    {
        if (UIManager.Instance.isMenuOpen) return;

        fsm?.OnUpdate();
        HandleSwitchInput();
    }

    private void FixedUpdate()
    {
        fsm?.OnFixedUpdate();
    }

    // TODO: Beter
    public void Reconstruct(List<PlacedObject> _gameObjects)
    {
        foreach (GameObject placedObject in allObjects.Keys)
        {
            Destroy(placedObject);
        }

        allObjects = new Dictionary<GameObject, PlacedObject>();

        // Build all Buildings
        foreach (PlacedObject building in _gameObjects)
        {
            GameObject placedBuilding = null;
            if (building.preset != null)
            {
                placedBuilding = building.preset.LoadInstance(building.buildingPos, transform);
                placedBuilding.transform.rotation = building.rotation;
            }

            if (placedBuilding != null)
            {
                allObjects.Add(placedBuilding, building);
            }
        }

        Debug.Log("Done Reconstructing");
    }

    public void SetCurrentPreset(Preset _preset)
    {
        currentGamePreset = _preset;
        if (phantomObject != null)
        {
            phantomObject.SetActive(false);
        }

        if (fsm != null
            && fsm.GetCurrentState().GetType() != typeof(BuildState)) {
            fsm.SwitchState(typeof(BuildState));
            FindObjectOfType<BuildModeTab>().SetState<BuildState>();
        }

        phantomObject = currentGamePreset.LoadInstance();
        phantomObject.layer = 7;
        sharedData.RegisterOrUpdate("currentGamePreset", currentGamePreset);
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
    }

    public List<PlacedObject> GetPlacedObjectList()
    {
        return allObjects.Values.ToList();
    }

    public void SwitchState(Type _stateType)
    {
        if (fsm == null) return;

        State currentState = fsm.GetCurrentState();
        if (_stateType != currentState.GetType())
        {
            fsm?.SwitchState(_stateType);
        }

        if (phantomObject != null) phantomObject.SetActive(false);
    }

    private void WriteScratchPadStartValues()
    {
        // Fields
        sharedData.RegisterOrUpdate("gridEnabled", isGridEnabled);
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
        sharedData.RegisterOrUpdate("currentGamePreset", currentGamePreset);

        // Actions
        sharedData.RegisterOrUpdate("PlaceObjectFunc", (Action<Vector3, Quaternion>)((Vector3 v, Quaternion q) => PlaceObject(v, q)));
        sharedData.RegisterOrUpdate("ReplaceFunc", (Action<Vector3, Quaternion>)((Vector3 v, Quaternion q) => ReplaceObject(v, q)));
        sharedData.RegisterOrUpdate("EditFunc", (Func<GameObject, GameObject>)((GameObject go) => EditObject(go)));
        sharedData.RegisterOrUpdate("DemolishFunc", (Action<GameObject>)((GameObject go) => DemolishObject(go)));
    }

/*    private void EnableSelf(object _value = null)
    {
        isEnabled = true;
    }

    private void DisableSelf(object _value = null)
    {
        isEnabled = false;
    }*/

    private void HandleSwitchInput()
    {
/*        State currentState = fsm.GetCurrentState();
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
        }*/
    }

    private void PlaceObject(Vector3 _groundPos, Quaternion _currentRotation)
    {
        allObjects.Add(phantomObject, new PlacedObject(currentGamePreset, _groundPos, _currentRotation));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = currentGamePreset.LoadInstance(_groundPos, transform);
        phantomObject.transform.rotation = _currentRotation;
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
    }

    private void ReplaceObject(Vector3 _groundPos, Quaternion _currentRotation)
    {
        allObjects.Add(phantomObject, new PlacedObject(currentGamePreset, _groundPos, _currentRotation));
        phantomObject = null;
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
    }

    private GameObject EditObject(GameObject _gameObject)
    {
        GameObject fullObject = _gameObject;
        if (_gameObject.transform.parent != null
            && _gameObject.transform.parent.gameObject.layer == _gameObject.layer)
        {
            fullObject = gameObject.transform.parent.gameObject;
        }

        if (allObjects.ContainsKey(fullObject))
        {
            currentGamePreset = allObjects[fullObject].preset;
            phantomObject = fullObject;
            sharedData.RegisterOrUpdate("phantomObject", phantomObject);
            allObjects.Remove(fullObject);
            return fullObject;
        }
        return null;
    }

    private void DemolishObject(GameObject _gameObject)
    {
        GameObject fullObject = _gameObject;
        if (_gameObject.transform.parent != null 
            && _gameObject.transform.parent.gameObject.layer == _gameObject.layer)
        {
            fullObject = _gameObject.transform.parent.gameObject;
        }

        if (allObjects.ContainsKey(fullObject))
        {
            allObjects.Remove(fullObject);
        }
        Destroy(fullObject);
    }
}
