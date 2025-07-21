using UnityEngine;

public class StoreRoomBottomBtn : MonoBehaviour
{
    public static GameObject currentStoreOpenPanel;
    public GameObject firstPanel;
    private void Start()
    {
        // 최초 한 번만 currentOpenPanel 설정
        if (currentStoreOpenPanel == null && firstPanel != null)
        {
            currentStoreOpenPanel = firstPanel;
        }
    }
    public static void ShowPanel(GameObject panel)
    {
        if (currentStoreOpenPanel != null && currentStoreOpenPanel != panel)
            currentStoreOpenPanel.SetActive(false);

        panel.SetActive(true);
        currentStoreOpenPanel = panel;
    }
}
