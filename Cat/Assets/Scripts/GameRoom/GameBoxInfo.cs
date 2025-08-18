using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GameBoxInfo : MonoBehaviour
{
    private GameCatalog catalog;

    [SerializeField]
    private Image backgroundImg;
    public void SettingGameInfo(GameCatalog getInfo)
    {
        catalog = getInfo;
        SettingBox();
    }

    private void SettingBox()
    {
        backgroundImg.sprite = catalog.BackgroundImg;
    }
    public void PickThisGame()
    {
        GameManager.Instance.PickGame(catalog);
    }
    public void GameBoxClick()
    {
        this.transform.parent.GetComponent<CharacterChoosePanel>().ReturnPanel().SetActive(true);
    }
}
