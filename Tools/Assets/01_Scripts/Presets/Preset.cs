using System;
using UnityEngine;

[System.Serializable]
public abstract class Preset
{
    [HideInInspector] public int presetID;

    public string presetName;
    public Category category = Category.Decoration;

    [Header("Scaling")]
    public Vector2 XZSizeUnits = new Vector2(1, 1);

    private Quaternion defaultRotation;

    public Preset(string _name, Category _category)
    {
        presetName = _name;
        category = _category;
        presetID = PresetCatalogue.GetFirstAvailableID();

        UIManager.Instance.AddPresetButton(this);
    }

    public override bool Equals(object obj)
    {
        Preset other = obj as Preset;
        if (this != null && other != null
            && (this.presetName == other.presetName))
        {
            return true;
        }

        return false;
    }

    public enum Category
    {
        None = 0,
        Road = 1,
        House = 2,
        Decoration = 3
    }

    /// <summary>
    /// Loads and instantiates the model corresponding to this preset
    /// </summary>
    /// <returns> Returns an phantom of the GameObject containing the loaded preset model </returns>
    public abstract GameObject LoadInstance();
    /// <summary>
    /// Loads and instantiates the model corresponding to this preset
    /// </summary>
    /// <param presetNameField="_position">Position applied to the phantom's transform</param>
    /// <param presetNameField="_rotation">Rotation applied to the phantom's transform</param>
    /// <returns> Returns an phantom of the GameObject containing the loaded preset model </returns>
    public GameObject LoadInstance(Vector3 _position)
    {
        GameObject instance = LoadInstance();
        instance.transform.position = _position;
        //phantom.transform.rotation = defaultRotation;
        return instance;
    }
    
    public GameObject LoadInstance(Transform _parent)
    {
        GameObject instance = LoadInstance();
        instance.transform.rotation = defaultRotation;
        instance.transform.parent = _parent;
        return instance;
    }

    public GameObject LoadInstance(Vector3 _position, Transform _parent)
    {
        GameObject instance = LoadInstance(_position);
        instance.transform.parent = _parent;
        return instance;
    }

    // If v2 = (3, 2), the bounds of _object should fit inside x = 3 and y = 2.
    protected GameObject ApplyScaling(GameObject _object)
    {
        MeshRenderer objectRenderer = _object.GetComponentInChildren<MeshRenderer>();

        Vector3 objectBounds = objectRenderer.bounds.size;

        float xRatio = XZSizeUnits.x / objectBounds.x;
        float zRatio = XZSizeUnits.y / objectBounds.z;

        float smallerRatio = xRatio < zRatio ? xRatio : zRatio;
        _object.transform.localScale = _object.transform.lossyScale * smallerRatio;


        BoxCollider collider = _object.AddComponent<BoxCollider>();
        collider.size = new Vector3(XZSizeUnits.x / _object.transform.localScale.x, 1, XZSizeUnits.y / _object.transform.localScale.z) * 0.95f;
        collider.center = new Vector3(0, 0.5f, 0);

        _object.gameObject.layer = LayerMask.NameToLayer("Building");
        objectRenderer.gameObject.layer = LayerMask.NameToLayer("Building");

        return _object;
    }
}
