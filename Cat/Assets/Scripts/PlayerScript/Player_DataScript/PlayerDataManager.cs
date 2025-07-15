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
        playerData = DataManager.LoadData();
        StartCoroutine(LoadScene());
        
    }
    //����� ������ �ڵ� ����
    private void OnApplicationQuit()
    {
        DataManager.SaveData(playerData);
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

}
