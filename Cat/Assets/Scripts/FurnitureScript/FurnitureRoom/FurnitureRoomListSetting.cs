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
            Transform secondChild = box.transform.GetChild(0); // index 1 = �� ��° �ڽ�
            Transform grandChild = secondChild.GetChild(0);    // �� �Ʒ� �ڽ� (index 0)
            try
            {
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = furniture.FurnitureThumbnail.texture;

            }
            catch {
                Debug.LogError("�������ÿ� ���� ����");
            }



        }

    }
}
