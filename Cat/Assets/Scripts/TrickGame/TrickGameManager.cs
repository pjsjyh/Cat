using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TrickGameManager : MonoBehaviour
{
    //컵 섞기 게임 매니저
    [Header("Refs")]
    public TrickGameUIManager shuffler;     // UI용 DOTween 셔플 매니저
    public TrickGameLevelSet levelSet;       // 네가 만든 Level Set

    [Header("Common")]
    public float revealTime = 1.0f;          // 시작 전 공 공개시간(모든 레벨 공통)
    public float selectTimeout = 10f;
    public float cupSettingTimeout = 1f;
    [Header("State")]
    public int currentLevelIndex = 0;        // levelSet에서 쓸 인덱스

    RectTransform ballCup;                  // 공이 숨어있는 컵(참조)
    bool inputEnabled;
    Coroutine timeoutCo;

    [SerializeField]
    private GameObject startBtn;

    public bool randomizeBallCup = true; // 정답 컵 랜덤
    [SerializeField] private RectTransform revealIconPrefab; // "여기!" 아이콘 프리팹(Image)
    [SerializeField] private Vector2 revealIconOffset = new Vector2(0, 80f); // 컵 위로 얼마 띄울지
    private RectTransform revealIconInstance;
    [SerializeField] private GameObject pickPrefabParent;
    void Start()
    {
        SetupClickHandlers(); // 처음 한 번 세팅
    }
    public void StartGame()
    {
        //start 버튼 누르면 시작
        Debug.Log("!!");
        StartCurrentLevel();
        startBtn.gameObject.SetActive(false);
    }
    void SetupClickHandlers()
    {
        // 모든 컵에 클릭 전달자 붙이기
        foreach (var cup in shuffler.Cups)
        {
            var h = cup.GetComponent<CupClickHandler>();
            if (!h) h = cup.gameObject.AddComponent<CupClickHandler>();
            h.Bind(this);
        }
    }
    [ContextMenu("Start Current Level")]
    public void StartCurrentLevel()
    {
        var lv = levelSet?.GetByIndex(currentLevelIndex);
        if (lv == null) { Debug.LogWarning("iv"); }
        if(shuffler == null)
        {
            Debug.LogWarning("shuffler");
        }
        if (lv == null || shuffler == null) { Debug.LogWarning("레벨셋/셔플러 확인"); return; }
        StopAllCoroutines();
        Debug.Log("!!");

        StartCoroutine(CoRunLevel(lv));
    }

    public void StartLevelByNumber(int levelNumber)
    {
        var lv = levelSet?.GetByLevelNumber(levelNumber);
        if (lv == null) { Debug.LogWarning($"Level {levelNumber} 없음"); return; }
        currentLevelIndex = Mathf.Max(0, levelSet.levels.IndexOf(lv));
        StartCurrentLevel();
    }

    public void NextLevel()
    {
        currentLevelIndex = Mathf.Min(currentLevelIndex + 1, levelSet.levels.Count - 1);
        StartCurrentLevel();
    }

    public void RestartLevel() => StartCurrentLevel();

    private IEnumerator CoRunLevel(TrickGame lv)
    {
        Debug.Log("!!");

        // 1) 레벨 값 주입
        shuffler.SetCupCount(lv.cupCount);
        shuffler.swapCount = lv.swapCount;

        SetupClickHandlers();
        yield return new WaitForSeconds(cupSettingTimeout);

        if (randomizeBallCup || ballCup == null)
        {
            int idx = UnityEngine.Random.Range(0, shuffler.Cups.Count);
            ballCup = (RectTransform)shuffler.Cups[idx];
        }
        Debug.Log(ballCup.gameObject.name);
        Debug.Log(ballCup.gameObject.transform.position);
        yield return RevealBallMarker();

        // 2) 공개 연출(간단 예: cupsRoot 살짝 깜빡)
        var rt = shuffler.cupsRoot;
        var s = rt.DOScale(1.05f, revealTime * 0.5f)
                  .SetLoops(2, LoopType.Yoyo)
                  .SetEase(Ease.InOutSine)
                  .SetLink(rt.gameObject);
        //yield return s.WaitForCompletion();



        // 3) 셔플 시작(끝날 때까지 대기하려면 코루틴으로)
        yield return shuffler.Shuffle();

        // TODO: 여기서부터는 선택/판정 로직 연결 (클릭 핸들러 등)
        inputEnabled = true;
        if (selectTimeout > 0f)
            timeoutCo = StartCoroutine(CoSelectTimeout(selectTimeout));
    }

    public void OnCupClicked(RectTransform clicked)
    {
        if (!inputEnabled) return;
        inputEnabled = false;
        if (timeoutCo != null) { StopCoroutine(timeoutCo); timeoutCo = null; }

        bool correct = (clicked == ballCup);
        StartCoroutine(CoResult(clicked, correct));
    }
    IEnumerator CoResult(RectTransform clicked, bool correct)
    {
        // 간단 피드백: 클릭 컵 점프 + 색상
        if (clicked)
        {
            var img = clicked.GetComponent<Image>();
            Color original = img ? img.color : Color.white;
            if (img) img.color = correct ? new Color(0.75f, 1f, 0.75f) : new Color(1f, 0.75f, 0.75f);

            yield return clicked.DOAnchorPosY(clicked.anchoredPosition.y + 30f, 0.2f)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetEase(Ease.OutQuad)
                                .SetLink(clicked.gameObject)
                                .WaitForCompletion();

            if (img) img.color = original;
        }

        // 오답이면 정답 컵을 짧게 표시
        if (!correct && ballCup)
        {
            yield return ballCup.DOScale(1.1f, 0.25f)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetEase(Ease.InOutSine)
                                .SetLink(ballCup.gameObject)
                                .WaitForCompletion();
        }

        // 결과 처리: 정답이면 다음 레벨, 아니면 재도전
        //if (correct) NextLevel();
        //else RestartLevel();
    }

    IEnumerator CoSelectTimeout(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (!inputEnabled) yield break;
        inputEnabled = false;
        // 타임아웃: 오답 처리와 동일하게
        StartCoroutine(CoResult(null, false));
    }
    private IEnumerator RevealBallMarker()
    {
        // 프리팹이 있으면 아이콘 표시, 없으면 컵 자체를 펄스
        if (revealIconPrefab)
        {
            // 기존 아이콘 정리
            if (revealIconInstance) Destroy(revealIconInstance.gameObject);
            Debug.Log("생성");
            // cupsRoot 기준으로 생성 → 컵 위치 + 오프셋
            revealIconInstance = Instantiate(revealIconPrefab, ballCup, false);
            revealIconInstance.anchorMin = revealIconInstance.anchorMax = new Vector2(0.5f, 0.5f);
            revealIconInstance.pivot = new Vector2(0.5f, 0.5f);


            revealIconInstance.localScale = Vector3.one;

            // 살짝 튀는 연출(스케일/위치 요요)
            var seq = DOTween.Sequence();
            seq.Join(revealIconInstance.DOScale(1.15f, revealTime * 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine));
            seq.Join(revealIconInstance.DOAnchorPosY(revealIconInstance.anchoredPosition.y + 10f, revealTime * 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine));
            seq.SetLink(revealIconInstance.gameObject);
            yield return seq.WaitForCompletion();

            //Destroy(revealIconInstance.gameObject); // 숨기고 시작
            revealIconInstance = null;
        }
        else
        {
            // 프리팹이 없다면 컵 자체를 펄스
            yield return ballCup.DOScale(1.12f, revealTime * 0.5f)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetEase(Ease.InOutSine)
                                .SetLink(ballCup.gameObject)
                                .WaitForCompletion();
        }
    }

}
public class CupClickHandler : MonoBehaviour, IPointerClickHandler
{
    public TrickGameManager runner;
    public RectTransform myCup;

    public void Bind(TrickGameManager r)
    {
        runner = r;
        if (!myCup) myCup = (RectTransform)transform;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 항상 "나 자신"을 넘김 → 참조 엇갈림 방지
        if (!myCup) myCup = (RectTransform)transform;
        runner?.OnCupClicked(myCup);
    }
}