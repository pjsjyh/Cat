using UnityEngine;

[System.Serializable]
public class Cat
{
    public int catNum; //������ �ִ� ����� ��ȣ
    public float health; // ����� ü��
    public float jump; // ����� ������
    public float happiness; // ����� ����
    public string catName; //����� �̸�

    public Vector3 catCurrentPosition; //����� ��ġ
    public bool isUse = false; //����� ��ġ ����
    public float installLocation; //����� ��ġ�� ��ġ

}
