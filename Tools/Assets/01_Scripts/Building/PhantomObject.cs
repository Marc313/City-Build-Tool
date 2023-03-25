using System.Linq;
using UnityEngine;

public class PhantomObject
{
    public GameObject phantom;
    public static Material phantomMaterial;

    private Material[] objectMaterials;

    public PhantomObject (GameObject _phantom)
    {
        phantom = _phantom;
        AssignPhantomMaterial();
    }

    public void AssignPhantomMaterial()
    {
        foreach (MeshRenderer renderer in phantom.GetComponentsInChildren<MeshRenderer>())
        {
            for (int i = 0; i < renderer.materials.Count(); i++)
            {
                renderer.sharedMaterials[i].CopyPropertiesFromMaterial(phantomMaterial);
            }
/*            foreach (Material material in renderer.materials)
            {
                material.CopyPropertiesFromMaterial(phantomMaterial);
            }*/
        }
    }

    public void ObtainOldMaterials()
    {
/*        foreach (MeshRenderer renderer in phantom.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.materials = objectMaterials;
        }*/
    }
}
