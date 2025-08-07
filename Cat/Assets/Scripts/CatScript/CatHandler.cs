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

    public float CatMoveSpeed = 200f;
    public float WaitTime = 2f; //고양이 멈추는 시간
    private Vector2 targetPosition;
    private float waitTimer;

    private Animator catAnimation;

    private void Start()
    {
        catAnimation = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
        thisCatState = CatState.Idle;
        SetNewTarget();
        thisCatState = CatState.Walk;
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
    void CatIdleStart()
    {

    }
    private void catStateWalk()
    {
        if (targetPosition.x < rectTransform.anchoredPosition.x)
            rectTransform.localScale = new Vector3(1, 1, 1); // 왼쪽으로
        else
            rectTransform.localScale = new Vector3(-1, 1, 1);  // 오른쪽으로
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, CatMoveSpeed * Time.deltaTime);
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
        // moveArea 기준으로 랜덤한 위치 지정
        float x = Random.Range(0, canvasRect.rect.width) - canvasRect.rect.width / 2;
        float y = Random.Range(0, canvasRect.rect.height) - canvasRect.rect.height / 2;
        targetPosition = new Vector2(x, y);
        waitTimer = WaitTime;
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
    public Cat ReturnCatData()
    {
        return cat;
    }
}
