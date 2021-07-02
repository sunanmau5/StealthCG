using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelDataManager : MonoBehaviour
{
    private LevelData levelData;
    private BinaryFormatter formatter = new BinaryFormatter();
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/level.data";
        LoadLevelData();
    }

    // call to get last level data (call on `StartScene`)
    public int GetLastLevel()
    {
        return levelData.Level;
    }

    public void SetLastLevel(int lastLevel)
    {
        if (lastLevel > levelData.Level)
        {
            levelData.Level = lastLevel;
            SaveLevelData();
        }
    }

    void SaveLevelData()
    {
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, levelData);
        stream.Close();
    }

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
