using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
/*  private static IFormatter formatter = new BinaryFormatter();

    public static void SaveSettings()
    {
        formatter.Serialize();
    }

    public static void LoadFile()
    {
        formatter.Deserialize();
    }*/

    public static string fileName = "/SaveData.cb";     // .cb extension for "City Builder"

    public static bool Save(SaveData _data)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        string path = StandaloneFileBrowser.SaveFilePanel("Save Current City", FilepathManager.GetApplicationDirectory(), "newCity", "cb");
        if (path != null && path != string.Empty)
        {
            FileStream file = File.Create(path);
            formatter.Serialize(file, _data);
            file.Flush();
            file.Close();
            return true;
        }

        return false;
    }

    public static SaveData Load ()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Load Current City", FilepathManager.GetApplicationDirectory(), "cb", false);
        if (paths.Length > 0 && File.Exists(paths[0]))
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream file = File.Open(paths[0], FileMode.Open);

            try
            {
                object save = formatter.Deserialize(file);
                file.Flush();
                file.Close();
                return (SaveData)save;
            }
            catch
            {
                Debug.LogError($"Save File not found! Looked at: {GetFilePath()}");
                file.Close();
                return null;
            }
        }
        return null;
}

    public static bool SaveExists()
    {
        return File.Exists(GetFilePath());
    }

    public static string GetFilePath()
    {
        return Path.Combine(FilepathManager.GetApplicationDirectory(), fileName);
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter= new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();
        Vector2SerializationSurrogate vector2Surrogate = new Vector2SerializationSurrogate();
        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quatSurrogate = new QuaternionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2Surrogate);
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quatSurrogate);
        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
