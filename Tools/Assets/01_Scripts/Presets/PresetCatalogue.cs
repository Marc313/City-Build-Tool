using MarcoHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PresetCatalogue
{
    public static List<Preset> presets = new List<Preset>();

    public static void LoadList(List<Preset> _presetList)
    {
        presets = _presetList;

        foreach (Preset preset in presets)
        {
            Debug.Log(preset.category);
        }

        EventSystem.RaiseEvent(EventName.PRESETS_LOADED);
    }

    public static Preset GetPresetByID(int _presetID)
    {
        return presets.Where(p => p.presetID == _presetID).First();
    }

    public static bool PresetWithOBJNameExits(string _objName)
    {
        foreach (UserPreset preset in presets)
        {
            if (_objName == preset.objFileName) return true;
        }

        return false;
    }
}
