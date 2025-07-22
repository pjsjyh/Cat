using UnityEngine;

public class CatController : MonoBehaviour
{
    //고양이 컨트롤 스크립트
    private Cat catData;
    private CatSaveData catSaveData;
    public void Init(CatSaveData data)
    {
        catData = Resources.Load<Cat>($"Data/Cat/{data.id}");

        if (catData == null)
        {
            Debug.LogWarning($"고양이 데이터를 찾을 수 없습니다: {data.id}");
            return;
        }

        // 2. 위치 설정 등
        catSaveData = data;
        transform.position = catSaveData.position;

        Debug.Log($"고양이 {catData.catName} 세팅 완료 (health: {catData.health})");
    }

    public float GetHealth() => catData.health;
}
