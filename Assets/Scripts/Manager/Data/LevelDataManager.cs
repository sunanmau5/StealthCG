using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Author: Louis Sutopo
public class LevelDataManager : MonoBehaviour
{
    private LevelData levelData;
    private BinaryFormatter formatter = new BinaryFormatter();
    private string path;
    void Start()
    {
        // Docs: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
        path = Application.persistentDataPath + "/level.data";
        LoadLevelData();
    }

    // Call to get last level data (call on `StartScene`)
    public int GetLastLevel()
    {
        return levelData.Level;
    }

    // Mutate `levelData` object
    public void SetLastLevel(int lastLevel)
    {
        if (lastLevel > levelData.Level)
        {
            levelData.Level = lastLevel;
            SaveLevelData();
        }
    }

    // Save level progress on a file called `level.data`  
    void SaveLevelData()
    {
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    // Load level progress from a file called `level.data`. Create new `LevelData` when file doesn't exist.
    public void LoadLevelData()
    {
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            levelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();
        }
        else
        {
            levelData = new LevelData();
        }
    }
}
