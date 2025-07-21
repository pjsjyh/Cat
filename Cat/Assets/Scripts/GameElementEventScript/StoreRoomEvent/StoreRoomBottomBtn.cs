using UnityEngine;

public class StoreRoomBottomBtn : MonoBehaviour
{
    public static GameObject currentStoreOpenPanel;
    public GameObject firstPanel;
    private void Start()
    {
        // ���� �� ���� currentOpenPanel ����
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
