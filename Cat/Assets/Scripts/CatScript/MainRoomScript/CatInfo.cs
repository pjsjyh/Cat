using System.Collections.Generic;
using UnityEngine;

public class CatInfo : MonoBehaviour
{
    public static CatInfo Instance { get; private set; }
    public GameObject catParent;
    public GameObject catSliding;
    public GameObject checkPanel;

    private Dictionary<string, GameObject> allBoxes = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ���� �ٲ� �����ǰ�
    }
    public void AddCatBoxList(string getId, GameObject boxObj)
    {
        //boxlist�� ���� ����.
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
        //boxlist���� ���ϴ� ������ box ã��
        if (allBoxes.ContainsKey(getId))
        {
            return allBoxes[getId];
        }
        return null;
    }
    public void CatSettingOn(Cat catData)
    {
        //box�� Ŭ���� ����� ���� �ϴ� �ڵ�
        checkPanel.GetComponent<MainCatSetting>().SettingData(catData);
    }
}
