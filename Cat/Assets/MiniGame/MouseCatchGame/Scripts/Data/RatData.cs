using UnityEngine;
// 쥐 데이터 ScriptableObject
[CreateAssetMenu(fileName = "RatData", menuName = "Game/RatData")]
public class RatData : ScriptableObject
{
    public RatType ratType;
    public Sprite ratSprite;
    public Sprite ratHitSprite;
    public int health = 1;
    public float showDuration = 2f;
    public AudioClip hitSound;
    public GameObject hitEffect;
    public int scoreValue = 10;
    public int scorePenalty = -20;
    public float timePenalty = 5f;
}


