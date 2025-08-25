using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Catalog", fileName = "GameCatalog")]
public class GameCatalog : ScriptableObject
{
    [Header("Identity")]
    public string id = "trick";             // ���� ID (�ߺ� ����)
    public string displayName = "Trick Game";

    [Header("Presentation")]
    [TextArea] public string description;
    public Sprite icon;
    public Sprite BackgroundImg;

    [Header("Scene")]
    // ���� ���ÿ� ��ϵ� �� �̸�(��Ÿ�� �ε���)
    public string sceneName;
}

