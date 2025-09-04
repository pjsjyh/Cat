using System.Collections.Generic;
using UnityEngine;

public class PlayerCatManager : MonoBehaviour
{
    public static PlayerCatManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void AddCatSaveData(CatSaveData cat)
    {
        PlayerDataManager.Instance.playerData.catData.catDataList.Add(cat);
    }

    public void RemoveCatSaveData(CatSaveData cat)
    {
        PlayerDataManager.Instance.playerData.catData.catDataList.Remove(cat);
    }
    public List<CatSaveData> GetCatSaveData()
    {
        return PlayerDataManager.Instance.playerData.catData.catDataList;
    }
    public void UpdateCatSaveData(CatSaveData cat)
    {
        int index = PlayerDataManager.Instance.playerData.catData.catDataList.FindIndex(c =>c.id == cat.id);
        if (index >= 0)
        {
            PlayerDataManager.Instance.playerData.catData.catDataList[index] = cat;
        }
    }
}
