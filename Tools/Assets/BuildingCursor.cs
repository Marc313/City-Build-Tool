using System;
using UnityEngine;

public class BuildingCursor : MonoBehaviour
{
    [HideInInspector] public bool isInCollision => colliders != null && colliders.Length > 0;
    [HideInInspector] public Collider[] colliders;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private float collisionSizeModifier = 0.85f;

    private GameObject child;
    private MeshRenderer childRenderer;
    private bool isEnabled;
    private RaycastHit hit;
    private Vector3 mouseHitPos;
    private bool coloring;

    private void Awake()
    {
        child = transform.GetChild(0).gameObject;
        childRenderer = child.GetComponent<MeshRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider>();
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

    public void ResetScale()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
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

        if (Raycaster.Instance.GetRaycastHit(out hit, groundLayer))
        {
            mouseHitPos = hit.point;
            mouseHitPos.y = 0;
            transform.position = Grid.ToGridPos(mouseHitPos);
        }

        //CheckCollisions();
    }

    public void EnableColoring()
    {
        coloring= true;
    }

    public void DisableColoring()
    {
        childRenderer.material = normalMaterial;
        coloring = false;
        ResetScale();
    }

    public void SetColor(string _color)
    {
        switch(_color.ToLower())
        {
            case "blue":
                childRenderer.material = blueMaterial;
                break;
            case "red":
                childRenderer.material = redMaterial;
                break;
            case "white":
            default:
                childRenderer.material = normalMaterial;
                break;
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (buildingLayer == (buildingLayer | 1 << _other.gameObject.layer))
        {
            //Debug.Log(_other.name);
            CheckCollisions();
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (buildingLayer == (buildingLayer | 1 << _other.gameObject.layer))
        {
            //Debug.Log(_other.name);
            CheckCollisions();
        }
    }

    private void CheckCollisions()
    {
        colliders = Physics.OverlapBox(boxCollider.center + transform.position, boxCollider.bounds.size * collisionSizeModifier, Quaternion.identity, buildingLayer);
        if (colliders.Length <= 0)
        {
            if (coloring)
            childRenderer.material = normalMaterial;
        }
        else
        {
/*            foreach (Collider collider in colliders)
            {
                Debug.Log($"Collider: {collider.gameObject.name}");
            }*/
            if (coloring) childRenderer.material = redMaterial;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.center + transform.position, boxCollider.bounds.size * collisionSizeModifier);
    }
}
