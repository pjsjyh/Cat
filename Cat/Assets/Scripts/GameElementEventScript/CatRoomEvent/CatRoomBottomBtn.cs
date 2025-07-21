using UnityEngine;

public class CatRoomBottomBtn : MonoBehaviour
{
    public static GameObject currentCatOpenPanel;
    public GameObject firstPanel;
    private void Start()
    {
        // ���� �� ���� currentOpenPanel ����
        if (currentCatOpenPanel == null && firstPanel != null)
        {
            currentCatOpenPanel = firstPanel;
        }
    }
    public static void ShowPanel(GameObject panel)
    {
        if (currentCatOpenPanel != null && currentCatOpenPanel != panel)
            currentCatOpenPanel.SetActive(false);

        panel.SetActive(true);
        currentCatOpenPanel = panel;
    }
}
