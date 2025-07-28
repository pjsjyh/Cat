using UnityEngine;
using UnityEngine.UI;

public class BottomPanelControl : MonoBehaviour
{
    public static BottomPanelControl Instance;

    public static GameObject currentOpenPanel;
    public GameObject firstPanel;
    public GameObject firstBtn;

    public Sprite pickIcon = null;
    public static Image originImg = null;
    private static Sprite previousSprite = null;
    private void Awake()
    {
        Instance = this; 
    }
    private void Start()
    {
        // 최초 한 번만 currentOpenPanel 설정
        if (currentOpenPanel == null && firstPanel !=null && firstBtn !=null)
        {
            currentOpenPanel = firstPanel;
            ChangeIcon(firstBtn.GetComponent<Image>());
        }
    }
    public static void ShowPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
            currentOpenPanel.SetActive(false);
        panel.SetActive(true);
        currentOpenPanel = panel;
    }
    public static void ChangeIcon(Image getImg)
    {
        if (originImg != null && previousSprite != null)
        {
            originImg.sprite = previousSprite;
        }
        originImg = getImg;
        previousSprite = getImg.sprite;
        getImg.sprite = Instance.pickIcon;
    }
}
