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
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지되게

        LoadData();
    }
    //데이터 불러오기. 기본아이템 제공을 위해 데이터가 없으면 기본 아이템 제공
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
                position = Vector3.zero,
                isPlaced = false
            });
        }
        if (playerData.roomData.furnitureList.Count == 0)
        {
            playerData.roomData.furnitureList.Add(new FurnitureSaveData
            {
                id = "CatTower",
                position = Vector3.zero,
                isPlaced = false
            });
        }

        StartCoroutine(LoadScene());
    }
    //종료시 데이터 자동 저장
    private void OnApplicationQuit()
    {
        DataManager.SaveData(playerData);
    }
    // title -> loading -> 데이터 셋팅 -> main
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f); // 3초 대기

        var fadeController = FindFirstObjectByType<FadeInOutScene>();
        if (fadeController != null)
        {
            // Fade 없이 Addressables 직접 전환
            //Addressables.LoadSceneAsync("MainRoomScene", LoadSceneMode.Single);

            // 페이드로 전환하고 싶으면 아래 사용
             fadeController.FadeToScene("MainRoomScene");
        }
        else
        {
            Debug.LogWarning("FadeInOutScene을 찾을 수 없습니다.");
        }

    }

}
