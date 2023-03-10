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
        XZSizeUnits = new Vector2(1, 1);
    }

    public UserPreset(string _name, Category _category, string _objFileName, Vector2 _xzSize) : base(_name, _category)
    {
        objFileName = _objFileName;
        XZSizeUnits = _xzSize;
    }

    public override GameObject LoadInstance()
    {
        string objPath = Path.Combine(FilepathManager.GetUserModelDirectory(), objFileName);
        GameObject result = new OBJLoader().Load(objPath);
        return ApplyScaling(result);
    }
}
