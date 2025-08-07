using UnityEngine;

public enum FurnitureType { Floor, Wall}

[CreateAssetMenu(fileName = "NewFurnitureData", menuName = "Data/Furniture")]
public class Furniture : ScriptableObject

{
    public string furnitureId; //가지고 있는 가구 번호
    public bool isPlaced = false; //가구 설치 유무
    public int installLocation; //설치된 방
    public Vector3 installPosition; //설치된 위치
    public bool isCatUse = false; //고양이 사용 여부
    public FurnitureType furnitureType = FurnitureType.Floor; //가구 설치 타입(벽 / 바닥)

    public Sprite FurnitureThumbnail; //가구 썸네일
    public GameObject FurniturePrefab; //가구 프리팹
}

[System.Serializable]
public class FurnitureSaveData
{
    public string id;                // 고유 ID
    public Vector3 position;        // 가구 위치
    public bool isPlaced;           // 배치 여부

    public int installLocation = 0; //가구 설치 위치
}