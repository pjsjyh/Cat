using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    //ó�� ����� ���� ���� �Ŵ���
    public static FurnitureManager Instance { get; private set; }
    public Transform furnitureParent; // ������ �߰� �� �θ�

    private Dictionary<string, GameObject> placedFurniture = new();
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
            if (furnitureData.isPlaced)
            {
                GameObject furnitureObj = Instantiate(furnitureData.FurniturePrefab, furnitureSaveData.position, Quaternion.identity, furnitureParent);
                furnitureObj.transform.SetParent(furnitureParent.transform, false);

                placedFurniture[furnitureSaveData.id] = furnitureObj;
            }
           
            //FurnitureController controller = furnitureObj.GetComponent<FurnitureController>();
            //controller.Init(furnitureSaveData);
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
}
