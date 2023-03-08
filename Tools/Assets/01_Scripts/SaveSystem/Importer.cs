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
        importButton.onClick.AddListener(ImportOBJModel);
    }

    private void ImportOBJModel()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Import OBJ Model", "", "obj", false);
        if (paths.Length <= 0 || paths[0] == null) return;

        // Copy File to UserModels folder
        string filePath = paths[0];
        string ObjFileName = Path.GetFileName(filePath);
        string destination = Path.Combine(FilepathManager.GetUserModelDirectory(), ObjFileName);
        if (File.Exists(destination))
        {
            File.Delete(destination);
            File.Delete(destination.Replace(".obj", ".mtl"));
        }
        File.Copy(filePath, destination);
        File.Copy(filePath.Replace(".obj", ".mtl"), destination.Replace(".obj", ".mtl"));

        AddToPresetCatalogue(ObjFileName);
    }

    private void AddToPresetCatalogue(string _objFileName)
    {
        if (!PresetCatalogue.PresetWithOBJNameExits(_objFileName))
        {
            Preset newPreset = new UserPreset(_objFileName.Replace(".obj", ""), Preset.Category.Road, _objFileName);
            PresetCatalogue.AddNewEntry(newPreset);
        }
        else
        {
            Debug.Log("Moeilijk moeilijk");
        }

    }
}
