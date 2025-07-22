using UnityEngine;

public class CatController : MonoBehaviour
{
    //����� ��Ʈ�� ��ũ��Ʈ
    private Cat catData;
    private CatSaveData catSaveData;
    public void Init(CatSaveData data)
    {
        catData = Resources.Load<Cat>($"Data/Cat/{data.id}");

        if (catData == null)
        {
            Debug.LogWarning($"����� �����͸� ã�� �� �����ϴ�: {data.id}");
            return;
        }

        // 2. ��ġ ���� ��
        catSaveData = data;
        transform.position = catSaveData.position;

        Debug.Log($"����� {catData.catName} ���� �Ϸ� (health: {catData.health})");
    }

    public float GetHealth() => catData.health;
}
