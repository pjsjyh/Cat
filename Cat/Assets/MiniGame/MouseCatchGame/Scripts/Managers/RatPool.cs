using UnityEngine;
using System.Collections.Generic;

public class RatPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<RatController> ratPool = new Queue<RatController>();
    private List<RatController> activeRats = new List<RatController>();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ratObj = Instantiate(ratPrefab, transform);
            RatController rat = ratObj.GetComponent<RatController>();
            ratObj.SetActive(false);
            ratPool.Enqueue(rat);
        }
    }

    public RatController GetRat()
    {
        RatController rat;

        if (ratPool.Count > 0)
        {
            rat = ratPool.Dequeue();
        }
        else
        {
            GameObject newRatObj = Instantiate(ratPrefab, transform);
            rat = newRatObj.GetComponent<RatController>();
        }

        rat.gameObject.SetActive(true);
        activeRats.Add(rat);
        return rat;
    }

    public void ReturnRat(RatController rat)
    {
        if (activeRats.Contains(rat))
        {
            activeRats.Remove(rat);
            rat.gameObject.SetActive(false);
            ratPool.Enqueue(rat);
        }
    }

    // 모든 활성 쥐를 풀로 반환
    public void ReturnAllRats()
    {
        for (int i = activeRats.Count - 1; i >= 0; i--)
        {
            ReturnRat(activeRats[i]);
        }
    }
}