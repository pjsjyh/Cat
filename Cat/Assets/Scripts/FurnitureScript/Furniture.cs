using UnityEngine;

public enum FurnitureType { Floor, Wall}

[CreateAssetMenu(fileName = "NewFurnitureData", menuName = "Data/Furniture")]
public class Furniture : ScriptableObject

{
    public string furnitureId; //������ �ִ� ���� ��ȣ
    public bool isPlaced = false; //���� ��ġ ����
    public int installLocation; //��ġ�� ��
    public Vector3 installPosition; //��ġ�� ��ġ
    public bool isCatUse = false; //����� ��� ����
    public FurnitureType furnitureType = FurnitureType.Floor; //���� ��ġ Ÿ��(�� / �ٴ�)

    public Sprite FurnitureThumbnail; //���� �����
    public GameObject FurniturePrefab; //���� ������
}

[System.Serializable]
public class FurnitureSaveData
{
    public string id;                // ���� ID
    public Vector3 position;        // ���� ��ġ
    public bool isPlaced;           // ��ġ ����

    public int installLocation = 0; //���� ��ġ ��ġ
}