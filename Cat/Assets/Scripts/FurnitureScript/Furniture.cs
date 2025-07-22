using UnityEngine;

[CreateAssetMenu(fileName = "NewFurnitureData", menuName = "Data/Furniture")]
public class Furniture : ScriptableObject

{
    public string furnitureId; //가지고 있는 가구 번호
    public bool isUse = false; //가구 설치 유무
    public float installLocation; //설치된 방
    public Vector3 installPosition; //설치된 위치
    public bool isCatUse = false; //고양이 사용 여부
}

[System.Serializable]
public class FurnitureSaveData
{
    public string id;                // 고유 ID
    public Vector3 position;        // 가구 위치
    public bool isPlaced;           // 배치 여부
}