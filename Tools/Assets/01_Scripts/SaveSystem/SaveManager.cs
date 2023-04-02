using JetBrains.Annotations;
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

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save(false);
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Load();
            }
        }
    }

    public void Save(bool _saveAs)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        List<PlacedObject> cityData = builder.GetPlacedObjectList();
        CamNavigation camera = FindObjectOfType<CamNavigation>();
        SaveData save = null;
        if (camera == null)
        {
            save = new SaveData(cityData, PresetCatalogue.userPresets, FilepathManager.projectName);
        }
        else 
        {
            save = new SaveData(cityData, PresetCatalogue.userPresets, FilepathManager.projectName, camera.transform.position);
        }

        bool status = _saveAs ? SaveSystem.SaveAs(save) : SaveSystem.Save(save);

        if (status) Logger.Log("City Saved!");
    }

    public bool Load()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SaveData save = SaveSystem.Load();
        if (save != null)
        {
            FilepathManager.projectName = save.projectName;
            PresetCatalogue.LoadList(save.presetCatalogue);
            builder.Reconstruct(save.builtObjects);
            FindObjectOfType<CamNavigation>()?.LoadPosition(save.cameraPosition);

            Logger.Log("City Loaded!");
            return true;
        }
        return false;
    }
}
