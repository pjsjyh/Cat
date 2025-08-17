using UnityEngine;

public class CatRoomCatBoxItem : MonoBehaviour
{
    private Cat catData;

    public void SettingCatData(Cat getData)
    {
        catData = getData;
    }
    public void OnClickBox()
    {
        GameObject parentObj = this.gameObject.transform.parent.gameObject;
        parentObj.GetComponent<CatRoomCatInfoSetting>().ChangeCatInfo(catData);
    }
}
