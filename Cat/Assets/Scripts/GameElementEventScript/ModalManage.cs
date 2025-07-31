using System;
using UnityEngine;

public class ModalManage : MonoBehaviour
{
    //И№До on,off
    public GameObject getModal;
    private bool handleCheck = true;
    public GameObject modalHandle;

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

        }
        else
        {
            modalHandle.SetActive(false);
            handleCheck = true;
        }
    }
    public void HanelTrue()
    {

        handleCheck = true;
    }
}
