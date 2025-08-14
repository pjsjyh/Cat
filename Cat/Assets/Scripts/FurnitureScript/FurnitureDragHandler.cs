using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //���� �Ѱ��� ���� drag and drop
    private RectTransform rectTransform; //���� ��ǥ
    private Canvas canvas; //���� �׷��� ĵ����
    public RectTransform canvasRect; //���� �׷��� ĵ���� ������

    private bool isEditoreMode = false;
    [SerializeField]
    private GameObject moveBox; //�ܰ��� �ڽ� 

    private Furniture furniture; //���� ���� ������

    // �� �߰�: �յ� �����
    [SerializeField] private Transform zOrderParent;   // �������� ���� �θ�(������ �ڱ� parent)
    [SerializeField] private bool autoDepthByY = true; // ��� �� Y���� �ڵ� ���� ����
    private int originalIndex;

    [SerializeField]
    DepthSorter sorter;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
        if (!zOrderParent) zOrderParent = transform.parent;
        sorter = transform.parent.GetComponent<DepthSorter>();

    }
    public void StartSetting()
    {
        moveBox?.SetActive(true);
        isEditoreMode = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isEditoreMode || !FurnitureManager.Instance.isFurnitureEditorModeOn()) return;

        // �巡�� �߿� �ֻ������ �÷��� ��� ���ϰ�
        originalIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        moveBox?.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEditoreMode || !FurnitureManager.Instance.isFurnitureEditorModeOn()) return;

        // ĵ���� �������� ����Ͽ� ��ġ �̵�
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

        // �̵� ������ �ִ� ���� ���
        float halfWidth = rectTransform.rect.width / 2;
        float halfHeight = rectTransform.rect.height / 2;

        float minX = -canvasRect.rect.width / 2 + halfWidth;
        float maxX = canvasRect.rect.width / 2 - halfWidth;
        float minY = -canvasRect.rect.height / 2 + halfHeight;
        float maxY = canvasRect.rect.height / 2 - halfHeight;
        if(furniture.furnitureType == FurnitureType.Floor)
        {
            maxY = canvasRect.rect.height / 2 + halfHeight - 50f;
        }
        else if (furniture.furnitureType == FurnitureType.Wall)
        {
            minY = -canvasRect.rect.height / 2 - halfHeight+50f;
        }
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEditoreMode || !FurnitureManager.Instance.isFurnitureEditorModeOn()) return;
        sorter.SortNow();
        //if (autoDepthByY) SortSiblingsByY((RectTransform)zOrderParent);
        //else rectTransform.SetSiblingIndex(originalIndex);
    }
    // Y�� ��������(ȭ�� �Ʒ���) ������ ���̰� �ڵ� ����
    public static void SortSiblingsByY(RectTransform parent)
    {
        var list = new System.Collections.Generic.List<RectTransform>();
        foreach (Transform t in parent) list.Add((RectTransform)t);
        list.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y)); // y������ ��
        for (int i = 0; i < list.Count; i++) { list[i].SetSiblingIndex(i); Debug.Log(list[i].name); }
    }
    public void SettingOn()
    {
        //���� ��ġ ok
        moveBox.SetActive(false);
        isEditoreMode = false;
        Debug.Log(furniture.furnitureId+"");
        FurnitureManager.Instance.AddFurniture(furniture.furnitureId, this.gameObject);
        GameObject obj = FurnitureInfo.Instance.FindFurnitureBox(furniture.furnitureId);
        obj.GetComponent<FurnitureBoxItem>().CheckIsPlaced(furniture.furnitureId);

        FurnitureSettingOnData();
    }
    public void SettingNo()
    {
        GameObject obj = FurnitureInfo.Instance.FindFurnitureBox(furniture.furnitureId);
        obj.GetComponent<FurnitureBoxItem>().RemoveFurnitureCheck();
        FurnitureManager.Instance.RemoveFurnitureInPlace(furniture.furnitureId);
        Destroy(this.gameObject);
    }

    public void FurnitureSettingOnData()
    {
        //���� ��ġ �� ������ ����
        furniture.installPosition = this.transform.position;
        furniture.isPlaced = true;

    }
    public void FurnitureSettingDeleteData()
    {
        //���� ��ġ ���� �� ������ ����
        furniture.isPlaced = false;

    }
    public void SettingFurnitureData(Furniture getData)
    {
        furniture = getData;
    }
    public Furniture ReturnFurnitureData()
    {
        return furniture;
    }
}
