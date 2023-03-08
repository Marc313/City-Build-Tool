using UnityEngine;

[System.Serializable]
public abstract class Preset
{
    public int presetID;
    public string presetName;
    public Category category = Category.Decoration;

    public Preset(string _name, Category _category)
    {
        presetName = _name;
        category = _category;
        presetID = PresetCatalogue.presets.Count;

        UIManager.Instance.AddPresetButton(this);
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
    /// <returns> Returns an instance of the GameObject containing the loaded preset model </returns>
    public abstract GameObject LoadInstance();
    /// <summary>
    /// Loads and instantiates the model corresponding to this preset
    /// </summary>
    /// <param presetName="_position">Position applied to the instance's transform</param>
    /// <param presetName="_rotation">Rotation applied to the instance's transform</param>
    /// <returns> Returns an instance of the GameObject containing the loaded preset model </returns>
    public GameObject LoadInstance(Vector3 _position, Quaternion _rotation)
    {
        GameObject instance = LoadInstance();
        instance.transform.position = _position;
        instance.transform.rotation = _rotation;
        return instance;
    }
    
    public GameObject LoadInstance(Transform _parent)
    {
        GameObject instance = LoadInstance();
        instance.transform.parent = _parent;
        return instance;
    }

    public GameObject LoadInstance(Vector3 _position, Quaternion _rotation, Transform _parent)
    {
        GameObject instance = LoadInstance(_position, _rotation);
        instance.transform.parent = _parent;
        return instance;
    }
}
