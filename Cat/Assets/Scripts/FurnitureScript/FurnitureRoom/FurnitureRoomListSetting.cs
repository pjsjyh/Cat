using UnityEngine;

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
        }

    }
}
