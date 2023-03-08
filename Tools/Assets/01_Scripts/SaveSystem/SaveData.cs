using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<PlacedObject> builtObjects;
    public List<Preset> presetCatalogue;

    public SaveData(List<PlacedObject> _placedObjects, List<Preset> _presets) 
    { 
        builtObjects = _placedObjects;
        presetCatalogue = _presets;
    }
}
