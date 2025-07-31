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

    [Header("Special Effects (폭탄쥐용)")]
    public int scorePenalty = 0;
    public float timePenalty = 0f;
}