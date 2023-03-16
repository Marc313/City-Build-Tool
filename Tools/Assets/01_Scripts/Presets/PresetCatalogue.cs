using MarcoHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PresetCatalogue
{
    public static List<Preset> allPresets = new List<Preset>();
    public static List<Preset> userPresets = new List<Preset>();
    public static List<DefaultPresetLink> defaultPresets = new List<DefaultPresetLink>();

    public static void SetDefaultPresets(List<DefaultPresetLink> _presetList, bool _addToPresets = true)
    {
        foreach (DefaultPresetLink df in _presetList)
        {
            defaultPresets.Add(df);
        }

        if (defaultPresets != null) Debug.Log(defaultPresets.Count);

        if (_addToPresets)
        {
            foreach (DefaultPresetLink df in defaultPresets)
            {
                if (df == null) Debug.Log("Links are null!");
                df.Debug();
            }

            Preset[] presets = defaultPresets.Select(link => link.preset).ToArray();
            Debug.Log("Presets: " + presets.Length);
            foreach (Preset preset in presets)
            {
                preset.presetID = GetFirstAvailableID();
                allPresets.Add(preset);
            }
        }

        EventSystem.RaiseEvent(EventName.PRESETS_LOADED);
    }

    public static void LoadList(List<Preset> _presetList)
    {
        userPresets = _presetList;

        foreach (Preset preset in userPresets)
        {
            if (!allPresets.Contains(preset))
            {
                allPresets.Add(preset);
            }
        }

        EventSystem.RaiseEvent(EventName.PRESETS_LOADED);
    }

    public static Preset GetPresetByID(int _presetID)
    {
        return allPresets.Where(p => p.presetID == _presetID).First();
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
        foreach (Preset preset in allPresets)
        {
            UserPreset userPreset = preset as UserPreset;
            if (userPreset != null && _objName == userPreset.objFileName) return true;
        }

        return false;
    }

    public static void AddNewEntry(Preset _preset)
    {
        userPresets.Add(_preset);
        allPresets.Add(_preset);
    }

    public static int GetFirstAvailableID()
    {
        if (allPresets == null) return -1;
        return allPresets.Count;
    }
}
