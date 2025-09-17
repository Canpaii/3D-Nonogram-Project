using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    private static string SaveFolder => Path.Combine(Application.persistentDataPath, "Saves");

    // Save data to a file
    public static void Save<T>(T data, string fileName)
    {
        if (!Directory.Exists(SaveFolder))
            Directory.CreateDirectory(SaveFolder);

        string json = JsonUtility.ToJson(data, true); // pretty print
        string path = Path.Combine(SaveFolder, fileName + ".json");

        File.WriteAllText(path, json);
        Debug.Log($"[SaveSystem] Saved to {path}");
    }

    // Load data from a file
    public static T Load<T>(string fileName)
    {
        string path = Path.Combine(SaveFolder, fileName + ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[SaveSystem] File not found at {path}");
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }

    // Get a list of all save files
    public static List<string> GetAllSaveFiles()
    {
        if (!Directory.Exists(SaveFolder))
            return new List<string>();

        string[] files = Directory.GetFiles(SaveFolder, "*.json");
        List<string> fileNames = new List<string>();

        foreach (string file in files)
            fileNames.Add(Path.GetFileNameWithoutExtension(file));

        return fileNames;
    }
}


