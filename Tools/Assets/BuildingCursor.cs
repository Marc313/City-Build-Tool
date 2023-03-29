using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingCursor : MonoBehaviour
{
    private GameObject child;
    private MeshRenderer childRenderer;
    private BoxCollider collider;
    private bool isEnabled;

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private LayerMask buildingLayer;

    private void Awake()
    {
        child = transform.GetChild(0).gameObject;
        childRenderer = child.GetComponent<MeshRenderer>();
        collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        childRenderer.material = normalMaterial;
    }

    public void SetScale(Preset _preset)
    {
        Vector2 xzScale = _preset.XZSizeUnits;
        transform.localScale = new Vector3(xzScale.x, 1, xzScale.y);
    }

    private void Update()
    {
        if (isEnabled && CursorManager.IsMouseOverUI())
        {
            isEnabled = false;
            child.SetActive(false);
        }


        else if (!isEnabled && !CursorManager.IsMouseOverUI())
        {
            isEnabled = true;
            child.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (buildingLayer == (buildingLayer | 1 << _other.gameObject.layer))
        {
            Debug.Log(_other.name);
            childRenderer.material = redMaterial;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (buildingLayer == (buildingLayer | 1 << _other.gameObject.layer))
        {
            Debug.Log(_other.name);
            Collider[] colliders = Physics.OverlapBox(collider.center, collider.bounds.size / 2, Quaternion.identity, buildingLayer);
            if (colliders.Length <= 0)
            {
                childRenderer.material = normalMaterial;
            }
        }
    }
}
