using UnityEngine;
using System.Collections.Generic;
// 8. 간단한 오브젝트 풀
public class RatPool : MonoBehaviour
{
    public static RatPool Instance;

    [Header("Pool Settings")]
    [SerializeField] private GameObject normalRatPrefab;
    [SerializeField] private GameObject bombRatPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> normalRatPool = new Queue<GameObject>();
    private Queue<GameObject> bombRatPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        // 일반쥐 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rat = Instantiate(normalRatPrefab);
            rat.SetActive(false);
            normalRatPool.Enqueue(rat);
        }

        // 폭탄쥐 풀 초기화
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rat = Instantiate(bombRatPrefab);
            rat.SetActive(false);
            bombRatPool.Enqueue(rat);
        }
    }

    public GameObject GetRat(RatType ratType)
    {
        Queue<GameObject> targetPool = ratType == RatType.Normal ? normalRatPool : bombRatPool;

        if (targetPool.Count > 0)
        {
            GameObject rat = targetPool.Dequeue();
            rat.SetActive(true);
            return rat;
        }

        // 풀이 비어있으면 새로 생성
        GameObject prefab = ratType == RatType.Normal ? normalRatPrefab : bombRatPrefab;
        return Instantiate(prefab);
    }

    public void ReturnRat(BaseRat rat)
    {
        rat.gameObject.SetActive(false);

        if (rat is NormalRat)
        {
            normalRatPool.Enqueue(rat.gameObject);
        }
        else if (rat is BombRat)
        {
            bombRatPool.Enqueue(rat.gameObject);
        }
    }
}