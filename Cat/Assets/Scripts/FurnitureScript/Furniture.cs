using UnityEngine;

[System.Serializable]
public class Furniture
{
    public float furnutureNum; //가지고 있는 가구 번호
    public bool isUse = false; //가구 설치 유무
    public float installLocation; //설치된 방
    public Vector3 installPosition; //설치된 위치
    public bool isCatUse = false; //고양이 사용 여부
}
