using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Builder builder;

    private void Awake()
    {
        builder = FindObjectOfType<Builder>();
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
        List<Building> cityData = builder.buildings;
        SaveData save = new SaveData(cityData);

        SaveSystem.Save(save);

        Debug.Log("City Saved!");
    }

    public void Load()
    {
        SaveData save = SaveSystem.Load();
        builder.Reconstruct(save.builtObjects);

        Debug.Log("City Loaded!");
    }
}
