using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string progressPath = Application.persistentDataPath + "/progress.save"; 
    public static void SaveProgress (LevelProgressController levelProgressController)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(progressPath, FileMode.Create);
        ProgressData data = new ProgressData(levelProgressController);
        Debug.Log("Level Index Saved: " + data.LevelIndex);
        Debug.Log("Spawn Point Saved: " + data.SpawnPoint);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProgressData LoadProgress()
    {
        if (File.Exists(progressPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(progressPath, FileMode.Open);

            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close(); 

            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in " + progressPath);
            return null;
        }
    }
}