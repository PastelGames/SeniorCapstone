using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public Record data;

    public string file = "results";

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);
    }

    public void Load()
    {
        data = new Record();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string GetFilePath(string fileName)
    {
        return "D:/Repositories/Senior Capstone/Assets/" + fileName;
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found!");
            return "";
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
