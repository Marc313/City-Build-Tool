using UnityEngine;

[CreateAssetMenu(menuName = "Presets/DefaultPreset")]
public class DefaultPresetLink : ScriptableObject
{
    public DefaultPreset preset;
    public GameObject prefab;

    public void Debug()
    {
        UnityEngine.Debug.Log("Preset: " + preset);
        UnityEngine.Debug.Log("Prefab: " + prefab);
    }
}