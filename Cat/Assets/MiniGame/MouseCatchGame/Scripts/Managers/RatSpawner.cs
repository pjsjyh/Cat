using UnityEngine;
using System.Collections;

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

    public void Initialize(RatGameManager manager)
    {
        gameManager = manager;
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

        Transform selectedHole = holePositions[Random.Range(0, holePositions.Length)];

        RatData selectedRatData;
        // if (Random.value < bombRatProbability)
        // {
        //     selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Bomb);
        //     if (selectedRatData == null) selectedRatData = ratDataArray[0];
        // }
        // else
        // {
        //     selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Normal);
        //     if (selectedRatData == null) selectedRatData = ratDataArray[0];
        // }
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
            // rat.Initialize(gameManager);
            rat.SetupRat(selectedRatData);
        }
    }

}