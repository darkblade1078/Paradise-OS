using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/savefile.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData saveData = new SaveData();

        formatter.Serialize(stream, saveData);

        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            createDefaultSave();
            return LoadGame();
        }
    }

    private static void createDefaultSave()
    {
        SaveData defaultData = new SaveData();
        SaveGame(defaultData);
    }
}