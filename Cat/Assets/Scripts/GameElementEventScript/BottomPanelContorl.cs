using UnityEngine;

public class BottomPanelControl : MonoBehaviour
{
    public static GameObject currentOpenPanel;
    public GameObject firstPanel;
    private void Start()
    {
        // ���� �� ���� currentOpenPanel ����
        if (currentOpenPanel == null && firstPanel !=null)
        {
            currentOpenPanel = firstPanel;
        }
    }
    public static void ShowPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
            currentOpenPanel.SetActive(false);

        panel.SetActive(true);
        currentOpenPanel = panel;
    }

}
