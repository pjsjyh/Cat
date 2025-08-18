using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameCatPick : MonoBehaviour
{
    [SerializeField]
    private GameObject CatBoxItem;
    [SerializeField]
    private GameObject CatParent;

    [SerializeField]
    private Cat PickCat;
    private void Start()
    {
        SettingCatListInGameList();
    }
    public void SettingCatListInGameList()
    {
        List<Cat> getCatList = CatManager.Instance.ReturnCatList();
        foreach (Cat cat in getCatList)
        {
            GameObject catBox = Instantiate(CatBoxItem, CatParent.transform);

        }
    }
}
