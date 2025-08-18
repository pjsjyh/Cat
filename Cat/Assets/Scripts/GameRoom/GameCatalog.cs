using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Catalog", fileName = "GameCatalog")]
public class GameCatalog : ScriptableObject
{
    [Header("Identity")]
    public string id = "trick";             // 고유 ID (중복 금지)
    public string displayName = "Trick Game";

    [Header("Presentation")]
    [TextArea] public string description;
    public Sprite icon;
    public Sprite BackgroundImg;

    [Header("Scene")]
    // 빌드 세팅에 등록된 씬 이름(런타임 로딩용)
    public string sceneName;
}

[CreateAssetMenu(menuName = "Game/GameList", fileName = "GameList")]
public class GameList : ScriptableObject
{
    public List<GameCatalog> gameList = new List<GameCatalog>();
}