using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    //가구 컨트롤 스크립트
    private Furniture furnitureData;
    private FurnitureSaveData furnitureSaveData;
    public void Init(FurnitureSaveData data)
    {
        furnitureData = Resources.Load<Furniture>($"Data/Furniture/{data.id}");

        if (furnitureData == null)
        {
            Debug.LogWarning($"고양이 데이터를 찾을 수 없습니다: {data.id}");
            return;
        }

        // 2. 위치 설정 등
        furnitureSaveData = data;
        transform.position = furnitureSaveData.position;

        Debug.Log($"가구 {furnitureData.furnitureId} 세팅 완료");

        furnitureSaveData = data;
    }
}
