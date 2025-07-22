using UnityEngine;

[CreateAssetMenu(fileName = "NewCatData", menuName = "Data/Cat")]
public class Cat : ScriptableObject
{
    public string catId; //가지고 있는 고양이 번호
    public float health; // 고양이 체력
    public float jump; // 고양이 점프력
    public float happiness; // 고양이 행운력
    public string catName; //고양이 이름

    public Vector3 catCurrentPosition; //고양이 위치
    public bool isUse = false; //고양이 배치 유무
    public float installLocation; //고양이 설치된 위치 기본 -1

    public Sprite CatThumbnail; //고양이 썸네일
}

[System.Serializable]
public class CatSaveData
{
    public string id;                // 고유 ID
    public Vector3 position;        // 고양이 위치
    public bool isPlaced;           // 배치 여부
}