using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<Building> builtObjects;

    public SaveData(List<Building> _placedObjects) 
    { 
        builtObjects = _placedObjects;
    }
}
