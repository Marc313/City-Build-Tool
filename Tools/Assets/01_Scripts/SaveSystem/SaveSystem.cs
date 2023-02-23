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

    public static string fileName = "/SaveData.txt";
/*
    public static void Save(SaveData saveData)
    {
        StreamWriter writer = new StreamWriter(GetFilePath(), false);
        writer.WriteLine(JsonUtility.ToJson(saveData, true));
        writer.Close();
        writer.Dispose();
    }

    public static SaveData Load()
    {
        if (!File.Exists(GetFilePath())) return null;

        StreamReader reader = new StreamReader(GetFilePath());
        SaveData data = JsonUtility.FromJson<SaveData>(reader.ReadToEnd());
        reader.Close();
        reader.Dispose();

        return data;
        //return new SaveData(data.DungeonSeed, data.SceneIndex);
    }

    public static void DeleteSave()
    {
        if (SaveExists())
        {
            File.Delete(GetFilePath());
        }
    }*/

    public static void Save(SaveData _data)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        FileStream file = File.Create(GetFilePath());
        formatter.Serialize(file, _data);
        file.Flush();
        file.Close();
    }

    public static SaveData Load ()
    {
        if (!SaveExists()) return null;

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(GetFilePath(), FileMode.Open);

        try
        {
            object save = formatter.Deserialize(file);
            file.Flush();
            file.Close();
            return (SaveData) save;
        }
        catch
        {
            Debug.LogError($"Save File not found! Looked at: {GetFilePath()}");
            file.Close();
            return null;
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(GetFilePath());
    }

    public static string GetFilePath()
    {
        if (Application.isEditor)
        {
            return Application.dataPath + fileName;
        }
        else
        {
            return Application.persistentDataPath + fileName;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter= new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();
        Vector3SerializationSurrogate vectorSurrogate = new Vector3SerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vectorSurrogate);
        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
