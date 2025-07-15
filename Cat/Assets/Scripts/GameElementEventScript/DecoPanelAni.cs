using UnityEngine;

public class DecoPanelAni : MonoBehaviour
{
    public GameObject getModal;
    public void OpenModalWithAni()
    {
        if (getModal.activeSelf)
        {
            getModal.GetComponent<Animator>().SetTrigger("CloseModal");

        }
        else
        {
            getModal.SetActive(true);
            getModal.GetComponent<Animator>().SetTrigger("OpenModal");

        }
    }
}
