using UnityEngine;

public class DecorateModal : MonoBehaviour
{
    //И№До on,off
    public GameObject getModal;
    private bool handleCheck = true;
    public GameObject modalHandle;
    public GameObject modalBtn;
    public void CloseModal()
    {
        getModal.SetActive(false);
    }
    public void OpenModal()
    {
        getModal.SetActive(true);
    }
    public void HandleChange()
    {
        if (handleCheck)
        {
            modalHandle.SetActive(true);
            handleCheck = false;
            modalBtn.SetActive(true);
        }
        else
        {
            modalHandle.SetActive(false);
            modalBtn.SetActive(false);
            handleCheck = true;
        }
    }
    public void HanelTrue()
    {

        handleCheck = true;
    }
}
