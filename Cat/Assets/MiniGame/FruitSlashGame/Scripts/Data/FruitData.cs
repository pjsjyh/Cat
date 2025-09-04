// ScriptableObject for fruit configuration
using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit", menuName = "Fruit Slash/Fruit Data")]
public class FruitData : ScriptableObject
{
    [Header("Visual")]
    public Sprite fruitSprite;
    public Sprite[] slicedSprites; // 2개 - 왼쪽, 오른쪽 조각

    [Header("Effects")]
    public string particleColor = "#FF0000"; // Hex 색상
    public Color GetParticleColor()
    {
        Color color;
        ColorUtility.TryParseHtmlString(particleColor, out color);
        return color;
    }

    [Header("Type")]
    public bool isBomb = false;
    public bool isSpecial = false; // 특별한 과일

    [Header("Score")]
    public int baseScore = 200;
}