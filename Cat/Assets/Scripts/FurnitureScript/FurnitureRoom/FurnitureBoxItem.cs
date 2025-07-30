using UnityEngine;

public class FurnitureBoxItem : MonoBehaviour
{
    //가구 list box 스크립트. 상자 클릭시(this) 방에 가구 생성
    private GameObject furnitureParent;
    private GameObject furnitureSliding;

    [SerializeField]
    private GameObject furnitureCheckBtn;

    private Furniture furnitureData;
    
    private void Awake()
    {
        furnitureParent = FurnitureInfo.Instance.furnitureParent;
        furnitureSliding = FurnitureInfo.Instance.furnitureSliding;
        
    }
    public void CheckIsPlaced(string id)
    {
        //화면에 설치 된 가구 인지 확인
        if (FurnitureManager.Instance.FurnitureIsPlaced(id))
        {
            furnitureCheckBtn.SetActive(true);
        }
    }
    public void SettingData(Furniture getData)
    {
        //box의 가구 값 셋팅
        furnitureData = getData;
    }
    public void FurnitureBoxClick()
    {
        //box 클릭해 가구 생성

        if (FurnitureManager.Instance.FurnitureIsPlaced(furnitureData.furnitureId))
        {
            //설치되어있는 가구의 box를 클릭
            GameObject findFurniture = FurnitureManager.Instance.FindFurniture(furnitureData.furnitureId);
            if (findFurniture != null) {
                findFurniture.GetComponent<FurnitureDragHandler>().StartSetting();
            }
        }
        else
        {
            //설치 안된 가구의 box클릭
            GameObject furniturePrefab = furnitureData.FurniturePrefab;
            GameObject furnitureObj = Instantiate(furniturePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            furnitureObj.transform.SetParent(furnitureParent.transform, false);
            furnitureObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furnitureData);
            
        }
        furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
