using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    //���� ��Ʈ�� ��ũ��Ʈ
    private Furniture furnitureData;
    private FurnitureSaveData furnitureSaveData;
    public void Init(FurnitureSaveData data)
    {
        furnitureData = Resources.Load<Furniture>($"Data/Furniture/{data.id}");

        if (furnitureData == null)
        {
            Debug.LogWarning($"����� �����͸� ã�� �� �����ϴ�: {data.id}");
            return;
        }

        // 2. ��ġ ���� ��
        furnitureSaveData = data;
        transform.position = furnitureSaveData.position;

        Debug.Log($"���� {furnitureData.furnitureId} ���� �Ϸ�");

        furnitureSaveData = data;
    }
}
