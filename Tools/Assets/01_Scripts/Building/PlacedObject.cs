using UnityEngine;

[System.Serializable]
public class PlacedObject
{
    public Preset preset;
    public Vector3 buildingPos;

    public PlacedObject(Preset _preset, Vector3 _buildingPosition)
    {
        preset = _preset;
        buildingPos = _buildingPosition;
    }
}
