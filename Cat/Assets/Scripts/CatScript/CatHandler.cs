using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CatHandler : MonoBehaviour
{
    //���� ȭ�� ����� �Ѹ����� ��Ʈ�� ��ũ��Ʈ
    public enum CatState { Idle, Walk, Jump}

    private RectTransform rectTransform; //����� ��ǥ
    private Canvas canvas; //����� �׷��� ĵ����
    public RectTransform canvasRect; //����� �׷��� ĵ���� ������


    private Cat cat; //�����
    private CatState thisCatState;

    [SerializeField] 
    float updateHz = 1f; //����� ���ʸ��� ������Ʈ
    float t;
    [SerializeField] 
    DepthSorter sorter;
    float lastY;

    public float CatMoveSpeed = 200f;
    public float WaitTime = 2f; //����� ���ߴ� �ð�
    private Vector2 targetPosition;
    private float waitTimer;

    private Animator catAnimation;

    // =================== �� A* ���� �ʵ� ===================
    public FloorNavGrid grid;                 // floor �׸���(�ʼ�: ���� �θ� ����)
    private GridPathfinder pathfinder;        // ���� A* ��ƿ(���� Ŭ����)
    private readonly List<Vector2Int> path = new(); // �� ���
    private int pathIndex = 0;
    public float arriveEps = 1.0f;            // �� �߽� ���� ��� ����
    public float repathInterval = 0.5f;       // ��Ž�� �ּ� ����
    private float lastRepathAt = -999f;

    private void Start()
    {
        catAnimation = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
        sorter = transform.parent.GetComponent<DepthSorter>();

        if (!grid) grid = GetComponentInParent<FloorNavGrid>();
        // �� ��ã�� �غ�
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
                // Idle ���� ó��
                break;
            case CatState.Walk:
                // Walk ���� ó��
                catStateWalk();
                break;
            case CatState.Jump:
                // Jump ���� ó��
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
            sorter.SortNow();   // �θ� �ϰ� ����
            lastY = y;
        }
    }

    void UpdateDepth()
    {
        // �θ�� "y ��������"���� ���ĵȴٰ� ����(������ ��, �Ʒ����� ��)
        int idx = 0;
        for (int i = 0; i < canvasRect.childCount; i++)
        {
            var c = (RectTransform)canvasRect.GetChild(i);
            if (c == rectTransform) continue;
            if (c.anchoredPosition.y > rectTransform.anchoredPosition.y)
                idx = i + 1; // y�� �� ū �ֵ� �ڿ� ��ġ �� ���� �� ��
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
            SetNewTarget();   // ���� ���� Ÿ��
            Repath(true);     // �� �� ��ǥ�� ��ã��
        }
    }

    private void catStateWalk()
    {
        //if (targetPosition.x < rectTransform.anchoredPosition.x)
        //    rectTransform.localScale = new Vector3(1, 1, 1); // ��������
        //else
        //    rectTransform.localScale = new Vector3(-1, 1, 1);  // ����������
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

        // ��ΰ� ���ų� �������� ��Ž��/�� ��ǥ ����
        if (path.Count == 0 || pathIndex >= path.Count)
        {
            if (!Repath(false)) // �ʹ� ���ָ� ��ŵ
            {
                // ��ǥ ��ü�� �������� ���� ������ �� ��ǥ
                SetNewTarget();
                Repath(true);
            }
            if (path.Count == 0) { DirectWalkFallback(); return; }
        }

        // ���� ���� ���� ���� �߽�
        var c = path[pathIndex];
        var centerLocal = grid.CellToWorld(c); // floor ���� ����(Vector2)
        var centerWorld = grid.transform.TransformPoint(new Vector3(centerLocal.x, centerLocal.y, 0));

        // �ü�(�¿� ����)
        var dir = centerWorld - rectTransform.position;
        if (dir.x < -0.1f) rectTransform.localScale = new Vector3(1, 1, 1);
        else if (dir.x > 0.1f) rectTransform.localScale = new Vector3(-1, 1, 1);

        // �̵�
        rectTransform.position = Vector3.MoveTowards(
            rectTransform.position, centerWorld, CatMoveSpeed * Time.deltaTime);

        // ���� üũ
        if ((rectTransform.position - centerWorld).sqrMagnitude <= arriveEps * arriveEps)
        {
            pathIndex++;
            if (pathIndex >= path.Count)
            {
                // ������ ���� �� Idle ��ȯ �� ��� ��
                thisCatState = CatState.Idle;
                waitTimer = WaitTime;
            }
        }

        // �� �ֱ��� ��Ž��(������ ���� ���� ���� �� ����)
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
            rectTransform.localScale = new Vector3(1, 1, 1); // ��������
        else
            rectTransform.localScale = new Vector3(-1, 1, 1); // ����������

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
        //// moveArea �������� ������ ��ġ ����
        //float x = Random.Range(0, canvasRect.rect.width) - canvasRect.rect.width / 2;
        //float y = Random.Range(0, canvasRect.rect.height) - canvasRect.rect.height / 2;
        //targetPosition = new Vector2(x, y);
        //waitTimer = WaitTime;
        // moveArea �������� ������ ��ġ ���� (��¦ ����)
        float x = Random.Range(-canvasRect.rect.width * 0.45f, canvasRect.rect.width * 0.45f);
        float y = Random.Range(-canvasRect.rect.height * 0.45f, canvasRect.rect.height * 0.45f);
        targetPosition = new Vector2(x, y);
        waitTimer = WaitTime;

        // �� ��ǥ�� �������� �� �� ���� �Űܺ�
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
    // =================== �� ��� ���� ===================
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
    // anchored(UI����) �� world
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
