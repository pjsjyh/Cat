using UnityEngine;
using UnityEngine.UI;


public class DecoPanelAni : MonoBehaviour
{
    //방 꾸미기 모드 on,off코드
    public GameObject getModal;
    [SerializeField]
    private GameObject handleObj;
    [SerializeField]
    private GameObject changeBtn;

    [SerializeField]
    private Sprite originImg;
    [SerializeField]
    private Sprite editImg;
    public void OpenModalWithAni()
    {
        if (getModal.activeSelf)
        {
            GetComponent<Image>().sprite = originImg;

            FurnitureManager.Instance.SetFurnitureEditorMode(false);
            if (handleObj != null)
            {
                if (handleObj.GetComponent<PanelSliding>().isOn)
                    getModal.GetComponent<Animator>().SetTrigger("CloseModal");
                else
                    getModal.SetActive(false);
            }
            
            FurnitureManager.Instance.DataUpdateFurniture();
            CatManager.Instance.DataUpdateCat();
        }
        else
        {
            GetComponent<Image>().sprite = editImg;
            FurnitureManager.Instance.SetFurnitureEditorMode(true);
            getModal.SetActive(true);
            getModal.transform.GetChild(0).gameObject.SetActive(false);
            getModal.transform.GetChild(1).gameObject.SetActive(false);
            getModal.GetComponent<Animator>().SetTrigger("OpenModal");
        }
    }
  
}
