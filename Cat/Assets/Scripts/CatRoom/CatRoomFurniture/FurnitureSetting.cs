using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureSetting : MonoBehaviour
{
    [SerializeField]
    private GameObject furnitureTumnail;
    [SerializeField]
    private Slider furniturePeice;
    [SerializeField]
    private TextMeshProUGUI level_text;
    [SerializeField]
    private TextMeshProUGUI peice_text;
    private Furniture furniture;


    private void settingBox()
    {
        furnitureTumnail.GetComponent<RawImage>().texture = furniture.FurnitureThumbnail.texture;
        furniturePeice.value = furniture.nowPeice;

        int nowLevel = GameManager.Instance.shardCostsByLevel[furniture.nowPeiceLevel];
        peice_text.text = furniture.nowPeice.ToString() +" / "+ nowLevel;
        level_text.text = "LV. "+furniture.nowPeiceLevel.ToString();
    }
    public void SettingFurniture(Furniture f)
    {
        furniture = f;
        settingBox();
    }
}
