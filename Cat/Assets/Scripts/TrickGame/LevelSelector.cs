using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [Header("UI")]
    public Button prevButton;
    public Button nextButton;
    public TextMeshProUGUI numberText;          // UGUI Text 사용 시
    // public TMP_Text numberTMP;    // TextMeshPro 사용 시 이 필드 사용

    [Header("Range")]
    [Min(1)] public int minLevel = 1;
    [Min(1)] public int maxLevel = 50;
    public bool wrap = false;        // 끝에서 되감기(50->1 / 1->50) 여부

    public TrickGameManager gameManager;
    public GameObject LevelChoosePanel;
  
    [Header("Feedback")]
    public float bumpScale = 1.08f;
    public float bumpTime = 0.12f;

    public int CurrentLevel { get; private set; } = 1;

    void Awake()
    {
        if (maxLevel < minLevel) maxLevel = minLevel;

        prevButton?.onClick.AddListener(OnPrev);
        nextButton?.onClick.AddListener(OnNext);

        // 초기 값 보정/표시
        SetLevel(Mathf.Clamp(CurrentLevel, minLevel, maxLevel), false);
    }

    public void OnPrev()
    {
        int next = CurrentLevel - 1;
        if (next < minLevel)
            next = wrap ? maxLevel : minLevel;
        SetLevel(next, true);
    }

    public void OnNext()
    {
        int next = CurrentLevel + 1;
        if (next > maxLevel)
            next = wrap ? minLevel : maxLevel;
        SetLevel(next, true);
    }

    public void SetLevel(int level, bool playFx)
    {
        level = Mathf.Clamp(level, minLevel, maxLevel);
        if (CurrentLevel == level && !playFx) { UpdateLabel(); return; }

        CurrentLevel = level;
        UpdateLabel();

      
        if (playFx)
        {
            var t = (numberText ? numberText.transform : transform);
            t.DOKill();
            t.localScale = Vector3.one;
            t.DOPunchScale(Vector3.one * (bumpScale - 1f), bumpTime, 1, 0.5f).SetLink(gameObject);
        }
    }

    void UpdateLabel()
    {
        if (numberText) numberText.text = CurrentLevel.ToString();
        // if (numberTMP) numberTMP.text = CurrentLevel.ToString();
    }
    public void OnClickStartBtn()
    {
        LevelChoosePanel.SetActive(false);

        gameManager.SettingLevel(CurrentLevel-1);
        gameManager.StartGame();
    }
}
