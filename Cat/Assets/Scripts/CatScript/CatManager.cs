using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatManager : MonoBehaviour
{
    //처음 사용자 고양이 셋팅 매니저
    public static CatManager Instance { get; private set; }
    public Transform catParent; // 고양이가 추가 될 부모

    private Dictionary<string, GameObject> placedCat = new();
    private Dictionary<string, CatSaveData> catSaveData = new();

    public Dictionary<string, Cat> allCatData = new(); //모든 고양이 데이터

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지되게

    }
    private void Start()
    {
        SpawnCats(PlayerDataManager.Instance.playerData.catData.catDataList);
    }
    public void SpawnCats(List<CatSaveData> catList)
    {
        foreach (var catSaveData in catList)
        {
            Cat catData = Resources.Load<Cat>($"Data/Cat/{catSaveData.id}");

            catData.isPlaced = catSaveData.isPlaced;
            catData.catCurrentPosition = catSaveData.position;
            if (catSaveData.isPlaced)
            {
                GameObject catObj = Instantiate(catData.catPrefab, catSaveData.position, Quaternion.identity, catParent);
                catObj.transform.SetParent(catParent.transform, false);

                catObj.GetComponent<CatHandler>().SettingCatData(catData);
                placedCat[catSaveData.id] = catObj;
            }

            allCatData[catSaveData.id] = catData;


        }
    }
    public void AddCat(string getId, GameObject getObj)
    {
        //고양이를 설치 리스트에 추가
        placedCat[getId] = getObj;
    }

    public bool CatIsPlaced(string getId)
    {
        //가구 설치 여부 확인
        if (placedCat.ContainsKey(getId))
        {
            return true;
        }
        return false;
    }
    public GameObject FindCat(string getId)
    {
        if (placedCat.ContainsKey(getId))
        {
            return placedCat[getId];
        }
        return null;
    }
    public void RemoveCatInPlace(string getId)
    {

        placedCat.Remove(getId);
        catSaveData.Remove(getId);
    }
    public void DataUpdateCat()
    {
        //고양이 꾸미기 종료 후 데이터 저장
        var list = PlayerDataManager.Instance.playerData.catData.catDataList;

        foreach (var cat in placedCat)
        {
            var cData = cat.Value.GetComponent<CatHandler>().ReturnCatData();
            var save = new CatSaveData
            {
                id = cData.catId,
                position = cData.catCurrentPosition,
                isPlaced = cData.isPlaced
            };

            // 리스트에서 같은 id 찾기
            var existing = list.Find(x => x.id == save.id);
            if (existing != null)
            {
                existing.position = save.position;
                existing.isPlaced = save.isPlaced;
            }
            else
            {
                list.Add(save);
            }
        }
        //PlayerDataManager.Instance.playerData.catData.catDataList = catSaveData.Values.ToList();
    }
    public List<Cat> ReturnCatList()
    {
        return allCatData.Values.ToList();
    }
}
