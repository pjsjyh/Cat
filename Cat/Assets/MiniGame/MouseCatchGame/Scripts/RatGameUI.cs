using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RatGameUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RatGameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;



    private void Start()
    {
        if (gameManager != null)
        {
            // 이벤트 구독
            gameManager.OnScoreChanged += UpdateScore;
            gameManager.OnTimeChanged += UpdateTime;
            gameManager.OnGamePaused += OnGamePaused;
            gameManager.OnGameResumed += OnGameResumed;
            gameManager.OnGameEnded += OnGameEnded;
        }

        // 버튼 이벤트
        if (pauseButton != null)
            pauseButton.onClick.AddListener(() => gameManager?.PauseMiniGame());

        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => gameManager?.ResumeMiniGame());

        // 초기 상태
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnScoreChanged -= UpdateScore;
            gameManager.OnTimeChanged -= UpdateTime;
            gameManager.OnGamePaused -= OnGamePaused;
            gameManager.OnGameResumed -= OnGameResumed;
            gameManager.OnGameEnded -= OnGameEnded;
        }
    }

    private void UpdateScore(int score)
    {
        Debug.Log(scoreText);
        if (scoreText != null)
            scoreText.text = score.ToString("D3");
    }

    private void UpdateTime(float time)
    {
        if (timeText != null)
            timeText.text = Mathf.Ceil(time).ToString();
    }

    private void OnGamePaused()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    private void OnGameResumed()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void OnGameEnded(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "" + finalScore;
        }
    }

    // 외부에서 미니게임 시작
    public void StartMiniGame()
    {
        if (gameManager != null)
        {
            gameManager.StartMiniGame();
        }
    }

    // 외부에서 미니게임 종료
    public void EndMiniGame()
    {
        if (gameManager != null)
        {
            gameManager.EndMiniGame();
        }
    }
}