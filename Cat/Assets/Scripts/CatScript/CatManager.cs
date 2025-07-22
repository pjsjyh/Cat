using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance { get; private set; }
    public GameObject catPrefab; // ����� ������
    public Transform catParent; // ����̰� �߰� �� �θ�
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
