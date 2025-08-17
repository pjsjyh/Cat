using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RatSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] holePositions;
    [SerializeField] private RatData[] ratDataArray;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Rat Spawn Probabilities")]
    [SerializeField] private float normalRatProbability = 0.4f;    // 40%
    [SerializeField] private float bombRatProbability = 0.2f;      // 20%
    [SerializeField] private float helmetRatProbability = 0.15f;   // 15%
    [SerializeField] private float goldenRatProbability = 0.15f;   // 15%
    [SerializeField] private float shieldRatProbability = 0.1f;    // 10%

    private RatGameManager gameManager;
    private bool isSpawning = false;
    private Coroutine spawnCoroutine;

    // 구멍 점유 상태를 추적하기 위한 딕셔너리
    private Dictionary<Transform, bool> holeOccupancy = new Dictionary<Transform, bool>();

    private void Awake()
    {
        // 모든 구멍을 비어있는 상태로 초기화
        InitializeHoleOccupancy();
    }

    private void InitializeHoleOccupancy()
    {
        holeOccupancy.Clear();
        foreach (Transform hole in holePositions)
        {
            holeOccupancy[hole] = false;
        }
    }

    public void Initialize(RatGameManager manager)
    {
        gameManager = manager;
        InitializeHoleOccupancy();
    }

    public void StartSpawning()
    {
        isSpawning = true;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    // 구멍이 비어있는지 확인하는 메서드
    public bool IsHoleEmpty(Transform hole)
    {
        return holeOccupancy.ContainsKey(hole) && !holeOccupancy[hole];
    }

    // 구멍 점유 상태를 설정하는 메서드
    public void SetHoleOccupancy(Transform hole, bool isOccupied)
    {
        if (holeOccupancy.ContainsKey(hole))
        {
            holeOccupancy[hole] = isOccupied;
        }
    }

    // 비어있는 구멍 목록을 반환하는 메서드
    private List<Transform> GetEmptyHoles()
    {
        return holeOccupancy.Where(pair => !pair.Value).Select(pair => pair.Key).ToList();
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning && (gameManager == null || gameManager.IsGamePlaying))
        {
            if (gameManager != null)
            {
                yield return new WaitWhile(() => gameManager.IsPaused);
            }

            yield return new WaitForSeconds(spawnInterval);

            if ((gameManager == null || (!gameManager.IsPaused && gameManager.IsGamePlaying)) && isSpawning)
            {
                SpawnRandomRat();
            }
        }
    }

    private void SpawnRandomRat()
    {
        if (holePositions.Length == 0 || ratDataArray.Length == 0) return;

        // 비어있는 구멍들만 가져오기
        List<Transform> emptyHoles = GetEmptyHoles();

        // 모든 구멍이 차있다면 스폰하지 않음
        if (emptyHoles.Count == 0)
        {
            return;
        }

        // 비어있는 구멍 중에서 랜덤하게 선택
        Transform selectedHole = emptyHoles[Random.Range(0, emptyHoles.Count)];

        RatData selectedRatData;
        float randomValue = Random.value;

        if (randomValue < normalRatProbability)
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Normal);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }
        else if (randomValue < normalRatProbability + bombRatProbability)
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Bomb);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }
        else if (randomValue < normalRatProbability + bombRatProbability + helmetRatProbability)
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Helmet);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }
        else if (randomValue < normalRatProbability + bombRatProbability + helmetRatProbability + goldenRatProbability)
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Golden);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }
        else
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Shield);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }

        RatPool ratPool = FindObjectOfType<RatPool>();
        if (ratPool != null)
        {
            RatController rat = ratPool.GetRat();
            rat.transform.position = selectedHole.position;
            rat.transform.parent = selectedHole;

            // 구멍을 점유 상태로 설정
            SetHoleOccupancy(selectedHole, true);

            rat.SetupRat(selectedRatData);
        }
    }
}