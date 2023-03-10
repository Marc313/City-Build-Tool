using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<PlacedObject> builtObjects;
    public List<Preset> presetCatalogue;
    public string projectName;

    public SaveData(List<PlacedObject> _placedObjects, List<Preset> _presets) 
    { 
        builtObjects = _placedObjects;
        presetCatalogue = _presets;
    }

    public SaveData(List<PlacedObject> _placedObjects, List<Preset> _presets, string _projectName)
    {
        builtObjects = _placedObjects;
        presetCatalogue = _presets;
        projectName = _projectName;
    }
}
