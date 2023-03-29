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

    //private Material[] objectMaterials;
    private Dictionary<MeshRenderer, Material[]> objectMaterials = new Dictionary<MeshRenderer, Material[]>();
    //private GameObject phantomParent;

    public PhantomObject (GameObject _phantom)
    {
        phantom = _phantom;
        phantom.GetComponent<Collider>().enabled = false;
        cursorIndicator.gameObject.SetActive(true);
/*        phantom._transform.parent = cursorIndicator._transform;
        phantom._transform.localPosition = Vector3.zero;*/
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

        phantom.GetComponent<Collider>().enabled = true;
    }

    public void SetParent(Transform _transform)
    {
        phantom.transform.parent = _transform;
    }
}
