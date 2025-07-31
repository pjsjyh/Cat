using UnityEngine;
using System.Collections;

public class RatSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] holePositions;
    [SerializeField] private RatData[] ratDataArray;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Spawn Probability")]
    [Range(0f, 1f)]
    [SerializeField] private float bombRatProbability = 0.2f;

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
        if (Random.value < bombRatProbability)
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Bomb);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }
        else
        {
            selectedRatData = System.Array.Find(ratDataArray, data => data.ratType == RatType.Normal);
            if (selectedRatData == null) selectedRatData = ratDataArray[0];
        }

        RatPool ratPool = FindObjectOfType<RatPool>();
        if (ratPool != null)
        {
            RatController rat = ratPool.GetRat();
            rat.transform.position = selectedHole.position;
            rat.Initialize(gameManager);
            rat.SetupRat(selectedRatData);
        }
    }
}