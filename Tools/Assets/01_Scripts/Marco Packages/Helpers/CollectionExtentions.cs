using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public static class CollectionExtentions
{
    public static T GetRandomEntry<T>(this T[] _array)
    {
        if (_array == null || _array.Length == 0) return default(T);
        return _array[Random.Range(0, _array.Length)];
    }
}
