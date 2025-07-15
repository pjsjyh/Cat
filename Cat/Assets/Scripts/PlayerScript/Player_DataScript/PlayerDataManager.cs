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
        playerData = DataManager.LoadData();
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
