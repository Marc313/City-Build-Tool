using UnityEngine;

public class Raycaster : Singleton<Raycaster>
{
    public float raycastRange = 50f;
    private Camera mainCam;

    private void Awake()
    {
        Instance = this;
        mainCam= GetComponent<Camera>();
    }

    public bool GetRaycastHit(out RaycastHit _hit, LayerMask _layers)
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, raycastRange, _layers))
        {
            return true;
        }

        return false;
    }
}
