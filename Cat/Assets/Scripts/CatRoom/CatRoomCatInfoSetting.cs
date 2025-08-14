using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatRoomCatInfoSetting : MonoBehaviour
{
    //고양이룸 고양이 정보 셋팅에 필요한 text object
    [SerializeField]
    public TextMeshProUGUI CatName;
    [SerializeField]
    public TextMeshProUGUI CatHealthBuff;
    [SerializeField]
    public TextMeshProUGUI CatLuckBuff;
    [SerializeField]
    public TextMeshProUGUI CatCoinBuff; 
    [SerializeField]
    public TextMeshProUGUI CatTimeBuff;

    public RawImage CatImg;

    public void ChangeCatInfo(Cat getCat)
    {
        CatName.text = getCat.catName;
        CatHealthBuff.text = getCat.health.ToString();
        CatLuckBuff.text = getCat.luck.ToString();
        CatCoinBuff.text = getCat.coin.ToString();
        CatTimeBuff.text = getCat.time.ToString();
    }
}
