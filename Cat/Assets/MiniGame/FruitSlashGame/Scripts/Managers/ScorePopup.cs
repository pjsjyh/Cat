// ScorePopup.cs - 점수 표시 효과
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public static ScorePopup Instance { get; private set; }

    [Header("Score Popup")]
    public GameObject scorePopupPrefab;
    public Canvas uiCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowScore(int score, Vector3 worldPosition)
    {
        // Debug.Log(score);
        if (scorePopupPrefab != null && uiCanvas != null)
        {
            GameObject popup = Instantiate(scorePopupPrefab, uiCanvas.transform);

            // 월드 좌표를 스크린 좌표로 변환
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            popup.transform.position = screenPos;

            Text scoreText = popup.GetComponent<Text>();
            if (scoreText != null)
            {
                scoreText.text = $"+{score}!";
            }

            StartCoroutine(AnimateScorePopup(popup));
        }
    }

    private IEnumerator AnimateScorePopup(GameObject popup)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = popup.transform.position;
        Vector3 endPos = startPos + Vector3.up * 100f;

        TextMeshProUGUI text = popup.GetComponent<TextMeshProUGUI>();
        Color startColor = text.color;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;

            popup.transform.position = Vector3.Lerp(startPos, endPos, progress);
            text.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), progress);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(popup);
    }
}