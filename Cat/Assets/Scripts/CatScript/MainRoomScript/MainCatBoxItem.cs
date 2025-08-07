using UnityEngine;

public class MainCatBoxItem : MonoBehaviour
{
    //mainroom cat box ��ũ��Ʈ.
    private GameObject catParent;
    private GameObject catSliding;

    [SerializeField]
    private GameObject catCheckBtn;
    private GameObject checkPanel; //����� ���⿡ ��ġ���� ����� �ǳ�

    private Cat catData;

    private void Awake()
    {
        catParent = CatInfo.Instance.catParent;
        catSliding = CatInfo.Instance.catSliding;
        checkPanel = CatInfo.Instance.checkPanel;
    }

    public void CheckIsPlaced(string id)
    {
        //ȭ�鿡 ��ġ �� ���� ���� Ȯ��
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
        //box�� ���� �� ����
        catData = getData;
    }
    public void CatBoxClick()
    {
        //box Ŭ���� ����� ����
        CatInfo.Instance.CatSettingOn(catData);
        //if (CatManager.Instance.CatIsPlaced(catData.catId))
        //{
        //    //��ġ�Ǿ��ִ� ������ box�� Ŭ��
        //    GameObject findCat = CatManager.Instance.FindCat(catData.catId);
        //    if (findCat != null)
        //    {
        //        //findCat.GetComponent<CatHandler>().StartSetting();
        //    }
        //}
        //else
        //{
        //    //��ġ �ȵ� ������ boxŬ��
        //    GameObject catPrefab = catData.catPrefab;
        //    GameObject catObj = Instantiate(catPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        //    catObj.transform.SetParent(catParent.transform, false);
        //    catObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //    catObj.GetComponent<CatHandler>().SettingCatData(catData);

        //}
        //furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
