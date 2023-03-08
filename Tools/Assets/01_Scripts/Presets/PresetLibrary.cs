using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Presets/Library")]
public class PresetLibrary : ScriptableObject
{
    public List<DefaultPresetLink> presets;
}

[System.Serializable]
public struct DefaultPresetLink
{
    public DefaultPreset preset;
    public GameObject prefab;
}
