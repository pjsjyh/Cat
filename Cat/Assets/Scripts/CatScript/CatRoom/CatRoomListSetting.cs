using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatRoomListSetting : MonoBehaviour
{
    //고양이룸 고양이 리스트 셋팅
    public GameObject parentObject;
    public GameObject catListBox;

    private readonly Dictionary<string, GameObject> _catItemList = new();
    private string _firstKey;
    private void Awake()
    {
    }
    private void OnEnable() => SettingCatListBox();
    public void SettingCatListBox()
    {
        var allCats = Resources.LoadAll<Cat>("Data/Cat")
        .OrderBy(c => c.catId) // 원하는 정렬 기준
        .ToList();
        var seen = new HashSet<string>();
        _firstKey = allCats.Count > 0 ? allCats[0].catId : null;

        foreach (Cat cat in allCats) {
            var key = cat.catId;
            seen.Add(key);

            if (!_catItemList.TryGetValue(key, out var box) || box == null)
            {
                box = Instantiate(catListBox, parentObject.transform);
                _catItemList[key] = box;          // ⭐ 딕셔너리에 반드시 등록
            }

            box.GetComponent<CatRoomCatBoxItem>().SettingCatData(cat); // 내용 갱신
        }

    }
    public void SettingFirstData()
    {
        if(_catItemList.TryGetValue(_firstKey, out var box))
        {
            box.GetComponent<CatRoomCatBoxItem>().OnClickBox();
        }
    }
    
}
