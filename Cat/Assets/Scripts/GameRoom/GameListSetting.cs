using UnityEngine;

public class GameListSetting : MonoBehaviour
{
    public GameList gameList;

    public GameObject GameListParent;
    public GameObject GameListPrefab;

    public void Start()
    {
        SettingGameList();
    }
    public void SettingGameList()
    {
        foreach (GameCatalog catalog in gameList.gameList)
        {
            GameObject newGame = Instantiate(GameListPrefab, GameListParent.transform);
            newGame.GetComponent<GameBoxInfo>().SettingGameInfo(catalog);

        }
    }
}
