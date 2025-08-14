using System.Collections.Generic;
using UnityEngine;

public class GridPathfinder
{
    readonly FloorNavGrid grid;
    static readonly Vector2Int[] DIRS = {
        new( 1,0), new(-1,0), new(0, 1), new(0,-1)
    };

    public GridPathfinder(FloorNavGrid g) { grid = g; }

    public bool FindPath(Vector2Int start, Vector2Int goal, List<Vector2Int> outPath)
    {
        outPath.Clear();
        if (!In(start) || !In(goal) || grid.blocked[goal.x, goal.y]) return false;

        var open = new PriorityQueue<Vector2Int>();
        var came = new Dictionary<Vector2Int, Vector2Int>();
        var gCost = new Dictionary<Vector2Int, int>();

        gCost[start] = 0;
        open.Push(start, Heu(start, goal));

        while (open.Count > 0)
        {
            var cur = open.Pop();
            if (cur == goal) { Reconstruct(came, cur, outPath); outPath.Reverse(); return true; }

            foreach (var d in DIRS)
            {
                var nx = cur + d;
                if (!In(nx) || grid.blocked[nx.x, nx.y]) continue;

                int tentative = gCost[cur] + 10;
                if (!gCost.TryGetValue(nx, out var old) || tentative < old)
                {
                    gCost[nx] = tentative;
                    came[nx] = cur;
                    int f = tentative + Heu(nx, goal);
                    open.Push(nx, f);
                }
            }
        }
        return false;
    }

    int Heu(Vector2Int a, Vector2Int b) => 10 * (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    bool In(Vector2Int c) => c.x >= 0 && c.x < grid.w && c.y >= 0 && c.y < grid.h;

    void Reconstruct(Dictionary<Vector2Int, Vector2Int> came, Vector2Int cur, List<Vector2Int> path)
    {
        path.Add(cur);
        while (came.TryGetValue(cur, out var p)) { cur = p; path.Add(cur); }
    }

    // 아주 단순한 우선순위 큐
    class PriorityQueue<T>
    {
        readonly List<(T item, int pri)> list = new();
        public int Count => list.Count;
        public void Push(T item, int pri) { list.Add((item, pri)); }
        public T Pop()
        {
            int bi = 0;
            for (int i = 1; i < list.Count; i++) if (list[i].pri < list[bi].pri) bi = i;
            var it = list[bi].item; list.RemoveAt(bi); return it;
        }
    }
}
