using UnityEngine;

[System.Serializable]
public class PlacedObject
{
    public int presetId;
    public Vector3 buildingPos;

    public PlacedObject(int _presetID, Vector3 _buildingPosition)
    {
        presetId = _presetID;
        buildingPos = _buildingPosition;
    }
}
