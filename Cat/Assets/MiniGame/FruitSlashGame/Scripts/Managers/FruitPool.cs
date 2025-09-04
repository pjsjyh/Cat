// FruitPool.cs - 오브젝트 풀링
using UnityEngine;
using System.Collections.Generic;

public class FruitPool : MonoBehaviour
{
    public static FruitPool Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject fruitPrefab;
    public int poolSize = 20;

    private Queue<GameObject> fruitPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fruit = Instantiate(fruitPrefab);
            fruit.SetActive(false);
            fruitPool.Enqueue(fruit);
        }
    }

    public GameObject GetFruit()
    {
        if (fruitPool.Count > 0)
        {
            GameObject fruit = fruitPool.Dequeue();
            fruit.SetActive(true);
            return fruit;
        }
        else
        {
            return Instantiate(fruitPrefab);
        }
    }

    public void ReturnFruit(GameObject fruit)
    {
        fruit.SetActive(false);
        fruitPool.Enqueue(fruit);
    }
}