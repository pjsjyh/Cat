// GameManager.cs - 게임 전체 관리
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class FruitGameManager : MonoBehaviour
{
    public static FruitGameManager Instance { get; private set; }

    [Header("Game UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public Slider feverSlider;
    public TextMeshProUGUI feverText;
    public TextMeshProUGUI timerText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI bestScoreText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Pause UI")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button pauseRestartButton;
    public Button pauseMainMenuButton;

    [Header("Game Settings")]
    public float gameTime = 60f;
    public float feverThreshold = 100f;
    public float feverDuration = 10f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sliceSound;
    public AudioClip bombSound;
    public AudioClip comboSound;
    public AudioClip gameOverSound;

    private int score = 0;
    private int combo = 0;
    private float feverMeter = 0f;
    private bool isFeverMode = false;
    private float currentGameTime;
    private bool isGameOver = false;
    private bool isPaused = false;

    public int Score => score;
    public int Combo => combo;
    public bool IsFeverMode => isFeverMode;
    public bool IsGameOver => isGameOver;
    public bool IsPaused => isPaused;
    private SlashController slashController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentGameTime = gameTime;
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // SlashController 찾기
        slashController = FindObjectOfType<SlashController>();
        if (slashController == null)
        {
            Debug.LogWarning("slashController not found! Pause/Resume blade control will not work.");
        }

        UpdateUI();

        // 게임 시작 시 패널들 비활성화
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    private void SetupUI()
    {
        // 버튼 이벤트 연결
        if (restartButton) restartButton.onClick.AddListener(RestartGame);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (pauseButton) pauseButton.onClick.AddListener(PauseGame);
        if (resumeButton) resumeButton.onClick.AddListener(ResumeGame);
        if (pauseRestartButton) pauseRestartButton.onClick.AddListener(RestartGame);
        if (pauseMainMenuButton) pauseMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Update()
    {
        // ESC 키로 일시정지/재개
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGameOver)
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }

        // 게임이 일시정지되었거나 종료되었으면 시간 업데이트 안함
        if (isPaused || isGameOver) return;

        // 게임 시간 감소
        currentGameTime -= Time.deltaTime;
        if (currentGameTime <= 0 && !isGameOver)
        {
            EndGame();
        }

        UpdateUI();
    }

    public void AddScore(int points, Vector3 position)
    {
        if (isGameOver || isPaused) return;

        int multiplier = isFeverMode ? 2 : 1; // 피버 모드에서 2배 점수
        int finalScore = points * (combo + 1) * multiplier;
        score += finalScore;

        // 점수 표시 효과
        ScorePopup.Instance?.ShowScore(finalScore, position);

        UpdateUI();
        audioSource?.PlayOneShot(sliceSound);
    }

    public void AddCombo()
    {
        if (isGameOver || isPaused) return;

        combo++;
        feverMeter += 10f;

        if (feverMeter >= feverThreshold && !isFeverMode)
        {
            StartFeverMode();
        }

        UpdateUI();

        if (combo > 1)
        {
            audioSource?.PlayOneShot(comboSound);
        }
    }

    public void ResetCombo()
    {
        if (isGameOver) return;

        combo = 0;
        UpdateUI();
    }

    public void OnBombHit()
    {
        if (isGameOver || isPaused) return;

        // 시간과 피버 감소
        currentGameTime -= 5f;
        feverMeter = Mathf.Max(0, feverMeter - 30f);
        combo = 0; // 콤보 리셋

        // 화면 흔들기
        CameraShake.Instance?.ShakeCamera(0.5f, 0.3f);

        audioSource?.PlayOneShot(bombSound);
        UpdateUI();
    }

    private void StartFeverMode()
    {
        isFeverMode = true;
        feverMeter = feverThreshold;
        StartCoroutine(FeverModeCoroutine());
    }

    private IEnumerator FeverModeCoroutine()
    {
        float timer = feverDuration;
        while (timer > 0 && !isGameOver && !isPaused)
        {
            timer -= Time.deltaTime;
            feverMeter = Mathf.Lerp(0, feverThreshold, timer / feverDuration);
            UpdateUI();
            yield return null;
        }

        if (!isGameOver)
        {
            isFeverMode = false;
            feverMeter = 0;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (scoreText) scoreText.text = $"Score: {score:N0}";
        if (comboText) comboText.text = combo > 1 ? $"Combo x{combo}" : "";
        if (feverSlider) feverSlider.value = feverMeter / feverThreshold;
        if (feverText) feverText.text = isFeverMode ? "FEVER!" : "";
        if (timerText)
        {
            int minutes = Mathf.FloorToInt(currentGameTime / 60);
            int seconds = Mathf.FloorToInt(currentGameTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        // 최고 점수 확인 및 저장
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScore = score;
        }

        // Game Over UI 표시
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText) finalScoreText.text = $"Final Score: {score:N0}";
            if (bestScoreText) bestScoreText.text = $"Best Score: {bestScore:N0}";
        }

        // 게임 오버 사운드 재생
        if (audioSource && gameOverSound)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        Debug.Log($"Game Over! Final Score: {score}");
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel) pausePanel.SetActive(true);
        if (pauseButton) pauseButton.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        if (isGameOver) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel) pausePanel.SetActive(false);
        if (pauseButton) pauseButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // 메인 메뉴 씬이 있다면 해당 씬으로 이동
        // SceneManager.LoadScene("MainMenu");

        // 또는 현재 씬을 재시작
        SceneManager.LoadScene(0); // Build Settings에서 첫 번째 씬
    }

    // 외부에서 호출할 수 있는 유틸리티 메서드들
    public void AddTime(float timeToAdd)
    {
        if (!isGameOver)
        {
            currentGameTime += timeToAdd;
        }
    }

    public void AddFeverMeter(float feverToAdd)
    {
        if (!isGameOver && !isPaused)
        {
            feverMeter = Mathf.Min(feverThreshold, feverMeter + feverToAdd);

            if (feverMeter >= feverThreshold && !isFeverMode)
            {
                StartFeverMode();
            }
        }
    }

    // 디버그용 - 치트 코드
    [ContextMenu("Add 1000 Score")]
    public void DebugAddScore()
    {
        AddScore(1000, Vector3.zero);
    }

    [ContextMenu("Add 10 Seconds")]
    public void DebugAddTime()
    {
        AddTime(10f);
    }

    [ContextMenu("Start Fever Mode")]
    public void DebugStartFever()
    {
        if (!isFeverMode)
        {
            StartFeverMode();
        }
    }
}