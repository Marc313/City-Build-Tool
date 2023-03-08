using Dummiesman;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserPreset : Preset
{
    public string objFileName;

    public UserPreset(string _name, Category _category, string _objFileName) : base(_name, _category)
    {
        objFileName = _objFileName;
    }

    public override GameObject LoadInstance()
    {
        string objPath = Path.Combine(FilepathManager.GetUserModelDirectory(), objFileName);
        return new OBJLoader().Load(objPath);
    }
}
