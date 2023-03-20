using UnityEngine;
using UnityEngine.UI;
using System.IO;
using MarcoHelpers;

public class Importer : MonoBehaviour
{
    public Button importButton;

    // Temporarily saved data:
    private string currentName;
    private Preset.Category category;
    private string filePath;
    private Vector2 xzSize;
    private string objFileName;
    private string destination;

    // TODO: Check if _filePath is valid!
    public void ImportOBJModel(string _name, Preset.Category _category, string _filePath, Vector2 _xzSize)
    {
        Debug.Log("Importing...");
        currentName = _name;
        category = _category;
        filePath = _filePath;
        xzSize = _xzSize;

        objFileName = Path.GetFileName(_filePath);
        destination = Path.Combine(FilepathManager.GetUserModelDirectory(), objFileName);
        if (File.Exists(destination) || PresetCatalogue.PresetWithOBJNameExits(objFileName))
        {
            ShowDuplicateMenu();
        }
        else
        {
            ContinuePresetCreation(_filePath, objFileName, destination);
        }
    }

    private void ContinuePresetCreation(string _filePath, string objFileName, string destination)
    {
        MakeModelCopy(_filePath, destination);
        AddPresetToCatalogue(currentName, category, xzSize, objFileName);
    }

    private static void MakeModelCopy(string _filePath, string destination)
    {
        File.Copy(_filePath, destination);
        File.Copy(_filePath.Replace(".obj", ".mtl"), destination.Replace(".obj", ".mtl"));
    }

    private static void AddPresetToCatalogue(string _name, Preset.Category _category, Vector2 _xzSize, string objFileName)
    {
        if (!PresetCatalogue.PresetWithOBJNameExits(objFileName))
        {
            Preset newPreset = new UserPreset(_name, _category, objFileName, _xzSize);
            PresetCatalogue.AddNewEntry(newPreset);
            EventSystem.RaiseEvent(EventName.IMPORT_SUCCESS, newPreset);
            Logger.Log($"New {_name} preset added to {_category.ToString()} category!");
        }
        else
        {
            Debug.Log("Moeilijk moeilijk");
            Logger.Log($"Import failed");

        }
    }

    private void ShowDuplicateMenu()
    {
        EventSystem.RaiseEvent(EventName.ON_OBJNAME_ALREADY_EXISTS);
    }

    public void ReplaceFile()
    {
        File.Delete(destination);
        File.Delete(destination.Replace(".obj", ".mtl"));
        ContinuePresetCreation(filePath, objFileName, destination);
    }

    public void CreateNewWithOtherName()
    {
        destination = destination.Replace(".obj", "_01.obj");
        objFileName = objFileName.Replace(".obj", "_01.obj");
        ContinuePresetCreation(filePath, objFileName, destination);
    }
}
