using UnityEngine;

public class ModalManage : MonoBehaviour
{
    public GameObject getModal;
    public void CloseModal()
    {
        getModal.SetActive(false);
    }
    public void OpenModal()
    {
        getModal.SetActive(true);
    }
    
}
