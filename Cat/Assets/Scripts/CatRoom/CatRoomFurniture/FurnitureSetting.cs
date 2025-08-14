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

    public List<int> shardCostsByLevel = new List<int> { 3, 5, 10 };

    private void settingBox()
    {
        furnitureTumnail.GetComponent<RawImage>().texture = furniture.FurnitureThumbnail.texture;
        furniturePeice.value = furniture.nowPeice;
        peice_text.text = furniture.nowPeice.ToString() +" / "+ shardCostsByLevel[furniture.nowPeiceLevel];
        level_text.text = "LV. "+furniture.nowPeiceLevel.ToString();
    }
    public void SettingFurniture(Furniture f)
    {
        furniture = f;
        settingBox();
    }
}
