using UnityEngine;
using UnityEngine.UI;

public class DecoChoiceBtn : MonoBehaviour
{
    //���� ����/����� ���� ��ư ��ũ��Ʈ
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
