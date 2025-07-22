using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerDataFrame;

public class Topbar_Player : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI cashText;

    private PlayerPersonalData personalData;
    public void Start()
    {
        personalData = PlayerDataManager.Instance.playerData.playerPersonalData;
        Init(personalData);
    }
    public void Init(PlayerPersonalData data)
    {
        personalData = data;

        // �ʱ� �� ����
        nameText.text = personalData.PlayerName;
        coinText.text = $"{personalData.PlayerCoin}";
        cashText.text = $"{personalData.PlayerCash}";

        // �̺�Ʈ ����
        personalData.OnNameChanged += newName => nameText.text = newName;
        personalData.OnCoinChanged += coin => coinText.text = $"{coin}";
        personalData.OnCashChanged += cash => cashText.text = $"{cash}";
    }
}
