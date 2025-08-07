using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FurnitureRoomListSetting : MonoBehaviour
{
    //가구 꾸미기 창 리스트 셋팅
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
            //현재 플레이어가 가지고 있는 가구 리스트 전부 추가.(설치 여부 상관 없음)
            Furniture matched = allFurnitures.FirstOrDefault(f => f.furnitureId == furniture.id);
            GameObject box;
            
            //이미 리스트에 있는 box인지
            if (FurnitureInfo.Instance.FindFurnitureBoxInList(matched.furnitureId))
            {
                box = FurnitureInfo.Instance.FindFurnitureBox(matched.furnitureId);
            }
            else
            {
                box = Instantiate(furnitureListBox, parentObject.transform);
            }

            Transform secondChild = box.transform.GetChild(0); // index 1 = 두 번째 자식
            Transform grandChild = secondChild.GetChild(0);    // 그 아래 자식 (index 0)

            try
            {
                //boxlist에 추가
                FurnitureInfo.Instance.AddFurnitureBoxList(matched.furnitureId, box);
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = matched.FurnitureThumbnail.texture;
                box.GetComponent<FurnitureBoxItem>().SettingData(matched);
                box.GetComponent<FurnitureBoxItem>().CheckIsPlaced(matched.furnitureId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[가구 셋팅 에러] 예외 발생: {ex.GetType().Name} - {ex.Message}\n스택트레이스: {ex.StackTrace}");
            }



        }

    }
}
