using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FurnitureInfo : MonoBehaviour
{
    //boxlist 와 box sliding 기능
    public GameObject furnitureParent;
    public GameObject furnitureSliding;

    private Dictionary<string, GameObject> allBoxes = new(); //만든 mainfurniture box list

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
    public Dictionary<string, GameObject> ReturnAllFurnitureBoxList()
    {
        return allBoxes;
    }
}
