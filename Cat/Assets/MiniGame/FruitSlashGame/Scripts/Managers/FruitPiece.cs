// FruitPiece.cs - 베어진 과일 조각 회전
using UnityEngine;

public class FruitPiece : MonoBehaviour
{
    [HideInInspector]
    public float rotationSpeed;

    private void Update()
    {
        // 조각들은 물리 회전과 별개로 추가 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}