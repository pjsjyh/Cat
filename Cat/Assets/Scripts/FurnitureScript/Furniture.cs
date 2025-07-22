using UnityEngine;

[CreateAssetMenu(fileName = "NewFurnitureData", menuName = "Data/Furniture")]
public class Furniture : ScriptableObject

{
    public string furnitureId; //������ �ִ� ���� ��ȣ
    public bool isUse = false; //���� ��ġ ����
    public float installLocation; //��ġ�� ��
    public Vector3 installPosition; //��ġ�� ��ġ
    public bool isCatUse = false; //����� ��� ����
}

[System.Serializable]
public class FurnitureSaveData
{
    public string id;                // ���� ID
    public Vector3 position;        // ���� ��ġ
    public bool isPlaced;           // ��ġ ����
}