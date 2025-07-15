using UnityEngine;

public class CatController : MonoBehaviour
{
    //����� ��Ʈ�� ��ũ��Ʈ
    private Cat catData;
    
    public void Init(Cat data)
    {
        catData = data;
        transform.position = catData.catCurrentPosition;
    }

    public float GetHealth() => catData.health;
}
