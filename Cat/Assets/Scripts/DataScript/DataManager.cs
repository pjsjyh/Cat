using System.IO;
using UnityEngine;
using static PlayerDataFrame;
public static class DataManager
{
    static string savePath => Application.persistentDataPath + "/CatGameSaveData.json";


    public static void SaveData(PlayerData playerData)
    {
        if (playerData != null){
            string json = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("저장 완료: " + json);
        }
        
    }
    public static PlayerData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Debug.Log(json);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            return new PlayerData();
        }
    }

}
