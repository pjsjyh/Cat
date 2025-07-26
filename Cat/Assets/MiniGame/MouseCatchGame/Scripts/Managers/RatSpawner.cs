using UnityEngine;

// 7. 쥐 스포너
public class RatSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private RatData normalRatData;
    [SerializeField] private RatData bombRatData;
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Spawn Probability")]
    [Range(0f, 1f)]
    [SerializeField] private float bombRatProbability = 0.2f; // 20% 확률로 폭탄쥐

    private void Start()
    {
        StartCoroutine(SpawnRats());
    }

    private System.Collections.IEnumerator SpawnRats()
    {
        while (GameManager.Instance.IsGamePlaying)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomRat();
        }
    }

    private void SpawnRandomRat()
    {
        // 랜덤 위치 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 쥐 타입 결정
        RatData selectedRatData = Random.value < bombRatProbability ? bombRatData : normalRatData;

        // 쥐 생성
        GameObject ratObj = RatPool.Instance.GetRat(selectedRatData.ratType);
        ratObj.transform.position = spawnPoint.position;

        // 쥐 초기화
        BaseRat rat = ratObj.GetComponent<BaseRat>();
        // rat에 ratData 할당하는 로직 추가 필요
    }
}