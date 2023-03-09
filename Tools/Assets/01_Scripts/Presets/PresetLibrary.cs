using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Presets/Library")]
public class PresetLibrary : ScriptableObject
{
    public List<DefaultPresetLink> presets;
}

[CreateAssetMenu(menuName = "Presets/DefaultPreset")]
public class DefaultPresetLink : ScriptableObject
{
    public DefaultPreset preset;
    public GameObject prefab;
}
