using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance { get; private set; }
    public GameObject catPrefab; // 고양이 프리팹
    public Transform catParent; // 고양이가 추가 될 부모
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
    public void SpawnCats(List<CatSaveData> catList)
    {
        foreach (var catSaveData in catList)
        {
            GameObject catObj = Instantiate(catPrefab, catSaveData.position, Quaternion.identity, catParent);
            CatController controller = catObj.GetComponent<CatController>();
            controller.Init(catSaveData);
        }
    }
}
