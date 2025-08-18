using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CatManager : MonoBehaviour
{
    //ó�� ����� ����� ���� �Ŵ���
    public static CatManager Instance { get; private set; }
    public Transform catParent; // ����̰� �߰� �� �θ�

    private Dictionary<string, GameObject> placedCat = new();
    private Dictionary<string, CatSaveData> catSaveData = new();

    public Dictionary<string, Cat> allCatData = new(); //��� ����� ������

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
        //����̸� ��ġ ����Ʈ�� �߰�
        placedCat[getId] = getObj;
    }

    public bool CatIsPlaced(string getId)
    {
        //���� ��ġ ���� Ȯ��
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
        //����� �ٹ̱� ���� �� ������ ����
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

            // ����Ʈ���� ���� id ã��
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
