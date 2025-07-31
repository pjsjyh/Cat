using System.Collections.Generic;
using UnityEngine;

public class FurnitureInfo : MonoBehaviour
{
    //boxlist 와 box sliding 기능
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
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지되게
    }

    public void AddFurnitureBoxList(string getId, GameObject boxObj)
    {
        //boxlist에 가구 저장.
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
        //boxlist에서 원하는 가구의 box 찾기
        if (allBoxes.ContainsKey(getId))
        {
            return allBoxes[getId];
        }
        return null;
    }
}
