using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameCatalog Catalog;

    public List<int> shardCostsByLevel = new List<int> { 3, 5, 10 };
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ¾ÀÀÌ ¹Ù²î¾îµµ À¯ÁöµÇ°Ô
    }
    public void PickGame(GameCatalog pickGame)
    {
        Catalog = pickGame;
    }
    public void StartGameBtn()
    {
        var op = SceneManager.LoadSceneAsync(Catalog.sceneName, LoadSceneMode.Single);
    }
}
