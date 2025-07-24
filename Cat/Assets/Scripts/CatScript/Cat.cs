using UnityEngine;

[CreateAssetMenu(fileName = "NewCatData", menuName = "Data/Cat")]
public class Cat : ScriptableObject
{
    public string catId; //������ �ִ� ����� ��ȣ
    public float health; // ����� ü��
    public float jump; // ����� ������
    public float happiness; // ����� ����
    public float time; // ����� �ð�����
    public string catName; //����� �̸�

    public Vector3 catCurrentPosition; //����� ��ġ
    public bool isPlaced = false; //����� ��ġ ����
    public float installLocation; //����� ��ġ�� ��ġ �⺻ -1

    public Sprite CatThumbnail; //����� �����
}

[System.Serializable]
public class CatSaveData
{
    public string id;                // ���� ID
    public Vector3 position;        // ����� ��ġ
    public bool isPlaced;           // ��ġ ����

    public float installLocation;
    public string catName;
}