using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Presets/Library")]
public class PresetLibrary : ScriptableObject
{
    public List<DefaultPreset> presets;
}
