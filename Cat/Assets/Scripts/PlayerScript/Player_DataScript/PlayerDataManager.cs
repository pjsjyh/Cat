using FadeInOut;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using static PlayerDataFrame;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    public PlayerData playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ���� �ٲ� �����ǰ�

        LoadData();
    }
    //������ �ҷ�����. �⺻������ ������ ���� �����Ͱ� ������ �⺻ ������ ����
    private void LoadData()
    {
        playerData = DataManager.LoadData();

        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        if (playerData.catData.catDataList.Count == 0)
        {
            playerData.catData.catDataList.Add(new CatSaveData
            {
                id = "WhiteCat",
                position = new Vector3(600f, 600f, 0),
                isPlaced = false
            });
        }
        if (playerData.roomData.furnitureList.Count == 0)
        {
            playerData.roomData.furnitureList.Add(new FurnitureSaveData
            {
                id = "CatTower",
                position = new Vector3(600f,600f,0),
                isPlaced = false,
                installLocation = 0,
                nowPeice = 0,
            });
            playerData.roomData.furnitureList.Add(new FurnitureSaveData
            {
                id = "Scratcher",
                position = new Vector3(600f, 600f, 0),
                isPlaced = false,
                installLocation =0,
                nowPeice =0,
            });
        }
        Debug.Log(playerData.playerPersonalData.PlayerName);
        StartCoroutine(LoadScene());
    }
    //����� ������ �ڵ� ����
    private void OnApplicationQuit()
    {
        DataManager.SaveData(playerData);
    }
    // ���� ��׶���� �� �� ����
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            DataManager.SaveData(playerData);
        }
    }
    // title -> loading -> ������ ���� -> main
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f); // 3�� ���

        var fadeController = FindFirstObjectByType<FadeInOutScene>();
        if (fadeController != null)
        {
            // Fade ���� Addressables ���� ��ȯ
            //Addressables.LoadSceneAsync("MainRoomScene", LoadSceneMode.Single);

            // ���̵�� ��ȯ�ϰ� ������ �Ʒ� ���
             fadeController.FadeToScene("MainRoomScene");
        }
        else
        {
            Debug.LogWarning("FadeInOutScene�� ã�� �� �����ϴ�.");
        }

    }
    public void ChangeName(string getName)
    {
        playerData.playerPersonalData.PlayerName = getName;

    }
    public int ReturnPlayerPlace()
    {
        return playerData.playerPersonalData.PlayerPlace;
    }
    public int ReturnPlayerCash()
    {
        return playerData.playerPersonalData.PlayerCash;
    }
    public void SaveData()
    {

    }
    public int ReturnPlayerCoin()
    {
        return playerData.playerPersonalData.PlayerCoin;
    }
    public void UsePlayerMoney(int getMoney)
    {
        playerData.playerPersonalData.PlayerCoin += getMoney;
    }
}
