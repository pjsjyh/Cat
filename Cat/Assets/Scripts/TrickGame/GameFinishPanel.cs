using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameFinishPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_mFinishTitle;
    [SerializeField]
    private TextMeshProUGUI text_coin;
    [SerializeField]
    private Button reStartBtn;
    [SerializeField]
    private Button reFinishBtn;
    [SerializeField]
    private GameObject reStartPanel;
    public void PanelSettings(bool isClear, string getMoney)
    {
        if (isClear)
        {
            text_mFinishTitle.text = "Game Clear";

            text_coin.text ="+"+ getMoney;
        }
        else
        {
            text_mFinishTitle.text = "Fail!";

            text_coin.text = "-"+getMoney;
        }

    }
    public void ClickFinishBtn()
    {
        var op = SceneManager.LoadSceneAsync("MainRoomScene", LoadSceneMode.Single);
    }
    public void RestartGame()
    {
        reStartPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
