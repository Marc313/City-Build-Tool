using UnityEngine;

public static class Grid
{
    public static float cellSize = 1f;

    public static Vector3 ToGridPos(Vector3 _pos)
    {
        _pos.x = ToCellSize(_pos.x);
        _pos.y = 0;
        _pos.z = ToCellSize(_pos.z);
        return _pos;
    }

    private static float ToCellSize(float _value)
    {
        return Mathf.Floor(_value / cellSize) * cellSize;
    }
}
