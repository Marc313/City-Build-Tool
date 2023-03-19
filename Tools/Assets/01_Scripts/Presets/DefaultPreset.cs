using UnityEngine;

[System.Serializable]
public class DefaultPreset : Preset
{
    public DefaultPreset(string _name, Category _category) : base(_name, _category)
    {
    }

    public override GameObject LoadInstance()
    {
        GameObject prefab = PresetCatalogue.GetPrefabByPreset(this);
        if (prefab != null)
        {
            GameObject result = Instantiator.Instantiate(prefab);
            return ApplyScaling(result);
        }
        Debug.Log($"Prefab for Preset {this} could not be found!");
        return null;
    }
}
