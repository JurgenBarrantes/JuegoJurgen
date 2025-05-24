using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;

public class GameRepository
{
    static GameRepository instance;
    GameData gameData;

    public static GameRepository GetInstance()
    {
        if (instance == null)
        {
            instance = new GameRepository();
        }
        return instance;

    }

    public GameData GetData()
    {

        if (gameData != null)
        {
            return gameData;
        }

        string path = Application.persistentDataPath + "/data.save";

        if (File.Exists(path))
        {
            //return new GameData();
            FileStream file = File.OpenRead(path);

            BinaryFormatter bf = new BinaryFormatter();
            gameData = (GameData)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            gameData = new GameData();
        }
        return gameData;
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/data.save";
        FileStream file = null;
        if (File.Exists(path))
        {
            file = File.OpenWrite(path);
        }
        else
        {
            file = File.Create(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData);
        file.Close();
    } 
}

