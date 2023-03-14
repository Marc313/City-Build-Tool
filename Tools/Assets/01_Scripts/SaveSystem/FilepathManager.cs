using System.IO;
using UnityEngine;

public static class FilepathManager
{
    public static string projectName = null;
    private static string userModelPath = "User_Models";

    public static string GetApplicationDirectory()
    {
        if (Application.isEditor)
        {
            return Path.Combine(Application.dataPath, "Runtime_Folders");
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
        string path = GetProjectDirectory();
        path = Path.Combine(path, userModelPath);

        return path;
    }

    public static void ClearUserModelDirectory()
    {
        string path = GetUserModelDirectory();
        Directory.Delete(path, true);
        Directory.CreateDirectory(path);
    }

    public static string GetProjectDirectory()
    {
        return Path.Combine(GetApplicationDirectory(), projectName);
    }

    public static string GetSavePath()
    {
        string path = Path.Combine(GetApplicationDirectory(), projectName + ".cb");
        return path;
    }
}
