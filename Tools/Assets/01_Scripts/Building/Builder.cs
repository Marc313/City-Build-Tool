using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public GameObject roadPrefab;
    public GameObject housePrefab;
    public GameObject treePrefab;

    [SerializeField] private LayerMask buildingLayers;

    private Camera mainCam;
    private GameObject currentGamePrefab;
    private GameObject phantomObject;

    private void Start()
    {
        OnStart();
    }

    private void OnStart()
    {
        mainCam = Camera.main;
        currentGamePrefab = roadPrefab;

        phantomObject = Instantiate(currentGamePrefab);
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        RaycastHit hit;
        if (Physics.Raycast(mainCam.ScreenPointToRay(mousePos), out hit, buildingLayers))
        {
            Vector3 mouseCords = hit.point;
            phantomObject.transform.position = mouseCords;
        }
    }
}
