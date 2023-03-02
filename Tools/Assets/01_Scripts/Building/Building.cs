using UnityEngine;

[System.Serializable]
public class PlacedObject
{
    public int buildingId;
    public Vector3 buildingPos;

    public PlacedObject(int _buildingId, Vector3 _buildingPosition)
    {
        buildingId = _buildingId;
        buildingPos = _buildingPosition;
    }
}
