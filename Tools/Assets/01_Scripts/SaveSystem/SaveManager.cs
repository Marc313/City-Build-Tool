using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private Builder builder;

    private void Awake()
    {
        Instance = this;
        builder = FindObjectOfType<Builder>();
    }

    private void Start()
    {
    }

    public void Save(bool _saveAs)
    {
        List<PlacedObject> cityData = builder.buildings;
        SaveData save = new SaveData(cityData, PresetCatalogue.userPresets, FilepathManager.projectName);

        bool status = _saveAs ? SaveSystem.SaveAs(save) : SaveSystem.Save(save);

        if (status) Debug.Log("City Saved!");
        if (status) save.Debug();
    }

    public bool Load()
    {
        SaveData save = SaveSystem.Load();
        save.Debug();
        if (save != null)
        {
            FilepathManager.projectName = save.projectName;
            PresetCatalogue.LoadList(save.presetCatalogue);
            builder.Reconstruct(save.builtObjects);

            Debug.Log("City Loaded!");
            return true;
        }
        return false;
    }
}
