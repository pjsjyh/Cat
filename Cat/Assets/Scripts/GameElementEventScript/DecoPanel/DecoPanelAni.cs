using UnityEngine;

public class DecoPanelAni : MonoBehaviour
{
    //방 꾸미기 모드 on,off코드
    public GameObject getModal;
    [SerializeField]
    private GameObject handleObj;
    public void OpenModalWithAni()
    {
        if (getModal.activeSelf)
        {
            FurnitureManager.Instance.SetFurnitureEditorMode(false);
            if (handleObj != null)
            {
                if (handleObj.GetComponent<PanelSliding>().isOn)
                    getModal.GetComponent<Animator>().SetTrigger("CloseModal");
                else
                    getModal.SetActive(false);
            }
            
            FurnitureManager.Instance.DataUpdateFurniture();
        }
        else
        {
            FurnitureManager.Instance.SetFurnitureEditorMode(true);
            getModal.SetActive(true);
            getModal.transform.GetChild(0).gameObject.SetActive(false);
            getModal.GetComponent<Animator>().SetTrigger("OpenModal");
        }
    }
  
}
