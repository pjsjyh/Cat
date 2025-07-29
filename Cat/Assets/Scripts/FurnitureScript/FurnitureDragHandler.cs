using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //���� �Ѱ��� ���� drag and drop
    private RectTransform rectTransform; //���� ��ǥ
    private Canvas canvas; //���� �׷��� ĵ����
    public RectTransform canvasRect; //���� �׷��� ĵ���� ������

    private bool isEditoreMode = true;
    [SerializeField]
    private GameObject moveBox; //�ܰ��� �ڽ� 

    private Furniture furniture; //���� ���� ������

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = transform.parent.GetComponent<RectTransform>();
    }
    public void StartSetting()
    {
        moveBox.SetActive(true);
        isEditoreMode = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEditoreMode) return;
        // ĵ���� �������� ����Ͽ� ��ġ �̵�
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

        // �̵� ������ �ִ� ���� ���
        float halfWidth = rectTransform.rect.width / 2;
        float halfHeight = rectTransform.rect.height / 2;

        float minX = -canvasRect.rect.width / 2 + halfWidth;
        float maxX = canvasRect.rect.width / 2 - halfWidth;
        float minY = -canvasRect.rect.height / 2 + halfHeight;
        float maxY = canvasRect.rect.height / 2 - halfHeight;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void SettingOn()
    {
        moveBox.SetActive(false);
        isEditoreMode = false;

        FurnitureManager.Instance.AddFurniture(furniture.furnitureId, this.gameObject);
        GameObject obj = FurnitureInfo.Instance.FindFurnitureBox(furniture.furnitureId);
        obj.GetComponent<FurnitureBoxItem>().CheckIsPlaced(furniture.furnitureId);
    }
    public void SettingNo()
    {
        Destroy(this);
    }

    public void SettingFurnitureData(Furniture getData)
    {
        furniture = getData;
    }
}
