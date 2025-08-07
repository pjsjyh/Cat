using UnityEngine;

public class CatRoomListSetting : MonoBehaviour
{
    //고양이룸 고양이 리스트 셋팅
    public GameObject parentObject;
    public GameObject catListBox;
    private void Awake()
    {
    }
    private void OnEnable()
    {
        SettingCatListBox();
    }
    public void SettingCatListBox()
    {
        Cat[] allCats = Resources.LoadAll<Cat>("Data/Cat");
        foreach (Cat cat in allCats) {
            GameObject box = Instantiate(catListBox, parentObject.transform);
        }
        
    }
}
