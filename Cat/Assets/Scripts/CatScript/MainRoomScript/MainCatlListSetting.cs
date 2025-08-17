using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainCatlListSetting : MonoBehaviour
{
    //���η� ����� ����Ʈ ����
    public GameObject parentObject;
    public GameObject furnitureListBox;
    void Start()
    {

    }

    private void OnEnable()
    {
        Debug.Log("!!!!");
        SettingCatListBox();
    }
    public void SettingCatListBox()
    {
        Cat[] allCat = Resources.LoadAll<Cat>("Data/Cat");
        Debug.Log(allCat.Length);
        Debug.Log(PlayerDataManager.Instance.playerData.catData.catDataList.Count);
        foreach (CatSaveData cat in PlayerDataManager.Instance.playerData.catData.catDataList)
        {
            Debug.Log(cat.id);
            Cat matched = allCat.FirstOrDefault(f => f.catId == cat.id);
            GameObject box;
            //box list�� ���ԵǴ��� Ȯ��
            if (CatInfo.Instance.FindCatBoxInList(matched.catId))
            {
                box = CatInfo.Instance.FindCatBox(matched.catId);
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
                CatInfo.Instance.AddCatBoxList(matched.catId, box);
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = matched.CatThumbnail.texture;
                box.GetComponent<MainCatBoxItem>().SettingData(matched);
                box.GetComponent<MainCatBoxItem>().CheckIsPlaced(matched.catId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[����� ���� ����] ���� �߻�: {ex.GetType().Name} - {ex.Message}\n����Ʈ���̽�: {ex.StackTrace}");
            }
        }
    }
}
