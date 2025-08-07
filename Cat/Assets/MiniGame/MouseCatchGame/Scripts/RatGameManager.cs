using UnityEngine;
using System.Collections;

public class RatGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float gameTime = 60f;
    [SerializeField] private int initialScore = 0;

    [Header("Game State")]
    public bool IsGamePlaying { get; private set; } = false;
    public bool IsPaused { get; private set; } = false;
    public float CurrentTime { get; private set; }
    public int CurrentScore { get; private set; }

    [Header("References")]
    [SerializeField] private RatSpawner ratSpawner;
    [SerializeField] private RatPool ratPool;

    // 미니게임 전용 이벤트
    public System.Action OnGamePaused;
    public System.Action OnGameResumed;
    public System.Action<int> OnScoreChanged;
    public System.Action<float> OnTimeChanged;
    public System.Action<int> OnGameEnded; // 최종 점수와 함께

    private Coroutine gameTimerCoroutine;
    private float originalTimeScale;

    private void Start()
    {
        // 원래 timeScale 저장
        originalTimeScale = Time.timeScale;

        // 이벤트 구독
        RatGameEvents.OnRatCaught += HandleRatCaught;
        RatGameEvents.OnBombExploded += HandleBombExploded;


        //게임시작
        StartMiniGame();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        RatGameEvents.OnRatCaught -= HandleRatCaught;
        RatGameEvents.OnBombExploded -= HandleBombExploded;

        // timeScale 복원
        if (IsPaused)
        {
            Time.timeScale = originalTimeScale;
        }
    }

    public void StartMiniGame()
    {
        IsGamePlaying = true;
        IsPaused = false;
        CurrentTime = gameTime;
        CurrentScore = initialScore;

        // UI 업데이트
        OnScoreChanged?.Invoke(CurrentScore);
        OnTimeChanged?.Invoke(CurrentTime);

        // 스포너 시작
        if (ratSpawner != null)
        {
            ratSpawner.StartSpawning();
        }

        // 타이머 시작
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }
        gameTimerCoroutine = StartCoroutine(GameTimer());

        Debug.Log("쥐잡기 미니게임 시작!");
    }

    public void EndMiniGame()
    {
        IsGamePlaying = false;
        IsPaused = false;

        // timeScale 복원
        Time.timeScale = originalTimeScale;

        // 스포너 정지
        if (ratSpawner != null)
        {
            ratSpawner.StopSpawning();
        }

        // 모든 쥐 회수
        if (ratPool != null)
        {
            ratPool.ReturnAllRats();
        }

        // 타이머 정지
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }

        // 게임 종료 이벤트
        OnGameEnded?.Invoke(CurrentScore);

        Debug.Log($"쥐잡기 미니게임 종료! 최종 점수: {CurrentScore}");
    }

    public void PauseMiniGame()
    {
        if (!IsGamePlaying || IsPaused) return;

        IsPaused = true;
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        OnGamePaused?.Invoke();
        Debug.Log("쥐잡기 게임 일시정지");
    }

    public void ResumeMiniGame()
    {
        if (!IsGamePlaying || !IsPaused) return;

        IsPaused = false;
        Time.timeScale = originalTimeScale;

        OnGameResumed?.Invoke();
        Debug.Log("쥐잡기 게임 재개");
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            ResumeMiniGame();
        }
        else
        {
            PauseMiniGame();
        }
    }

    private IEnumerator GameTimer()
    {
        while (CurrentTime > 0 && IsGamePlaying)
        {
            // 일시정지 중에는 대기
            yield return new WaitWhile(() => IsPaused);

            yield return new WaitForSecondsRealtime(1f);

            if (!IsPaused)
            {
                CurrentTime--;
                OnTimeChanged?.Invoke(CurrentTime);
            }
        }

        EndMiniGame();
    }

    private void HandleRatCaught(RatType ratType, int score)
    {
        CurrentScore += score;
        OnScoreChanged?.Invoke(CurrentScore);
        Debug.Log($"{ratType} 쥐 잡음! 점수: +{score}, 총점: {CurrentScore}");
    }

    private void HandleBombExploded(int scorePenalty, float timePenalty)
    {
        CurrentScore += scorePenalty;
        CurrentTime -= timePenalty;

        if (CurrentTime < 0) CurrentTime = 0;

        OnScoreChanged?.Invoke(CurrentScore);
        OnTimeChanged?.Invoke(CurrentTime);

        Debug.Log($"폭탄 터짐! 점수: {scorePenalty}, 시간: -{timePenalty}");
    }

    // 외부에서 점수나 시간을 직접 조작할 때 사용
    public void AddScore(int score)
    {
        CurrentScore += score;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void AddTime(float time)
    {
        CurrentTime += time;
        if (CurrentTime < 0) CurrentTime = 0;
        OnTimeChanged?.Invoke(CurrentTime);
    }
}