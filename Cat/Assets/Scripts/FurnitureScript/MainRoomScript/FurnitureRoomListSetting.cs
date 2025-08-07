using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FurnitureRoomListSetting : MonoBehaviour
{
    //���� �ٹ̱� â ����Ʈ ����
    public GameObject parentObject;
    public GameObject furnitureListBox;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        SettingFurnitureListBox();
    }
    public void SettingFurnitureListBox()
    {
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");

        foreach (FurnitureSaveData furniture in PlayerDataManager.Instance.playerData.roomData.furnitureList)
        {
            //���� �÷��̾ ������ �ִ� ���� ����Ʈ ���� �߰�.(��ġ ���� ��� ����)
            Furniture matched = allFurnitures.FirstOrDefault(f => f.furnitureId == furniture.id);
            GameObject box;
            
            //�̹� ����Ʈ�� �ִ� box����
            if (FurnitureInfo.Instance.FindFurnitureBoxInList(matched.furnitureId))
            {
                box = FurnitureInfo.Instance.FindFurnitureBox(matched.furnitureId);
            }
            else
            {
                box = Instantiate(furnitureListBox, parentObject.transform);
            }

            Transform secondChild = box.transform.GetChild(0); // index 1 = �� ��° �ڽ�
            Transform grandChild = secondChild.GetChild(0);    // �� �Ʒ� �ڽ� (index 0)

            try
            {
                //boxlist�� �߰�
                FurnitureInfo.Instance.AddFurnitureBoxList(matched.furnitureId, box);
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = matched.FurnitureThumbnail.texture;
                box.GetComponent<FurnitureBoxItem>().SettingData(matched);
                box.GetComponent<FurnitureBoxItem>().CheckIsPlaced(matched.furnitureId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[���� ���� ����] ���� �߻�: {ex.GetType().Name} - {ex.Message}\n����Ʈ���̽�: {ex.StackTrace}");
            }



        }

    }
}
