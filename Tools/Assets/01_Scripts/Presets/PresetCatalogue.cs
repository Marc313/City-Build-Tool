using MarcoHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PresetCatalogue
{
    public static List<Preset> presets = new List<Preset>();
    public static List<DefaultPresetLink> defaultPresets = new List<DefaultPresetLink>();

    public static void SetDefaultPresets(List<DefaultPresetLink> _presetList, bool _addToPresets = true)
    {
        defaultPresets = _presetList;

        if (_addToPresets)
        {
            foreach (Preset preset in _presetList.Select(link => link.preset))
            {
                presets.Add(preset);
            }
        }
    }

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

    public static GameObject GetPrefabByPreset(Preset _preset)
    {
        if (defaultPresets.Count <= 0) return null;
        var list = defaultPresets.Where(link => link.preset.presetID == _preset.presetID);
        if (list == null) return null;
        var prefab = list.FirstOrDefault().prefab;
        return prefab;
    }

    public static bool PresetWithOBJNameExits(string _objName)
    {
        foreach (Preset preset in presets)
        {
            UserPreset userPreset = preset as UserPreset;
            if (userPreset != null && _objName == userPreset.objFileName) return true;
        }

        return false;
    }
}
