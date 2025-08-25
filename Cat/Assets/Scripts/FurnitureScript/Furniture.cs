using System.Collections.Generic;
using UnityEngine;

public enum FurnitureType { Floor, Wall}
public enum Rarity { Common, Rare, Epic, Legendary }

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
    public int nowPeice=0; //���� ���� ����
    public int nowPeiceLevel=0; //���� ���� ����
    //��í�ý���
    public Rarity rarity = Rarity.Common; //���
    public bool gachaEligible = true; //��í ��� ����
    public int maxOwned = 5; //�ϼ�ǰ ���� �ѵ�
    public int nowOwned = 0; //���� ������ �ִ� ����

}

[System.Serializable]
public class FurnitureSaveData
{
    public string id;                // ���� ID
    public Vector3 position;        // ���� ��ġ
    public bool isPlaced;           // ��ġ ����

    public int installLocation = 0; //���� ��ġ ��ġ

    public int nowPeice = 0; //���� ���� ����
}