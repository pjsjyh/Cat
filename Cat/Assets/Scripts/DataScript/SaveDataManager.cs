using System.IO;
using UnityEngine;
using static PlayerDataFrame;
public static class SaveDataManager
{
    static string savePath => Application.persistentDataPath + "/CatGameSaveData.json";


    public static void SaveData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
    }
    public static PlayerData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            return new PlayerData();
        }
    }

}
