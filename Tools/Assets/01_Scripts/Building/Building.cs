using UnityEngine;

[System.Serializable]
public class Building
{
    public int buildingId;
    public Vector3 buildingPos;

    public Building(int _buildingId, Vector3 _buildingPosition)
    {
        buildingId = _buildingId;
        buildingPos = _buildingPosition;
    }
}
