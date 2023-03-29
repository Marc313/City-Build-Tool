using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Material gridMaterial;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            gridMaterial.SetFloat("_CellSize", 5.0f);
        }
    }
}
