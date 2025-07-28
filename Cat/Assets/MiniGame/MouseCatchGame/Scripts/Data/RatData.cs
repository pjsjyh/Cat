using UnityEngine;
// 4. 쥐 데이터 ScriptableObject
[CreateAssetMenu(fileName = "RatData", menuName = "Game/RatData")]
public class RatData : ScriptableObject
{
    [Header("Basic Info")]
    public RatType ratType;
    public string ratName;

    [Header("Stats")]
    public int health = 1;
    public int scoreValue = 10;
    public float showDuration = 2f;

    [Header("Visual & Audio")]
    public Sprite ratSprite;
    public AudioClip hitSound;
    public GameObject hitEffect;
}