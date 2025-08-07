using TMPro;
using UnityEngine;

public class ChangeName : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_TextMeshPro;


    void Start()
    {
        m_TextMeshPro.characterLimit = 10;
    }
    public void OnClickNameChangeBtn()
    {
        PlayerDataManager.Instance.ChangeName(m_TextMeshPro.text);
    }
}
