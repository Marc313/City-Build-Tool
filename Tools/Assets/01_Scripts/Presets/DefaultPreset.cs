using UnityEngine;

[System.Serializable]
public class DefaultPreset : Preset
{
    public DefaultPreset(string _name, Category _category) : base(_name, _category)
    {
    }

    public override GameObject LoadInstance()
    {
        throw new System.NotImplementedException();
    }
}
