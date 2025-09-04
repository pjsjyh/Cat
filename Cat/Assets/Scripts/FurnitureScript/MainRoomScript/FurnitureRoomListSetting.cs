using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class FurnitureRoomListSetting : MonoBehaviour
{
    //가구 꾸미기 창 리스트 셋팅
    public GameObject parentObject;
    public GameObject furnitureListBox;

    private FurnitureInfo furnitureInfo;

    private bool furnitureSetting = false;
    private void Start()
    {
        furnitureInfo = GameObject.FindWithTag("InfoGroup").GetComponent<FurnitureInfo>();
        SettingFurnitureListBox();

    }
    private void OnEnable()
    {
        //main으로 돌아올때마다 정보 업데이트
        if(furnitureSetting)
            BoxListUpdate();
        furnitureSetting = true;
    }
    private void BoxListUpdate()
    {
        List<Furniture> getFurnitureList = PlayerFurnitureManager.Instance.GetFurnitures();
        Dictionary<string, GameObject> myFurnitureBox = furnitureInfo.ReturnAllFurnitureBoxList();
        foreach (Furniture f in getFurnitureList)
        {
            myFurnitureBox[f.furnitureId].GetComponent<FurnitureBoxItem>().SettingData(f);
        }
    }
    public void SettingFurnitureListBox()
    {
        //가지고 있는 가구 리스트만 박스 리스트로 표출하기
        Furniture[] allFurnitures = Resources.LoadAll<Furniture>("Data/Furniture");
        Debug.Log(PlayerDataManager.Instance.playerData.roomData.furnitureList.Count);
        foreach (FurnitureSaveData furniture in PlayerDataManager.Instance.playerData.roomData.furnitureList)
        {
            //현재 플레이어가 가지고 있는 가구 리스트 전부 추가.(설치 여부 상관 없음)
            Furniture matched = allFurnitures.FirstOrDefault(f => f.furnitureId == furniture.id);
            GameObject box;
            
            //이미 리스트에 있는 box인지
            if (furnitureInfo.FindFurnitureBoxInList(matched.furnitureId))
            {
                box = furnitureInfo.FindFurnitureBox(matched.furnitureId);
            }
            else
            {
                box = Instantiate(furnitureListBox, parentObject.transform);
            }
            if (furnitureListBox == null) Debug.Log("furnitureListBox없음");
            if (box == null) Debug.Log("box없음");
            Transform secondChild = box.transform.GetChild(0); // index 1 = 두 번째 자식
            Transform grandChild = secondChild.GetChild(0);    // 그 아래 자식 (index 0)

            try
            {
                //boxlist에 추가
                furnitureInfo.AddFurnitureBoxList(matched.furnitureId, box);
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
