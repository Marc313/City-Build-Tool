using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<PlacedObject> builtObjects;
    public List<Preset> presetCatalogue;

    public SaveData(List<PlacedObject> _placedObjects) 
    { 
        builtObjects = _placedObjects;
    }
}
