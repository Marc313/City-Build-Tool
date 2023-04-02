using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransformer : MonoBehaviour
{
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
    }
}
