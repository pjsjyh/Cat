using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CatRoomFurniture : MonoBehaviour
{
    public GameObject FurnitureBox;
    public Transform furnitureParent;

    public Dictionary<string, GameObject> catRoomFurnitureBox = new(); //catroombox 가구 데이터

    public void OnEnable()
    {
        SettingBox();
    }
    public void SettingBox()
    {
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");
        foreach (var furniture in allFurnitures)
        {
            GameObject furnitureObj = Instantiate(FurnitureBox, furnitureParent.position, Quaternion.identity, furnitureParent);
            //furnitureObj.transform.SetParent(furnitureParent.transform, false);
            furnitureObj.GetComponent<FurnitureSetting>().SettingFurniture(furniture);
            catRoomFurnitureBox[furniture.furnitureId] = furnitureObj;
        }
        
    }
    public void OnDisable()
    {
        foreach (Transform child in furnitureParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void OnClickUpgradeFurniture()
    {
        List<Furniture> furni = FurnitureManager.Instance.ReturnAllFurnitureData();
        for(int i=0;i< furni.Count; i++)
        {
            int nowPieceLevel = furni[i].nowPeiceLevel;
            int sum = 0;
            List<int> list = GameManager.Instance.shardCostsByLevel;
            for (int j = 0; j < list.Count; j++)
            {
                sum += list[j];
                if (furni[i].nowPeice <= sum)
                {
                    nowPieceLevel = j;
                }
            }
            furni[i].nowPeiceLevel = nowPieceLevel;
            catRoomFurnitureBox[furni[i].furnitureId].GetComponent<FurnitureSetting>().SettingFurniture(furni[i]);
        }
       
    }
}
