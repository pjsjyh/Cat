using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameCatalog Catalog;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ���� �ٲ� �����ǰ�
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
