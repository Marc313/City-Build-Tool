using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Builder builder;

    private void Awake()
    {
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
        SaveData save = new SaveData(cityData, PresetCatalogue.presets);

        bool status = SaveSystem.Save(save);

        if (status) Debug.Log("City Saved!");
    }

    public void Load()
    {
        SaveData save = SaveSystem.Load();
        if (save != null)
        {
            builder.Reconstruct(save.builtObjects);
            PresetCatalogue.LoadList(save.presetCatalogue);

            Debug.Log("City Loaded!");
        }
    }
}
