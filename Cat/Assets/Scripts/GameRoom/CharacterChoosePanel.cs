using UnityEngine;

public class CharacterChoosePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject characterChoosePanel;

    public GameObject ReturnPanel()
    {
        return characterChoosePanel;
    }

}
