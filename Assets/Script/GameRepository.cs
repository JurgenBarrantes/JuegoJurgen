using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameRepository
{
    static GameRepository instance;
    private static readonly object fileLock = new object();

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

        lock (fileLock)
        {
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        gameData = (GameData)bf.Deserialize(file);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error al leer data.save: " + e.Message);
                    gameData = new GameData(); // fallback seguro
                }
            }
            else
            {
                gameData = new GameData();
            }

            return gameData;
        }
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/data.save";

        lock (fileLock)
        {
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(file, gameData);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al guardar data.save: " + e.Message);
            }
        }
    }
}

