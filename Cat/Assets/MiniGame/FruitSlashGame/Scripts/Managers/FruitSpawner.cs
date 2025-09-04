using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public FruitData[] fruitDatas;
    public GameObject fruitPrefab;
    public Transform spawnParent; // 빈 GameObject를 만들어서 할당 (선택사항)

    [Header("Spawn Weights - 생성 비율 조절")]
    [Range(0f, 100f)]
    public float normalFruitWeight = 70f;    // 일반 과일 70%
    [Range(0f, 100f)]
    public float bombWeight = 15f;           // 폭탄 15%
    [Range(0f, 100f)]
    public float specialFruitWeight = 15f;   // 특별한 과일 15%

    [Header("Spawn Timing")]
    public float baseSpawnInterval = 1.5f;
    public float feverSpawnInterval = 0.5f;
    public int maxFruitsOnScreen = 8;

    [Header("Physics - 포물선 조정")]
    public float minForce = 6f;      // 힘 살짝 증가
    public float maxForce = 9f;      // 힘 살짝 증가  
    public float minTorque = -2f;
    public float maxTorque = 2f;
    public float horizontalVariation = 1.5f;

    [Header("Spawn Position")]
    public float spawnYOffset = -1.2f; // 스폰 Y 위치 조정

    private Camera mainCamera;
    private List<GameObject> activeFruits = new List<GameObject>();
    private Coroutine spawnCoroutine;

    // 각 타입별로 분류된 과일 데이터
    private List<FruitData> normalFruits = new List<FruitData>();
    private List<FruitData> bombs = new List<FruitData>();
    private List<FruitData> specialFruits = new List<FruitData>();

    private void Start()
    {
        mainCamera = Camera.main;
        ClassifyFruits();
        StartSpawning();
    }

    private void ClassifyFruits()
    {
        // 과일들을 타입별로 분류
        normalFruits.Clear();
        bombs.Clear();
        specialFruits.Clear();

        foreach (FruitData fruit in fruitDatas)
        {
            if (fruit.isBomb)
            {
                bombs.Add(fruit);
            }
            else if (fruit.isSpecial)
            {
                specialFruits.Add(fruit);
            }
            else
            {
                normalFruits.Add(fruit);
            }
        }

        // 디버그 출력
        // Debug.Log($"Normal Fruits: {normalFruits.Count}, Bombs: {bombs.Count}, Special Fruits: {specialFruits.Count}");
    }

    private void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float interval = FruitGameManager.Instance.IsFeverMode ? feverSpawnInterval : baseSpawnInterval;
            yield return new WaitForSeconds(interval);

            if (activeFruits.Count < maxFruitsOnScreen)
            {
                SpawnFruit();
            }
        }
    }

    private void SpawnFruit()
    {
        // 카메라 기준 스폰 위치 계산
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // 화면 아래쪽에서 스폰 (보이는 위치에서 시작)
        Vector3 spawnPos = new Vector3(
            Random.Range(-cameraWidth / 2 + 1f, cameraWidth / 2 - 1f),
            -cameraHeight / 2 + spawnYOffset,  // 조정 가능한 오프셋 사용
            0
        );

        GameObject fruit = FruitPool.Instance.GetFruit();
        fruit.transform.position = spawnPos;

        // spawnParent가 할당되었다면 부모 설정
        if (spawnParent != null)
        {
            fruit.transform.SetParent(spawnParent);
        }

        // 가중치를 기반으로 과일 타입 선택
        FruitData selectedFruit = SelectFruitByWeight();
        Fruit fruitScript = fruit.GetComponent<Fruit>();
        fruitScript.Initialize(selectedFruit);

        // 물리력 적용 - 포물선 운동을 위한 조정
        Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();

        // 중력 설정 - 곡률에서 머무르게 하기 위해 중력을 더 약하게
        rb.gravityScale = 0.5f;  // 0.8f에서 0.5f로 더 줄임

        // 포물선을 위한 힘 계산
        float forceX = Random.Range(-horizontalVariation, horizontalVariation);
        float forceY = Random.Range(minForce, maxForce);

        // 힘 적용 (Impulse 모드로 순간적인 힘)
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(minTorque, maxTorque));

        activeFruits.Add(fruit);

        // 디버그용 - 스폰 위치 및 타입 확인
        string fruitType = selectedFruit.isBomb ? "Bomb" : (selectedFruit.isSpecial ? "Special" : "Normal");
        // Debug.Log($"{fruitType} fruit spawned at: {spawnPos}, Force: ({forceX}, {forceY})");
    }

    private FruitData SelectFruitByWeight()
    {
        // 총 가중치 계산
        float totalWeight = normalFruitWeight + bombWeight + specialFruitWeight;

        // 0~totalWeight 사이의 랜덤 값
        float randomValue = Random.Range(0f, totalWeight);

        // 가중치에 따라 타입 선택
        if (randomValue <= normalFruitWeight && normalFruits.Count > 0)
        {
            // 일반 과일
            return normalFruits[Random.Range(0, normalFruits.Count)];
        }
        else if (randomValue <= normalFruitWeight + bombWeight && bombs.Count > 0)
        {
            // 폭탄
            return bombs[Random.Range(0, bombs.Count)];
        }
        else if (specialFruits.Count > 0)
        {
            // 특별한 과일
            return specialFruits[Random.Range(0, specialFruits.Count)];
        }

        // 만약 선택된 타입에 과일이 없다면 일반 과일 반환
        if (normalFruits.Count > 0)
        {
            return normalFruits[Random.Range(0, normalFruits.Count)];
        }

        // 아무것도 없다면 첫 번째 과일 반환
        return fruitDatas[0];
    }

    public void RemoveFruit(GameObject fruit)
    {
        activeFruits.Remove(fruit);
    }

    // 런타임에서 가중치 조절을 위한 메서드들
    public void SetNormalFruitWeight(float weight)
    {
        normalFruitWeight = Mathf.Clamp(weight, 0f, 100f);
    }

    public void SetBombWeight(float weight)
    {
        bombWeight = Mathf.Clamp(weight, 0f, 100f);
    }

    public void SetSpecialFruitWeight(float weight)
    {
        specialFruitWeight = Mathf.Clamp(weight, 0f, 100f);
    }

    // 디버그용 - 현재 가중치 출력
    [ContextMenu("Show Current Weights")]
    public void ShowCurrentWeights()
    {
        float totalWeight = normalFruitWeight + bombWeight + specialFruitWeight;
        // Debug.Log($"Current Weights - Normal: {normalFruitWeight / totalWeight * 100:F1}%, " +
        //          $"Bomb: {bombWeight / totalWeight * 100:F1}%, " +
        //          $"Special: {specialFruitWeight / totalWeight * 100:F1}%");
    }
}