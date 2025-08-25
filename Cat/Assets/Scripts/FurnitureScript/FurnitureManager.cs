using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class FurnitureManager : MonoBehaviour
{
    //ó�� ����� ���� ���� �Ŵ���
    public static FurnitureManager Instance { get; private set; }
    public Transform furnitureParent; // ������ �߰� �� �θ�

    private Dictionary<string, GameObject> placedFurniture = new(); //��ġ�� ���� ����Ʈ
    private Dictionary<string, FurnitureSaveData> furnitureSaveData = new(); //���� saveData
    public Dictionary<string, Furniture> allFurnitureData= new(); //��� ���� ������
    
    private bool furnitureEditorModeOn = false;

    [SerializeField] 
    FloorNavGrid grid;     
    [SerializeField] 
    DepthSorter depthSorter;

    public event Action<string, int> OnPiecesChanged;
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
        StartCoroutine(EndOfFrameBuild()); // �� �� ������ �ڿ� ����/���
        setAllFurnitureData();
    }
    public void SpawnFurnitures(List<FurnitureSaveData> furnitureList)
    {
        //���� ���� �� ���� ����
        foreach (var furnitureSaveData in furnitureList)
        {
            Furniture furnitureData = Resources.Load<Furniture>($"Data/Furniture/{furnitureSaveData.id}");
            furnitureData.isPlaced = furnitureSaveData.isPlaced;
            furnitureData.installPosition = furnitureSaveData.position;
            furnitureData.nowPeice = furnitureSaveData.nowPeice;

            allFurnitureData[furnitureData.furnitureId] = (furnitureData);

            if (furnitureSaveData.isPlaced)
            {
                GameObject furnitureObj = Instantiate(furnitureData.FurniturePrefab, furnitureSaveData.position, Quaternion.identity, furnitureParent);
                furnitureObj.transform.SetParent(furnitureParent.transform, false);

                furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furnitureData);
                placedFurniture[furnitureSaveData.id] = furnitureObj;
            }
           
        }
    }
    private void setAllFurnitureData()
    {
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");
        foreach (Furniture f in allFurnitures)
        {
            allFurnitureData[f.furnitureId] = f;
        }
    }
    IEnumerator EndOfFrameBuild()
    {
        yield return null; // RectTransform ��ġ�� �������� 1������ ���

        if (grid) grid.Clear();

        foreach (var go in placedFurniture.Values)
        {
            go.GetComponent<FurnitureNav>()?.Register(); // ���� �� ����
        }
        depthSorter?.SortNow(); // ��/�� ����
    }
    public void AddPieces(string id, int add)
    {
        if (!allFurnitureData.TryGetValue(id, out var s)) return;
        
        
        s.nowPeice += add;
        // ���� ȣ���� ���⼭: PlayerDataManager.Instance.SaveData();
        OnPiecesChanged?.Invoke(id, s.nowPeice);
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
                isPlaced = fData.isPlaced,
                installLocation = fData.installLocation,
                nowPeice = fData.nowPeice,
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
    public List<Furniture> ReturnAllFurnitureData()
    {
        List<Furniture> furnitureList = new List<Furniture>(allFurnitureData.Values);
        return furnitureList;
    }

}
