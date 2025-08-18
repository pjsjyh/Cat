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
    //�� ���� ���� �Ŵ���
    [Header("Refs")]
    public TrickGameUIManager shuffler;     // UI�� DOTween ���� �Ŵ���
    public TrickGameLevelSet levelSet;       // �װ� ���� Level Set

    [Header("Common")]
    public float revealTime = 1.0f;          // ���� �� �� �����ð�(��� ���� ����)
    public float selectTimeout = 10f;
    public float cupSettingTimeout = 1f;
    [Header("State")]
    public int currentLevelIndex = 0;        // levelSet���� �� �ε���

    RectTransform ballCup;                  // ���� �����ִ� ��(����)
    bool inputEnabled;
    Coroutine timeoutCo;

    [SerializeField]
    private GameObject startBtn;

    public bool randomizeBallCup = true; // ���� �� ����
    [SerializeField] private RectTransform revealIconPrefab; // "����!" ������ ������(Image)
    [SerializeField] private Vector2 revealIconOffset = new Vector2(0, 80f); // �� ���� �� �����
    private RectTransform revealIconInstance;
    [SerializeField] private GameObject pickPrefabParent;
    void Start()
    {
        SetupClickHandlers(); // ó�� �� �� ����
    }
    public void StartGame()
    {
        //start ��ư ������ ����
        Debug.Log("!!");
        StartCurrentLevel();
        startBtn.gameObject.SetActive(false);
    }
    void SetupClickHandlers()
    {
        // ��� �ſ� Ŭ�� ������ ���̱�
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
        if (lv == null || shuffler == null) { Debug.LogWarning("������/���÷� Ȯ��"); return; }
        StopAllCoroutines();
        Debug.Log("!!");

        StartCoroutine(CoRunLevel(lv));
    }

    public void StartLevelByNumber(int levelNumber)
    {
        var lv = levelSet?.GetByLevelNumber(levelNumber);
        if (lv == null) { Debug.LogWarning($"Level {levelNumber} ����"); return; }
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

        // 1) ���� �� ����
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

        // 2) ���� ����(���� ��: cupsRoot ��¦ ����)
        var rt = shuffler.cupsRoot;
        var s = rt.DOScale(1.05f, revealTime * 0.5f)
                  .SetLoops(2, LoopType.Yoyo)
                  .SetEase(Ease.InOutSine)
                  .SetLink(rt.gameObject);
        //yield return s.WaitForCompletion();



        // 3) ���� ����(���� ������ ����Ϸ��� �ڷ�ƾ����)
        yield return shuffler.Shuffle();

        // TODO: ���⼭���ʹ� ����/���� ���� ���� (Ŭ�� �ڵ鷯 ��)
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
        // ���� �ǵ��: Ŭ�� �� ���� + ����
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

        // �����̸� ���� ���� ª�� ǥ��
        if (!correct && ballCup)
        {
            yield return ballCup.DOScale(1.1f, 0.25f)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetEase(Ease.InOutSine)
                                .SetLink(ballCup.gameObject)
                                .WaitForCompletion();
        }

        // ��� ó��: �����̸� ���� ����, �ƴϸ� �絵��
        //if (correct) NextLevel();
        //else RestartLevel();
    }

    IEnumerator CoSelectTimeout(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (!inputEnabled) yield break;
        inputEnabled = false;
        // Ÿ�Ӿƿ�: ���� ó���� �����ϰ�
        StartCoroutine(CoResult(null, false));
    }
    private IEnumerator RevealBallMarker()
    {
        // �������� ������ ������ ǥ��, ������ �� ��ü�� �޽�
        if (revealIconPrefab)
        {
            // ���� ������ ����
            if (revealIconInstance) Destroy(revealIconInstance.gameObject);
            Debug.Log("����");
            // cupsRoot �������� ���� �� �� ��ġ + ������
            revealIconInstance = Instantiate(revealIconPrefab, ballCup, false);
            revealIconInstance.anchorMin = revealIconInstance.anchorMax = new Vector2(0.5f, 0.5f);
            revealIconInstance.pivot = new Vector2(0.5f, 0.5f);


            revealIconInstance.localScale = Vector3.one;

            // ��¦ Ƣ�� ����(������/��ġ ���)
            var seq = DOTween.Sequence();
            seq.Join(revealIconInstance.DOScale(1.15f, revealTime * 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine));
            seq.Join(revealIconInstance.DOAnchorPosY(revealIconInstance.anchoredPosition.y + 10f, revealTime * 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine));
            seq.SetLink(revealIconInstance.gameObject);
            yield return seq.WaitForCompletion();

            //Destroy(revealIconInstance.gameObject); // ����� ����
            revealIconInstance = null;
        }
        else
        {
            // �������� ���ٸ� �� ��ü�� �޽�
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
        // �׻� "�� �ڽ�"�� �ѱ� �� ���� ������ ����
        if (!myCup) myCup = (RectTransform)transform;
        runner?.OnCupClicked(myCup);
    }
}