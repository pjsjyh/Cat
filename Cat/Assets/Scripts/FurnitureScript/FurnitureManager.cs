using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FurnitureManager : MonoBehaviour
{
    //ó�� ����� ���� ���� �Ŵ���
    public static FurnitureManager Instance { get; private set; }
    public Transform furnitureParent; // ������ �߰� �� �θ�

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
        DontDestroyOnLoad(gameObject); // ���� �ٲ� �����ǰ�

    }
    private void Start()
    {
        SpawnFurnitures(PlayerDataManager.Instance.playerData.roomData.furnitureList);
    }
    public void SpawnFurnitures(List<FurnitureSaveData> furnitureList)
    {
        //���� ���� �� ���� ����
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
        //������ ��ġ ����Ʈ�� �߰�
        placedFurniture[getId] = getObj;
    }
    public bool FurnitureIsPlaced(string getId)
    {
        //���� ��ġ ���� Ȯ��
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
        //���� �ٹ̱� ���� �� ������ ����
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
