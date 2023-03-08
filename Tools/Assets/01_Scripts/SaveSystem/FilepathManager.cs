using System.IO;
using UnityEngine;

public static class FilepathManager
{
    public static string userModelPath = "User_Models";

    public static string GetApplicationDirectory()
    {
        if (Application.isEditor)
        {
            return Application.dataPath;
        }
        else
        {
            return Application.persistentDataPath;
        }
    }

    public static void CreateUserModelDirectory()
    {
        string path = GetUserModelDirectory();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static string GetUserModelDirectory()
    {
        return Path.Combine(GetApplicationDirectory(), userModelPath);
    }

    public static void ClearUserModelDirectory()
    {
        string path = GetUserModelDirectory();
        Directory.Delete(path, true);
        Directory.CreateDirectory(path);
    }

}
