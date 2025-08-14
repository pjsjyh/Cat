using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaBtn : MonoBehaviour
{
    [SerializeField] Button myButton;
    [SerializeField] GachaSystem gacha;   // 호출 대상
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
        // ★ 여기서 원하는 값 두 개를 넘겨 호출
        gacha.OnClickGachaBtn(thisCash, boxnum);
    }
}
