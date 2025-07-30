using UnityEngine;

public class FurnitureBoxItem : MonoBehaviour
{
    //���� list box ��ũ��Ʈ. ���� Ŭ����(this) �濡 ���� ����
    private GameObject furnitureParent;
    private GameObject furnitureSliding;

    [SerializeField]
    private GameObject furnitureCheckBtn;

    private Furniture furnitureData;
    
    private void Awake()
    {
        furnitureParent = FurnitureInfo.Instance.furnitureParent;
        furnitureSliding = FurnitureInfo.Instance.furnitureSliding;
        
    }
    public void CheckIsPlaced(string id)
    {
        //ȭ�鿡 ��ġ �� ���� ���� Ȯ��
        if (FurnitureManager.Instance.FurnitureIsPlaced(id))
        {
            furnitureCheckBtn.SetActive(true);
        }
    }
    public void SettingData(Furniture getData)
    {
        //box�� ���� �� ����
        furnitureData = getData;
    }
    public void FurnitureBoxClick()
    {
        //box Ŭ���� ���� ����

        if (FurnitureManager.Instance.FurnitureIsPlaced(furnitureData.furnitureId))
        {
            //��ġ�Ǿ��ִ� ������ box�� Ŭ��
            GameObject findFurniture = FurnitureManager.Instance.FindFurniture(furnitureData.furnitureId);
            if (findFurniture != null) {
                findFurniture.GetComponent<FurnitureDragHandler>().StartSetting();
            }
        }
        else
        {
            //��ġ �ȵ� ������ boxŬ��
            GameObject furniturePrefab = furnitureData.FurniturePrefab;
            GameObject furnitureObj = Instantiate(furniturePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            furnitureObj.transform.SetParent(furnitureParent.transform, false);
            furnitureObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            furnitureObj.GetComponent<FurnitureDragHandler>().SettingFurnitureData(furnitureData);
            
        }
        furnitureSliding.GetComponent<PanelSliding>().SlideDown();

    }
}
