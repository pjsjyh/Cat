using UnityEngine;

public class FloorNavGrid : MonoBehaviour
{
    //¹Ù´Ú¿¡ ÀÌµ¿Ä­ »ý¼º
    [SerializeField] RectTransform floor;   // ¹Ù´Ú ºÎ¸ð
    [SerializeField] float cell = 32f;
    public int w = 20, h = 12;               // ¼¿ ¼ö
    public bool[,] blocked;

    [Header("Debug")]
    public bool drawGridGizmos = true;
    public Color lineColor = new Color(1f, 1f, 1f, 0.25f);
    public Color blockedFill = new Color(1f, 0f, 0f, 0.25f);
    [Range(0.5f, 1f)] public float blockedFillScale = 0.9f;

    public float CellSize => cell;

    void Awake() 
    {
        if (!floor) 
        { 
            floor = (RectTransform)transform; 
        }
        RecalcGrid();
    }
    void RecalcGrid()
    {
        var size = floor.rect.size;
        w = Mathf.Max(1, Mathf.FloorToInt(size.x / cell));
        h = Mathf.Max(1, Mathf.FloorToInt(size.y / cell));
        blocked = new bool[w, h];
    }
    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        var local = floor.InverseTransformPoint(worldPos);
        var size = floor.rect.size;
        float ox = -size.x * 0.5f, oy = -size.y * 0.5f;
        int cx = Mathf.Clamp(Mathf.FloorToInt((local.x - ox) / cell), 0, w - 1);
        int cy = Mathf.Clamp(Mathf.FloorToInt((local.y - oy) / cell), 0, h - 1);
        return new Vector2Int(cx, cy);
    }
    public Vector2 CellToWorld(Vector2Int c)
    {
        var size = floor.rect.size;
        float ox = -size.x * 0.5f, oy = -size.y * 0.5f;
        float x = ox + (c.x + 0.5f) * cell;
        float y = oy + (c.y + 0.5f) * cell;
        return new Vector2(x, y);
    }
    public void Clear() { blocked = new bool[w, h]; }
    public void SetBlocked(RectInt rect, bool value)
    {
        for (int y = rect.yMin; y < rect.yMax; y++)
            for (int x = rect.xMin; x < rect.xMax; x++)
                if (x >= 0 && x < w && y >= 0 && y < h) blocked[x, y] = value;
    }
    public void OnDrawGizmos()
    {
        if (!drawGridGizmos) return;
        var f = floor ? floor : (RectTransform)transform;

        var size = f.rect.size;
        var half = size * 0.5f;
        int cols = Mathf.FloorToInt(size.x / cell);
        int rows = Mathf.FloorToInt(size.y / cell);

        // 1) °ÝÀÚ ¼±
        Gizmos.color = lineColor;
        for (int y = 0; y <= rows; y++)
        {
            float yy = -half.y + y * cell;
            var a = f.TransformPoint(new Vector3(-half.x, yy, 0));
            var b = f.TransformPoint(new Vector3(half.x, yy, 0));
            Gizmos.DrawLine(a, b);
        }
        for (int x = 0; x <= cols; x++)
        {
            float xx = -half.x + x * cell;
            var a = f.TransformPoint(new Vector3(xx, -half.y, 0));
            var b = f.TransformPoint(new Vector3(xx, half.y, 0));
            Gizmos.DrawLine(a, b);
        }

        // 2) ¸·Èù Ä­ Ã¤¿ì±â
        if (blocked != null)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!blocked[x, y]) continue;
                    float cx = -half.x + (x + 0.5f) * cell;
                    float cy = -half.y + (y + 0.5f) * cell;
                    var center = f.TransformPoint(new Vector3(cx, cy, 0));
                    Gizmos.color = blockedFill;
                    // »ìÂ¦ ¾Õ/µÚ·Î ¶ç¿ö¼­ ¾À¿¡¼­ º¸ÀÌ°Ô zµÎ²² 0.001
                    Gizmos.DrawCube(center, new Vector3(cell, cell, 0.001f));
                }
            }
        }
    }
}
