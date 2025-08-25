using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/GameList", fileName = "GameList")]
public class GameList : ScriptableObject
{
    public List<GameCatalog> gameList = new List<GameCatalog>();
}
