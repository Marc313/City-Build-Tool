using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Builder : MonoBehaviour, IFSMOwner
{
    public ScratchPad sharedData { get; private set; } = new ScratchPad();

    public PresetLibrary library;
    public bool isGridEnabled;
    public sPhantomObjectValues phantomValues;
    public BuildingCursor cursorIndicator;

    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask buildingLayers;

    private Dictionary<GameObject, PlacedObject> allObjects = new Dictionary<GameObject, PlacedObject>();
    private Preset currentGamePreset;
    private PhantomObject phantomObject;
    private BuildingModeFSM fsm;
    [HideInInspector] public Vector3 mouseHitPos;

    private bool hasStarted;

    private void Awake()
    {
        fsm = new BuildingModeFSM(this, groundLayers, buildingLayers, cursorIndicator);
    }

    private void Start()
    {
        PhantomObject.cursorIndicator = cursorIndicator;
        PhantomObject.phantomValues = phantomValues;
        OnStart();
    }

    public void OnStart()
    {
        if (hasStarted) return;

        hasStarted= true;
        PresetCatalogue.SetDefaultPresets(library.presets, true);

        if (PresetCatalogue.allPresets.Count > 0)
        {
            SetCurrentPreset(PresetCatalogue.allPresets[0]);
        }
        WriteScratchPadStartValues();
        fsm?.Start();
    }

    private void Update()
    {
        if (UIManager.Instance.isMenuOpen) return;

        fsm?.OnUpdate();
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
        cursorIndicator.SetScale(_preset);
        if (phantomObject != null)
        {
            phantomObject.phantom.SetActive(false);
        }

        if (fsm != null && fsm.HasStates()
            && fsm.GetCurrentState()?.GetType() != typeof(BuildState)) {
            fsm.SwitchState(typeof(BuildState));
            FindObjectOfType<BuildModeTab>().SetState<BuildState>();
        }

        phantomObject = new PhantomObject(currentGamePreset.LoadInstance());
        phantomObject.phantom.layer = 7;
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

        if (phantomObject != null) phantomObject.phantom.SetActive(false);
    }

    private void WriteScratchPadStartValues()
    {
        // Fields
        sharedData.RegisterOrUpdate("gridEnabled", isGridEnabled);
        //sharedData.RegisterOrUpdate("phantomObject", phantomObject.phantom);
        sharedData.RegisterOrUpdate("currentGamePreset", currentGamePreset);

        // Actions
        sharedData.RegisterOrUpdate("PlaceObjectFunc", (Action<Vector3, Quaternion>)((Vector3 v, Quaternion q) => PlaceObject(v, q)));
        sharedData.RegisterOrUpdate("ReplaceFunc", (Action<Vector3, Quaternion>)((Vector3 v, Quaternion q) => ReplaceObject(v, q)));
        sharedData.RegisterOrUpdate("EditFunc", (Func<GameObject, PhantomObject>)((GameObject go) => EditObject(go)));
        sharedData.RegisterOrUpdate("DemolishFunc", (Action<GameObject>)((GameObject go) => DemolishObject(go)));
    }

    private void PlaceObject(Vector3 _groundPos, Quaternion _currentRotation)
    {
        if (cursorIndicator.isInCollision)
        {
            // Play error sound
            //Logger.Log("An object is already placed here!");
            return;
        }

        if (phantomObject == null) phantomObject = new PhantomObject(sharedData.Get<GameObject>("phantomObject"));
        phantomObject.PlaceObject();
        phantomObject.SetParent(transform);
        allObjects.Add(phantomObject.phantom, new PlacedObject(currentGamePreset, _groundPos, _currentRotation));

        // Overwrite phantomObject so the old phantom will stay in place
        phantomObject = new PhantomObject(currentGamePreset.LoadInstance(_groundPos));
        phantomObject.phantom.transform.rotation = _currentRotation;
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
    }

    private void ReplaceObject(Vector3 _groundPos, Quaternion _currentRotation)
    {
        phantomObject.PlaceObject();
        allObjects.Add(phantomObject.phantom, new PlacedObject(currentGamePreset, _groundPos, _currentRotation));
        phantomObject = null;
        sharedData.RegisterOrUpdate("phantomObject", phantomObject);
    }

    private PhantomObject EditObject(GameObject _gameObject)
    {
        GameObject fullObject = _gameObject;
        if (_gameObject.transform.parent != null
            && _gameObject.transform.parent.gameObject.layer == _gameObject.layer)
        {
            fullObject = _gameObject.transform.parent.gameObject;
        }

        if (allObjects.ContainsKey(fullObject))
        {
            currentGamePreset = allObjects[fullObject].preset;
            phantomObject = new PhantomObject(fullObject);
            sharedData.RegisterOrUpdate("phantomObject", phantomObject);
            allObjects.Remove(fullObject);
            fullObject.transform.parent = null;
            return new PhantomObject(fullObject);
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
