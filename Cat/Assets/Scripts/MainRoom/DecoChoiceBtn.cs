using UnityEngine;
using UnityEngine.UI;

public class DecoChoiceBtn : MonoBehaviour
{
    //메인 가구/고양이 선택 버튼 스크립트
    public Sprite mainImg;
    public Sprite choiceImg;
    public DecoButtonGroup group;

    public void OnClick()
    {
        group.OnButtonClicked(this);
    }

    public void ChangeMain() => GetComponent<Image>().sprite = mainImg;
    public void ChoiceImg() => GetComponent<Image>().sprite = choiceImg;
}
