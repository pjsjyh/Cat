using UnityEngine;

public class CatController : MonoBehaviour
{
    //고양이 컨트롤 스크립트
    private Cat catData;
    
    public void Init(Cat data)
    {
        catData = data;
        transform.position = catData.catCurrentPosition;
    }

    public float GetHealth() => catData.health;
}
