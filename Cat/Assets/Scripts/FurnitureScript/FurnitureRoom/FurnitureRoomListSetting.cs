using System;
using UnityEngine;
using UnityEngine.UI;
public class FurnitureRoomListSetting : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject furnitureListBox;
    private void Awake()
    {
        SettingFurnitureListBox();
    }
    public void SettingFurnitureListBox()
    {
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");
        Debug.Log(allFurnitures.Length);
        foreach (Furniture furniture in allFurnitures)
        {
            GameObject box = Instantiate(furnitureListBox, parentObject.transform);
            Transform secondChild = box.transform.GetChild(0); // index 1 = 두 번째 자식
            Transform grandChild = secondChild.GetChild(0);    // 그 아래 자식 (index 0)
            try
            {
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = furniture.FurnitureThumbnail.texture;

            }
            catch {
                Debug.LogError("가구셋팅에 문제 있음");
            }



        }

    }
}
