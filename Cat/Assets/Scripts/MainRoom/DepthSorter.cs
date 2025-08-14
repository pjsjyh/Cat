using System.Collections.Generic;
using UnityEngine;

public class DepthSorter : MonoBehaviour
{
    [Tooltip("아래(y작음)가 앞이면 false, 뒤면 true")]
    public bool lowerIsBack = false;
    public void SortNow()
    {
        //자식 순회하여 오브젝트 리스트화
        var parent = (RectTransform)transform;
        var list = new List<RectTransform>();
        foreach (Transform t in parent)
        {
            if (t.gameObject.activeInHierarchy) 
                list.Add((RectTransform)t);
        }

        list.Sort((a, b) =>
        {
            float ya = GetBottomY(a);
            float yb = GetBottomY(b);
            return lowerIsBack ? ya.CompareTo(yb) : yb.CompareTo(ya);
        });

        for (int i = 0; i < list.Count; i++) list[i].SetSiblingIndex(i);
    }

    // 바닥이 낮을수록(작음) 뒤, 높을수록 앞
    public static void SortByBaseline(RectTransform parent)
    {
        var list = new List<RectTransform>();
        foreach (Transform t in parent) list.Add((RectTransform)t);

        list.Sort((a, b) => GetBottomY(a).CompareTo(GetBottomY(b))); // 오름차순: 아래→뒤
        for (int i = 0; i < list.Count; i++) list[i].SetSiblingIndex(i);
    }

    public static float GetBottomY(RectTransform rt)
    {
        //부모기준 자식 y
        return rt.anchoredPosition.y - rt.rect.height * rt.pivot.y;
    }
}
