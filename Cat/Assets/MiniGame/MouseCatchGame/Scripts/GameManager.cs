using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 60f; // 게임 시간 (초)
    [SerializeField] private int initialScore = 0;

    [Header("Game State")]
    public bool IsGamePlaying { get; private set; } = false;
    public float CurrentTime { get; private set; }
    public int CurrentScore { get; private set; }

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 이벤트 구독
        GameEvents.OnRatCaught += HandleRatCaught;
        GameEvents.OnBombExploded += HandleBombExploded;

        // 게임 시작
        StartGame();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        GameEvents.OnRatCaught -= HandleRatCaught;
        GameEvents.OnBombExploded -= HandleBombExploded;
    }

    public void StartGame()
    {
        IsGamePlaying = true;
        CurrentTime = gameTime;
        CurrentScore = initialScore;

        // UI 업데이트
        GameEvents.OnScoreChanged?.Invoke(CurrentScore);
        GameEvents.OnTimeChanged?.Invoke(CurrentTime);

        // 타이머 시작
        StartCoroutine(GameTimer());
    }

    private System.Collections.IEnumerator GameTimer()
    {
        while (CurrentTime > 0 && IsGamePlaying)
        {
            yield return new WaitForSeconds(1f);
            CurrentTime--;
            GameEvents.OnTimeChanged?.Invoke(CurrentTime);
        }

        // 게임 종료
        EndGame();
    }

    public void EndGame()
    {
        IsGamePlaying = false;
        Debug.Log($"게임 종료! 최종 점수: {CurrentScore}");
        // 게임 오버 UI 표시 등
    }

    private void HandleRatCaught(RatType ratType, int score)
    {
        CurrentScore += score;
        GameEvents.OnScoreChanged?.Invoke(CurrentScore);
        Debug.Log($"{ratType} 쥐 잡음! 점수: +{score}, 총점: {CurrentScore}");
    }

    private void HandleBombExploded(int scorePenalty, float timePenalty)
    {
        CurrentScore += scorePenalty; // scorePenalty는 음수값
        CurrentTime -= timePenalty;

        // 시간이 0 이하로 떨어지지 않도록
        if (CurrentTime < 0) CurrentTime = 0;

        GameEvents.OnScoreChanged?.Invoke(CurrentScore);
        GameEvents.OnTimeChanged?.Invoke(CurrentTime);

        Debug.Log($"폭탄 터짐! 점수: {scorePenalty}, 시간: -{timePenalty}, 총점: {CurrentScore}");
    }

    // 게임 일시정지/재개
    public void PauseGame() => IsGamePlaying = false;
    public void ResumeGame() => IsGamePlaying = true;
}
