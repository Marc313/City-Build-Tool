using Ookii.Dialogs;
using System.Collections.Generic;
using UnityEngine;

public class UIList
{
    public List<GameObject> elements = new List<GameObject>();

    private GameObject prefab;
    private GameObject parent;

    // Layout \\
    private float elementOffset;
    private Vector3 direction;

    private Vector3 lastButtonPos;

    public UIList(GameObject _prefab, GameObject _parent, float _elementOffset, Vector3 _direction)
    {
        prefab = _prefab;
        parent = _parent;
        elementOffset = _elementOffset;
        direction = _direction;
    }

    public GameObject AddElement()
    {
        Debug.Log(lastButtonPos);
        GameObject newInstance = GameObject.Instantiate(prefab, parent.transform);
        elements.Add(newInstance);

        // Positioning
        if (elements.Count > 1)
        {
            lastButtonPos = lastButtonPos + direction * elementOffset;
            newInstance.transform.localPosition = lastButtonPos;
        }
        else
        {
            lastButtonPos = newInstance.transform.localPosition;
        }

        return newInstance;
    }

    public void Reset()
    {
        lastButtonPos = default;
        foreach (GameObject element in elements)
        {
            // Delete or Object pool!
            element.SetActive(false);
        }

        elements = new List<GameObject>();
    }

    public void EnableParent(bool _enabled)
    {
        parent.SetActive(_enabled);
    }
}
