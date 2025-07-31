using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FurnitureManager : MonoBehaviour
{
    //처음 사용자 가구 셋팅 매니저
    public static FurnitureManager Instance { get; private set; }
    public Transform furnitureParent; // 가구가 추가 될 부모

    private Dictionary<string, GameObject> placedFurniture = new();
    private Dictionary<string, FurnitureSaveData> furnitureSaveData = new();

    private bool furnitureEditorModeOn = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지되게

    }
    private void Start()
    {
        SpawnFurnitures(PlayerDataManager.Instance.playerData.roomData.furnitureList);
    }
    public void SpawnFurnitures(List<FurnitureSaveData> furnitureList)
    {
        //게임 시작 시 가구 셋팅
        foreach (var furnitureSaveData in furnitureList)
        {
            Furniture furnitureData = Resources.Load<Furniture>($"Data/Furniture/{furnitureSaveData.id}");
            furnitureData.isPlaced = furnitureSaveData.isPlaced;
            furnitureData.installPosition = furnitureSaveData.position;
            if (furnitureSaveData.isPlaced)
            {
                Debug.Log(furnitureData.furnitureId);

                GameObject furnitureObj = Instantiate(furnitureData.FurniturePrefab, furnitureSaveData.position, Quaternion.identity, furnitureParent);
                furnitureObj.transform.SetParent(furnitureParent.transform, false);

                furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furnitureData);
                placedFurniture[furnitureSaveData.id] = furnitureObj;
            }
           
        }
    }
    public void AddFurniture(string getId, GameObject getObj)
    {
        //가구를 설치 리스트에 추가
        placedFurniture[getId] = getObj;
    }
    public bool FurnitureIsPlaced(string getId)
    {
        //가구 설치 여부 확인
        if (placedFurniture.ContainsKey(getId))
        {
            return true;
        }
        return false;
    }
    public GameObject FindFurniture(string getId)
    {
        if (placedFurniture.ContainsKey(getId))
        {
            return placedFurniture[getId];
        }
        return null;
    }
    public void RemoveFurnitureInPlace(string getId)
    {
        placedFurniture.Remove(getId);
        furnitureSaveData.Remove(getId);
    }
    public void DataUpdateFurniture()
    {
        //가구 꾸미기 종료 후 데이터 저장
        foreach (var furniture in placedFurniture)
        {
            Furniture fData = furniture.Value.GetComponent<FurnitureDragHandler>().ReturnFurnitureData();

            FurnitureSaveData saveData = new FurnitureSaveData
            {
                id = fData.furnitureId,
                position = fData.installPosition,
                isPlaced = fData.isPlaced
            };

            furnitureSaveData[fData.furnitureId] = saveData;
        }
        PlayerDataManager.Instance.playerData.roomData.furnitureList = furnitureSaveData.Values.ToList();
    }
    public bool isFurnitureEditorModeOn()
    {
        return furnitureEditorModeOn;
    }
    public void SetFurnitureEditorMode(bool getMode)
    {
        furnitureEditorModeOn = getMode;
    }
}
