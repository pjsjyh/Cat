using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatHandler : MonoBehaviour
{
    //메인 화면 고양이 한마리의 컨트롤 스크립트
    public enum CatState { Idle, Walk, Jump}

    private RectTransform rectTransform; //고양이 좌표
    private Canvas canvas; //고양이 그려진 캔버스
    public RectTransform canvasRect; //고양이 그려질 캔버스 사이즈


    private Cat cat; //고양이
    private CatState thisCatState;

    [SerializeField] 
    float updateHz = 1f; //고양이 몇초마다 업데이트
    float t;
    [SerializeField] 
    DepthSorter sorter;
    float lastY;

    public float CatMoveSpeed = 200f;
    public float WaitTime = 2f; //고양이 멈추는 시간
    private Vector2 targetPosition;
    private float waitTimer;

    private Animator catAnimation;

    // =================== ★ A* 관련 필드 ===================
    public FloorNavGrid grid;                 // floor 그리드(필수: 같은 부모 계층)
    private GridPathfinder pathfinder;        // 간단 A* 유틸(별도 클래스)
    private readonly List<Vector2Int> path = new(); // 셀 경로
    private int pathIndex = 0;
    public float arriveEps = 1.0f;            // 셀 중심 도착 허용 오차
    public float repathInterval = 0.5f;       // 재탐색 최소 간격
    private float lastRepathAt = -999f;

    private void Start()
    {
        catAnimation = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
        sorter = transform.parent.GetComponent<DepthSorter>();

        if (!grid) grid = GetComponentInParent<FloorNavGrid>();
        // ★ 길찾기 준비
        if (grid != null) pathfinder = new GridPathfinder(grid);

        thisCatState = CatState.Idle;
        SetNewTarget();
        thisCatState = CatState.Walk;

        Repath(true);
        lastY = DepthSorter.GetBottomY(rectTransform);
        sorter.SortNow();
    }
    public void Update()
    {
        

        switch (thisCatState)
        {
            
            case CatState.Idle:
                // Idle 상태 처리
                break;
            case CatState.Walk:
                // Walk 상태 처리
                catStateWalk();
                break;
            case CatState.Jump:
                // Jump 상태 처리
                break;
        }
       
    }
    void LateUpdate()
    {
        t += Time.deltaTime;
        if (t < 1f / updateHz) return;
        t = 0f;

        float y = DepthSorter.GetBottomY(rectTransform);
        if (Mathf.Abs(y - lastY) > 0.5f)
        {
            sorter.SortNow();   // 부모가 일괄 정렬
            lastY = y;
        }
    }

    void UpdateDepth()
    {
        // 부모는 "y 내림차순"으로 정렬된다고 가정(위쪽이 뒤, 아래쪽이 앞)
        int idx = 0;
        for (int i = 0; i < canvasRect.childCount; i++)
        {
            var c = (RectTransform)canvasRect.GetChild(i);
            if (c == rectTransform) continue;
            if (c.anchoredPosition.y > rectTransform.anchoredPosition.y)
                idx = i + 1; // y가 더 큰 애들 뒤에 배치 → 나는 더 앞
        }
        rectTransform.SetSiblingIndex(idx);
    }
    void CatIdleStart()
    {

    }
    void CatIdleUpdate()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0f)
        {
            thisCatState = CatState.Walk;
            SetNewTarget();   // 기존 랜덤 타겟
            Repath(true);     // ★ 새 목표로 길찾기
        }
    }

    private void catStateWalk()
    {
        //if (targetPosition.x < rectTransform.anchoredPosition.x)
        //    rectTransform.localScale = new Vector3(1, 1, 1); // 왼쪽으로
        //else
        //    rectTransform.localScale = new Vector3(-1, 1, 1);  // 오른쪽으로
        //rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, CatMoveSpeed * Time.deltaTime);
        //if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 1f)
        //{
        //    waitTimer -= Time.deltaTime;
        //    if (waitTimer <= 0f)
        //    {
        //        SetNewTarget();
        //    }
        //}..........000000000000000000000000000000000000000000000000000
        if (grid == null || pathfinder == null)
        {
            DirectWalkFallback();
            return;
        }

        // 경로가 없거나 끝났으면 재탐색/새 목표 선택
        if (path.Count == 0 || pathIndex >= path.Count)
        {
            if (!Repath(false)) // 너무 자주면 스킵
            {
                // 목표 자체가 막혀있을 수도 있으니 새 목표
                SetNewTarget();
                Repath(true);
            }
            if (path.Count == 0) { DirectWalkFallback(); return; }
        }

        // 현재 따라갈 셀의 월드 중심
        var c = path[pathIndex];
        var centerLocal = grid.CellToWorld(c); // floor 기준 로컬(Vector2)
        var centerWorld = grid.transform.TransformPoint(new Vector3(centerLocal.x, centerLocal.y, 0));

        // 시선(좌우 반전)
        var dir = centerWorld - rectTransform.position;
        if (dir.x < -0.1f) rectTransform.localScale = new Vector3(1, 1, 1);
        else if (dir.x > 0.1f) rectTransform.localScale = new Vector3(-1, 1, 1);

        // 이동
        rectTransform.position = Vector3.MoveTowards(
            rectTransform.position, centerWorld, CatMoveSpeed * Time.deltaTime);

        // 도착 체크
        if ((rectTransform.position - centerWorld).sqrMagnitude <= arriveEps * arriveEps)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                // 목적지 도착 → Idle 전환 후 잠깐 쉼
                thisCatState = CatState.Idle;
                waitTimer = WaitTime;
            }
        }

        // ★ 주기적 재탐색(가구가 새로 생겨 막힐 수 있음)
        if (Time.time - lastRepathAt >= repathInterval)
        {
            var cur = grid.WorldToCell(rectTransform.position);
            var goal = grid.WorldToCell(AnchoredToWorld(targetPosition));
            if (grid.blocked[cur.x, cur.y] || grid.blocked[goal.x, goal.y])
            {
                Repath(true);
            }
        }
    }
    private void DirectWalkFallback()
    {
        if (targetPosition.x < rectTransform.anchoredPosition.x)
            rectTransform.localScale = new Vector3(1, 1, 1); // 왼쪽으로
        else
            rectTransform.localScale = new Vector3(-1, 1, 1); // 오른쪽으로

        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition, targetPosition, CatMoveSpeed * Time.deltaTime);

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 1f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                SetNewTarget();
            }
        }
    }
    private void SetNewTarget()
    {
        //// moveArea 기준으로 랜덤한 위치 지정
        //float x = Random.Range(0, canvasRect.rect.width) - canvasRect.rect.width / 2;
        //float y = Random.Range(0, canvasRect.rect.height) - canvasRect.rect.height / 2;
        //targetPosition = new Vector2(x, y);
        //waitTimer = WaitTime;
        // moveArea 기준으로 랜덤한 위치 지정 (살짝 여백)
        float x = Random.Range(-canvasRect.rect.width * 0.45f, canvasRect.rect.width * 0.45f);
        float y = Random.Range(-canvasRect.rect.height * 0.45f, canvasRect.rect.height * 0.45f);
        targetPosition = new Vector2(x, y);
        waitTimer = WaitTime;

        // ★ 목표가 막혔으면 몇 번 샘플 옮겨봄
        if (grid != null)
        {
            for (int i = 0; i < 4; i++)
            {
                var goal = grid.WorldToCell(AnchoredToWorld(targetPosition));
                if (!grid.blocked[goal.x, goal.y]) break;
                targetPosition += Random.insideUnitCircle * grid.CellSize * 2f;
            }
        }
    }
    // =================== ★ 경로 재계산 ===================
    private bool Repath(bool force)
    {
        if (grid == null || pathfinder == null) return false;
        if (!force && Time.time - lastRepathAt < repathInterval) return false;

        var start = grid.WorldToCell(rectTransform.position);
        var goal = grid.WorldToCell(AnchoredToWorld(targetPosition));

        path.Clear();
        bool ok = pathfinder.FindPath(start, goal, path);
        lastRepathAt = Time.time;
        pathIndex = 0;
        return ok;
    }
    // anchored(UI로컬) → world
    private Vector3 AnchoredToWorld(Vector2 anchored)
    {
        return canvasRect.TransformPoint(new Vector3(anchored.x, anchored.y, 0f));
    }
    public void ChangeCatState(CatState getCatState)
    {
        thisCatState = getCatState;
    }

    private void CatSettingOn()
    {
        cat.catCurrentPosition = this.transform.position;
        cat.isPlaced = true;
        CatManager.Instance.AddCat(cat.catId, gameObject);
    }

    public void SettingCatData(Cat getData)
    {
        cat = getData;
        CatSettingOn();
    }
    public void CatRoom(int roomId)
    {
        cat.installLocation = roomId;
    }
    public Cat ReturnCatData()
    {
        return cat;
    }
}
