using TMPro;
using UnityEngine;

public class MainCatSetting : MonoBehaviour
{
    private Cat catData;
    [SerializeField] private TextMeshProUGUI catSettingWord;

    public void OnEnable()
    {
        if (catData != null) {
            int nowPlayerPlaced = PlayerDataManager.Instance.ReturnPlayerPlace();
            if (catData.isPlaced)
            {
                if(nowPlayerPlaced == catData.installLocation)
                {
                    catSettingWord.text = "현재방에 위치한 고양이 입니다. \n  ";
                }
                else
                {
                    catSettingWord.text = "다른방에 위치한 고양이 입니다. \n  현재 방으로 이동하시겠습니까?";
                }
                    
            }
            else
            {
                catSettingWord.text = "고양이를 현재방으로 이동하시겠습니까? \n  ";
            }
        }
    }
    public void SettingData(Cat getData)
    {
        catData = getData;
        this.gameObject.SetActive(true);
    }
    public void CatBoxClick()
    {
        //box 클릭해 가구 생성
        if (CatManager.Instance.CatIsPlaced(catData.catId))
        {
            //설치되어있는 가구의 box를 클릭
            GameObject findCat = CatManager.Instance.FindCat(catData.catId);
            if (findCat != null)
            {
               // findCat.GetComponent<FurnitureDragHandler>().StartSetting();
            }
        }
        else
        {
            //설치 안된 가구의 box클릭
            GameObject catPrefab = catData.catPrefab;
            GameObject catObj = Instantiate(catPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            catObj.transform.SetParent(CatInfo.Instance.catParent.transform, false);
            catObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            catObj.GetComponent<CatHandler>().SettingCatData(catData);

        }
        //furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
