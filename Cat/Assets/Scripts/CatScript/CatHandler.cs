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

    public float CatMoveSpeed = 200f;
    public float WaitTime = 2f; //����� ���ߴ� �ð�
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
    void CatIdleStart()
    {

    }
    private void catStateWalk()
    {
        if (targetPosition.x < rectTransform.anchoredPosition.x)
            rectTransform.localScale = new Vector3(1, 1, 1); // ��������
        else
            rectTransform.localScale = new Vector3(-1, 1, 1);  // ����������
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
        // moveArea �������� ������ ��ġ ����
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
