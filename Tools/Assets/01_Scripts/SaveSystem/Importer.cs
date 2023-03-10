using SFB;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Dummiesman;
using UnityEditor.Presets;

public class Importer : MonoBehaviour
{
    public Button importButton;

    private void Start()
    {
        //Button btn = importButton.GetComponent<Button>();
        //importButton.onClick.AddListener(ImportOBJModel);
    }

    public void ImportOBJModel(string _name, Preset.Category _category, string _filePath, Vector2 _xzSize)
    {
        string objFileName = Path.GetFileName(_filePath);
        string destination = Path.Combine(FilepathManager.GetUserModelDirectory(), objFileName);
        if (File.Exists(destination))
        {
            File.Delete(destination);
            File.Delete(destination.Replace(".obj", ".mtl"));
        }
        File.Copy(_filePath, destination);
        File.Copy(_filePath.Replace(".obj", ".mtl"), destination.Replace(".obj", ".mtl"));

        AddPresetToCatalogue(_name, _category, _xzSize, objFileName);
    }

    private static void AddPresetToCatalogue(string _name, Preset.Category _category, Vector2 _xzSize, string objFileName)
    {
        if (!PresetCatalogue.PresetWithOBJNameExits(objFileName))
        {
            Preset newPreset = new UserPreset(_name, _category, objFileName, _xzSize);
            PresetCatalogue.AddNewEntry(newPreset);
        }
        else
        {
            Debug.Log("Moeilijk moeilijk");
        }
    }
}
