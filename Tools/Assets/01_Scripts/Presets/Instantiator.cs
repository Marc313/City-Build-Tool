using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public static GameObject Instantiate(GameObject _object)
    {
        return GameObject.Instantiate(_object);
    }
}
