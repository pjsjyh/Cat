using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class CatRoomFurniture : MonoBehaviour
{
    public GameObject FurnitureBox;
    public Transform furnitureParent;

    public Dictionary<string, Furniture> catRoomFurnitureBox = new(); //catroombox 가구 데이터

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
            catRoomFurnitureBox[furniture.furnitureId] = furniture;
        }
        
    }
    public void OnDisable()
    {
        foreach (Transform child in furnitureParent)
        {
            Destroy(child.gameObject);
        }
    }
}
