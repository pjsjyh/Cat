using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickGameUIManager : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform cupsRoot;  // 컵들이 들어있는 부모
    [Header("Shuffle")]
    public int swapCount = 10;      // 레벨에서 받아 주입
    public float baseDuration = 0.6f;
    public float accel = 0.02f;     // 스왑될수록 빨라짐
    public float arc = 40f;         // 아치 높이(px)

    [Header("Layout")]
    public bool disableLayoutDuringShuffle = true;

    readonly List<RectTransform> _cups = new();
    readonly Stack<RectTransform> _pool = new();
    LayoutGroup _layout;
    System.Random _rng = new System.Random();

    public IReadOnlyList<RectTransform> Cups => _cups;

    void Awake()
    {
        _layout = cupsRoot.GetComponent<LayoutGroup>();
        SetTrickCup();
    }

    public void SetTrickCup()
    {
        //rt아래에 있는 cup가져오기
        _cups.Clear();
        for (int i = 0; i < cupsRoot.childCount; i++)
        {
            if (cupsRoot.GetChild(i) is RectTransform rt)
                _cups.Add(rt);
        }
    }

    public void SetCupCount(int n)
    {
        // 컵 개수 보충.
        while (_cups.Count > n)
        {
            var last = _cups[_cups.Count - 1];
            _cups.RemoveAt(_cups.Count - 1);
            last.gameObject.SetActive(false);
            _pool.Push(last);
        }
        while (_cups.Count < n)
        {
            RectTransform cup = _pool.Count > 0 ? _pool.Pop() : Instantiate(_cups[0], cupsRoot);
            cup.gameObject.SetActive(true);
            cup.SetParent(cupsRoot, false);
            cup.name = $"Cup{_cups.Count}";
            _cups.Add(cup);
        }
        if (_layout) LayoutRebuilder.ForceRebuildLayoutImmediate(cupsRoot);
    }

    public Coroutine StartShuffle() => StartCoroutine(Shuffle());
    public IEnumerator Shuffle()
    {
        if (disableLayoutDuringShuffle && _layout) _layout.enabled = false;

        for (int i = 0; i < swapCount; i++)
        {
            if (_cups.Count < 2) break;

            int a = _rng.Next(_cups.Count);
            int b = _rng.Next(_cups.Count);
            if (a == b) { i--; continue; }

            float dur = Mathf.Max(0.2f, baseDuration - accel * i);
            yield return SwapUI_OrientedArc(_cups[a], _cups[b], dur, arc).WaitForCompletion();

            // 리스트 상 참조도 교환(다음 스왑에서 자연스럽게)
            (_cups[a], _cups[b]) = (_cups[b], _cups[a]);
        }

        if (disableLayoutDuringShuffle && _layout)
        {
            //_layout.enabled = true;
            //LayoutRebuilder.ForceRebuildLayoutImmediate(cupsRoot);
        }
    }
  
    static Sequence SwapUI(RectTransform a, RectTransform b, float dur, float arcPx)
    {
        var a0 = a.anchoredPosition;
        var b0 = b.anchoredPosition;

        a.SetAsLastSibling();
        b.SetAsLastSibling();

        var s = DOTween.Sequence();
        s.Join(a.DOAnchorPos(b0, dur).SetEase(Ease.InOutSine));
        s.Join(b.DOAnchorPos(a0, dur).SetEase(Ease.InOutSine));
        s.Insert(0, a.DOAnchorPosY(a0.y + arcPx, dur * 0.5f).SetLoops(2, LoopType.Yoyo));
        s.Insert(0, b.DOAnchorPosY(b0.y + arcPx, dur * 0.5f).SetLoops(2, LoopType.Yoyo));

        s.SetLink(a.gameObject);
        s.SetLink(b.gameObject);
        return s;
    }
    static Tween SwapUI_OrientedArc(RectTransform a, RectTransform b, float dur, float arcPx, Ease ease = Ease.InOutSine)
    {
        Vector2 a0 = a.anchoredPosition;
        Vector2 b0 = b.anchoredPosition;
        Vector2 d = b0 - a0;
        float len = d.magnitude;
        if (len < 0.001f) return DOVirtual.DelayedCall(0f, () => { });
        Vector2 n = new Vector2(-d.y, d.x).normalized; // 선분 수직 방향

        a.SetAsLastSibling(); b.SetAsLastSibling();

        return DOVirtual.Float(0f, 1f, dur, u =>
        {
            float w = DOVirtual.EasedValue(0f, 1f, u, ease);
            float hump = Mathf.Sin(Mathf.PI * w) * arcPx;
            a.anchoredPosition = Vector2.Lerp(a0, b0, w) + n * hump;
            b.anchoredPosition = Vector2.Lerp(b0, a0, w) - n * hump;
        })
        .OnComplete(() => { a.anchoredPosition = b0; b.anchoredPosition = a0; })
        .SetLink(a.gameObject);
    }
}

