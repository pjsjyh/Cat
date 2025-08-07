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
                    catSettingWord.text = "����濡 ��ġ�� ����� �Դϴ�. \n  ";
                }
                else
                {
                    catSettingWord.text = "�ٸ��濡 ��ġ�� ����� �Դϴ�. \n  ���� ������ �̵��Ͻðڽ��ϱ�?";
                }
                    
            }
            else
            {
                catSettingWord.text = "����̸� ��������� �̵��Ͻðڽ��ϱ�? \n  ";
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
        //box Ŭ���� ���� ����
        if (CatManager.Instance.CatIsPlaced(catData.catId))
        {
            //��ġ�Ǿ��ִ� ������ box�� Ŭ��
            GameObject findCat = CatManager.Instance.FindCat(catData.catId);
            if (findCat != null)
            {
               // findCat.GetComponent<FurnitureDragHandler>().StartSetting();
            }
        }
        else
        {
            //��ġ �ȵ� ������ boxŬ��
            GameObject catPrefab = catData.catPrefab;
            GameObject catObj = Instantiate(catPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            catObj.transform.SetParent(CatInfo.Instance.catParent.transform, false);
            catObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            catObj.GetComponent<CatHandler>().SettingCatData(catData);

        }
        //furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
