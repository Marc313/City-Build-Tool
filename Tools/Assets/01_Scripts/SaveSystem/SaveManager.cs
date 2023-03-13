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
        FilepathManager.CreateUserModelDirectory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        List<PlacedObject> cityData = builder.buildings;
        SaveData save = new SaveData(cityData, PresetCatalogue.userPresets, FilepathManager.projectName);

        bool status = SaveSystem.Save(save);

        if (status) Debug.Log("City Saved!");
    }

    public bool Load()
    {
        SaveData save = SaveSystem.Load();
        if (save != null)
        {
            PresetCatalogue.LoadList(save.presetCatalogue);
            builder.Reconstruct(save.builtObjects);
            FilepathManager.projectName = save.projectName;

            Debug.Log("City Loaded!");
            return true;
        }
        return false;
    }
}
