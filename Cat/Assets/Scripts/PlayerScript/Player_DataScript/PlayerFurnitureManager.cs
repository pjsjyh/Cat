using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PlayerFurnitureManager : MonoBehaviour
{
    public static PlayerFurnitureManager Instance { get; private set; }

    public event Action OnFurnitureChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void AddFurnitureSaveData(FurnitureSaveData furniture)
    {
        PlayerDataManager.Instance.playerData.roomData.furnitureList.Add(furniture);
    }

    public void RemoveFurnitureSaveData(FurnitureSaveData furniture)
    {
        PlayerDataManager.Instance.playerData.roomData.furnitureList.Remove(furniture);
    }
    public List<FurnitureSaveData> GetFurnituresSaveData()
    {
        return PlayerDataManager.Instance.playerData.roomData.furnitureList;
    }
    public void UpdateFurnitureSaveData(FurnitureSaveData furniture)
    {
        int index = PlayerDataManager.Instance.playerData.roomData.furnitureList.FindIndex(f => f.id == furniture.id);
        if (index >= 0)
        {
            PlayerDataManager.Instance.playerData.roomData.furnitureList[index] = furniture;
        }
    }
    public List<Furniture> GetFurnitures()
    {
        return PlayerDataManager.Instance.playerFurniture;
    }
    public void AddFurniture(Furniture furniture)
    {
        PlayerDataManager.Instance.playerFurniture.Add(furniture);
        OnFurnitureChanged?.Invoke();
    }

    public void RemoveFurniture(Furniture furniture)
    {
        PlayerDataManager.Instance.playerFurniture.Remove(furniture);
        OnFurnitureChanged?.Invoke();
    }
    public void UpdateFurniture(Furniture furniture)
    {
        int index = PlayerDataManager.Instance.playerFurniture.FindIndex(f => f.furnitureId == furniture.furnitureId);
        if (index >=0) {
            PlayerDataManager.Instance.playerFurniture[index] = furniture;
        }
    }
    public void DataUpdateFurniture()
    {
        List<FurnitureSaveData> myData = GetFurnituresSaveData();
        //가구 꾸미기 종료 후 데이터 저장
        foreach (var furniture in GetFurnitures())
        {
            FurnitureSaveData saveData = new FurnitureSaveData
            {
                id = furniture.furnitureId,
                position = furniture.installPosition,
                isPlaced = furniture.isPlaced,
                installLocation = furniture.installLocation,
                nowPeice = furniture.nowPeice,
            };
            UpdateFurnitureSaveData(saveData);
        }
    }
}
