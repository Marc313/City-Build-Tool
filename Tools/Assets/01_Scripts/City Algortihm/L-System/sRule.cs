using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "L-System/Rule")]
public class sRule : ScriptableObject
{
    public string letter;
    [SerializeField] private string[] results = null;

    [Tooltip("Check this to choose a random result from result list. When false, result will be the first element of result list.")]
    [SerializeField] private bool randomResult;

    public string GetResults()
    {
        if (randomResult)
        {
            return results.GetRandomEntry();
        }
        else
        {
            return results[0];
        }
    }

}
