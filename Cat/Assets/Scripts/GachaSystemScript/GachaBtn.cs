using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaBtn : MonoBehaviour
{
    [SerializeField] Button myButton;
    [SerializeField] GachaSystem gacha;   // ȣ�� ���
    [SerializeField] int thisCash = 3000;
    [SerializeField] int boxnum = 10;

    void Awake()
    {
        if (!myButton) myButton = GetComponent<Button>();
        if (!gacha) gacha = FindObjectOfType<GachaSystem>(); 
    }

    void OnEnable() => myButton.onClick.AddListener(HandleClick);
    void OnDisable() => myButton.onClick.RemoveListener(HandleClick);

    void HandleClick()
    {
        // �� ���⼭ ���ϴ� �� �� ���� �Ѱ� ȣ��
        gacha.OnClickGachaBtn(thisCash, boxnum);
    }
}
