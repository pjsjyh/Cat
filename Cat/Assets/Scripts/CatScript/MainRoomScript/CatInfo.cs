using System.Collections.Generic;
using UnityEngine;

public class CatInfo : MonoBehaviour
{
    public GameObject catParent;
    public GameObject catSliding;
    public GameObject checkPanel;

    private Dictionary<string, GameObject> allBoxes = new();

    public void AddCatBoxList(string getId, GameObject boxObj)
    {
        //boxlist에 가구 저장.
        allBoxes[getId] = boxObj;
    }
    public bool FindCatBoxInList(string getId)
    {
        if (allBoxes.ContainsKey(getId))
        {
            return true;
        }
        return false;
    }
    public GameObject FindCatBox(string getId)
    {
        //boxlist에서 원하는 가구의 box 찾기
        if (allBoxes.ContainsKey(getId))
        {
            return allBoxes[getId];
        }
        return null;
    }
    public void CatSettingOn(Cat catData)
    {
        //box를 클릭해 고양이 셋팅 하는 코드
        checkPanel.GetComponent<MainCatSetting>().SettingData(catData);
    }
}
