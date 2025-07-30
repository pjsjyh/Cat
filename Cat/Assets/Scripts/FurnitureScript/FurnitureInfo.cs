using System.Collections.Generic;
using UnityEngine;

public class FurnitureInfo : MonoBehaviour
{
    //boxlist �� box sliding ���
    public static FurnitureInfo Instance { get; private set; }
    public GameObject furnitureParent;
    public GameObject furnitureSliding;

    private Dictionary<string, GameObject> allBoxes = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ���� �ٲ� �����ǰ�
    }

    public void AddFurnitureBoxList(string getId, GameObject boxObj)
    {
        //boxlist�� ���� ����.
        allBoxes[getId] = boxObj;
    }
    public bool FindFurnitureBoxInList(string getId)
    {
        if (allBoxes.ContainsKey(getId))
        {
            return true;
        }
        return false;
    }
    public GameObject FindFurnitureBox(string getId)
    {
        //boxlist���� ���ϴ� ������ box ã��
        if (allBoxes.ContainsKey(getId))
        {
            return allBoxes[getId];
        }
        return null;
    }
}
