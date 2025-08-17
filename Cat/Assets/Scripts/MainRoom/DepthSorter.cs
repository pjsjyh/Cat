using System.Collections.Generic;
using UnityEngine;

public class DepthSorter : MonoBehaviour
{
    [Tooltip("�Ʒ�(y����)�� ���̸� false, �ڸ� true")]
    public bool lowerIsBack = false;
    public void SortNow()
    {
        //�ڽ� ��ȸ�Ͽ� ������Ʈ ����Ʈȭ
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

    // �ٴ��� ��������(����) ��, �������� ��
    public static void SortByBaseline(RectTransform parent)
    {
        var list = new List<RectTransform>();
        foreach (Transform t in parent) list.Add((RectTransform)t);

        list.Sort((a, b) => GetBottomY(a).CompareTo(GetBottomY(b))); // ��������: �Ʒ����
        for (int i = 0; i < list.Count; i++) list[i].SetSiblingIndex(i);
    }

    public static float GetBottomY(RectTransform rt)
    {
        //�θ���� �ڽ� y
        return rt.anchoredPosition.y - rt.rect.height * rt.pivot.y;
    }
}
