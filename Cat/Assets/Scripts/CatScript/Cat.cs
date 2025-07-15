using UnityEngine;

[System.Serializable]
public class Cat
{
    public int catNum; //가지고 있는 고양이 번호
    public float health; // 고양이 체력
    public float jump; // 고양이 점프력
    public float happiness; // 고양이 행운력
    public string catName; //고양이 이름

    public Vector3 catCurrentPosition; //고양이 위치
    public bool isUse = false; //고양이 배치 유무
    public float installLocation; //고양이 설치된 위치

}
