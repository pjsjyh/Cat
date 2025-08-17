using UnityEngine;
using UnityEngine.EventSystems;

public class FurnitureDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //가구 한개에 대한 drag and drop
    private RectTransform rectTransform; //가구 좌표
    private Canvas canvas; //가구 그려진 캔버스
    public RectTransform canvasRect; //가구 그려질 캔버스 사이즈

    private bool isEditoreMode = false;
    [SerializeField]
    private GameObject moveBox; //외곽선 박스 

    private Furniture furniture; //받은 가구 데이터

    // ▼ 추가: 앞뒤 제어용
    [SerializeField] private Transform zOrderParent;   // 가구들이 모인 부모(없으면 자기 parent)
    [SerializeField] private bool autoDepthByY = true; // 드랍 시 Y기준 자동 정렬 여부
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

        // 드래그 중엔 최상단으로 올려서 잡기 편하게
        originalIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        moveBox?.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEditoreMode || !FurnitureManager.Instance.isFurnitureEditorModeOn()) return;

        // 캔버스 스케일을 고려하여 위치 이동
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;

        // 이동 가능한 최대 영역 계산
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
    // Y가 낮을수록(화면 아래쪽) 앞으로 보이게 자동 정렬
    public static void SortSiblingsByY(RectTransform parent)
    {
        var list = new System.Collections.Generic.List<RectTransform>();
        foreach (Transform t in parent) list.Add((RectTransform)t);
        list.Sort((a, b) => b.anchoredPosition.y.CompareTo(a.anchoredPosition.y)); // y작은게 앞
        for (int i = 0; i < list.Count; i++) { list[i].SetSiblingIndex(i); Debug.Log(list[i].name); }
    }
    public void SettingOn()
    {
        //가구 설치 ok
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
        //가구 설치 후 데이터 셋팅
        furniture.installPosition = this.transform.position;
        furniture.isPlaced = true;

    }
    public void FurnitureSettingDeleteData()
    {
        //가구 설치 삭제 후 데이터 셋팅
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
