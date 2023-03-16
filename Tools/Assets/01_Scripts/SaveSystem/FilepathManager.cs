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
        if (path == null) 
        {
            Logger.Log("Model Directory Could not be found");
        }
        
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
        if (CheckProjectPath())
            return Path.Combine(GetApplicationDirectory(), projectName);
        else return null;
    }

    public static string GetSavePath()
    {
        string path = Path.Combine(GetApplicationDirectory(), projectName + ".cb");
        return path;
    }

    public static bool CheckProjectPath()
    {
        if (projectName == null)
        {
            Logger.Log("Error: Project Folder not found!");
            return false;
        }
        return true;
    }
}
