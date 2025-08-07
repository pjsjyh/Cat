using UnityEngine;

public class MainCatBoxItem : MonoBehaviour
{
    //mainroom cat box 스크립트.
    private GameObject catParent;
    private GameObject catSliding;

    [SerializeField]
    private GameObject catCheckBtn;
    private GameObject checkPanel; //고양이 여기에 설치할지 물어보는 판넬

    private Cat catData;

    private void Awake()
    {
        catParent = CatInfo.Instance.catParent;
        catSliding = CatInfo.Instance.catSliding;
        checkPanel = CatInfo.Instance.checkPanel;
    }

    public void CheckIsPlaced(string id)
    {
        //화면에 설치 된 가구 인지 확인
        if (CatManager.Instance.CatIsPlaced(id))
        {
            catCheckBtn.SetActive(true);
        }
    }
    public void RemoveCatCheck()
    {
        catCheckBtn.SetActive(false);
    }
    public void SettingData(Cat getData)
    {
        //box의 가구 값 셋팅
        catData = getData;
    }
    public void CatBoxClick()
    {
        //box 클릭해 고양이 생성
        CatInfo.Instance.CatSettingOn(catData);
        //if (CatManager.Instance.CatIsPlaced(catData.catId))
        //{
        //    //설치되어있는 가구의 box를 클릭
        //    GameObject findCat = CatManager.Instance.FindCat(catData.catId);
        //    if (findCat != null)
        //    {
        //        //findCat.GetComponent<CatHandler>().StartSetting();
        //    }
        //}
        //else
        //{
        //    //설치 안된 가구의 box클릭
        //    GameObject catPrefab = catData.catPrefab;
        //    GameObject catObj = Instantiate(catPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //    catObj.transform.SetParent(catParent.transform, false);
        //    catObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //    catObj.GetComponent<CatHandler>().SettingCatData(catData);

        //}
        //furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
