using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //가구 한개에 대한 drag and drop
    private RectTransform rectTransform; //가구 좌표
    private Canvas canvas; //가구 그려진 캔버스
    public RectTransform canvasRect; //가구 그려질 캔버스 사이즈

    private bool isEditoreMode = true;
    [SerializeField]
    private GameObject moveBox; //외곽선 박스 

    private Furniture furniture; //받은 가구 데이터

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
        // 캔버스 스케일을 고려하여 위치 이동
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

        // 이동 가능한 최대 영역 계산
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
