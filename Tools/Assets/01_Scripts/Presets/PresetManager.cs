using System.Collections.Generic;
using UnityEngine;

public static class PresetManager
{
    public static List<Preset> presets = new List<Preset>();

    public static void LoadList(List<Preset> _presetList)
    {
        presets = _presetList;
        
        foreach (Preset preset in presets)
        {
            Debug.Log(preset.prefab.name);
        }
    }
}
