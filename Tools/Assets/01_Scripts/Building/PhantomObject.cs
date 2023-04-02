using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhantomObject
{
    public GameObject phantom;
    public static sPhantomObjectValues phantomValues;
    public static BuildingCursor cursorIndicator;
    public bool isPlaced;

    private Collider[] objectColliders;
    private Dictionary<MeshRenderer, Material[]> objectMaterials = new Dictionary<MeshRenderer, Material[]>();

    public PhantomObject (GameObject _phantom)
    {
        phantom = _phantom;
        objectColliders = phantom.GetComponentsInChildren<Collider>();
        cursorIndicator.gameObject.SetActive(true);
        EnableColliders(false);
        AssignPhantomMaterial();
    }

    public void AssignPhantomMaterial()
    {
        foreach (MeshRenderer renderer in phantom.GetComponentsInChildren<MeshRenderer>())
        {
            //if (renderer.GetComponent<BuildingCursor>() != null) continue;
            objectMaterials.Add(renderer, renderer.sharedMaterials);
            for (int i = 0; i < renderer.materials.Count(); i++)
            {
                //renderer.materials[i] = phantomValues.greenPhantom;
                renderer.sharedMaterials[i].CopyPropertiesFromMaterial(phantomValues.greenPhantom);
            }
        }
    }

    public void PlaceObject()
    {
        ObtainOldMaterials();
        isPlaced = true;
    }

    public void ObtainOldMaterials()
    {
        foreach (MeshRenderer renderer in phantom.GetComponentsInChildren<MeshRenderer>())
        {
            if (objectMaterials.ContainsKey(renderer))
                renderer.sharedMaterials = objectMaterials[renderer];
        }

        EnableColliders(true);
    }

    public void SetParent(Transform _transform)
    {
        phantom.transform.parent = _transform;
    }

    private void EnableColliders(bool _enabled)
    {
        foreach (Collider collider in objectColliders)
        {
            collider.enabled = _enabled;
        }
    }
}
