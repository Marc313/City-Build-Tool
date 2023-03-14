using UnityEngine;

[System.Serializable]
public class PlacedObject
{
    public Preset preset;
    public Vector3 buildingPos;
    public Quaternion rotation;

    public PlacedObject(Preset _preset, Vector3 _buildingPosition, Quaternion _rotation)
    {
        preset = _preset;
        buildingPos = _buildingPosition;
        rotation = _rotation;
    }
}
