using UnityEngine;

public class FurnitureNav : MonoBehaviour
{
    public FloorNavGrid grid;
    public RectTransform rt;
    [Header("그리드 발판(셀 단위)")]
    public Vector2Int sizeCells = new(1, 1);    // 가로x세로
    public Vector2Int pivotCell = new(0, 0);    // 발판 내부에서의 피벗(보통 아래중앙)
    
    
 
    void Awake() 
    {
        if (!rt) rt = GetComponent<RectTransform>();
        if (!grid) grid = GetComponentInParent<FloorNavGrid>();
        FitToRect();
    }
    [ContextMenu("Fit To Rect")]
    public void FitToRect()
    {
        float cell = grid ? grid.CellSize : 32f;
        var sz = rt.rect.size;

        sizeCells = new Vector2Int(
            Mathf.Max(1, Mathf.CeilToInt(sz.x / cell)),
            Mathf.Max(1, Mathf.CeilToInt(sz.y / cell))
        );

        pivotCell = new Vector2Int(
            Mathf.Clamp(Mathf.RoundToInt(rt.pivot.x * sizeCells.x), 0, sizeCells.x - 1),
            Mathf.Clamp(Mathf.RoundToInt(rt.pivot.y * sizeCells.y), 0, sizeCells.y - 1)
        );
    }
    public void Register()
    {
        Vector3 bottomCenterLocal = new(rt.rect.center.x, rt.rect.yMin, 0f);
        Vector3 bottomCenterWorld = rt.TransformPoint(bottomCenterLocal);

        var baseCell = grid.WorldToCell(bottomCenterWorld); // ← baseCell 시작 위치
        Debug.Log(baseCell.x + " " + baseCell.y);
        var min = new Vector2Int(baseCell.x - pivotCell.x, baseCell.y - pivotCell.y);
        var rect = new RectInt(min, sizeCells);
        grid.SetBlocked(rect, true);
    }
    public void Unregister()
    {
        Vector3 bottomCenterLocal = new(rt.rect.center.x, rt.rect.yMin, 0f);
        Vector3 bottomCenterWorld = rt.TransformPoint(bottomCenterLocal);

        var baseCell = grid.WorldToCell(bottomCenterWorld);
        var min = new Vector2Int(baseCell.x - pivotCell.x, baseCell.y - pivotCell.y);
        var rect = new RectInt(min, sizeCells);
        grid.SetBlocked(rect, false);
    }
}
