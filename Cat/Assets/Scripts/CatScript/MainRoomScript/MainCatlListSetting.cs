using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainCatlListSetting : MonoBehaviour
{
    //메인룸 고양이 리스트 셋팅
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
            //box list에 포함되는지 확인
            if (CatInfo.Instance.FindCatBoxInList(matched.catId))
            {
                box = CatInfo.Instance.FindCatBox(matched.catId);
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
                CatInfo.Instance.AddCatBoxList(matched.catId, box);
                RawImage image = grandChild.GetComponent<RawImage>();
                image.texture = matched.CatThumbnail.texture;
                box.GetComponent<MainCatBoxItem>().SettingData(matched);
                box.GetComponent<MainCatBoxItem>().CheckIsPlaced(matched.catId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[고양이 셋팅 에러] 예외 발생: {ex.GetType().Name} - {ex.Message}\n스택트레이스: {ex.StackTrace}");
            }
        }
    }
}
