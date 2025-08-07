using System.Collections.Generic;
using UnityEngine;

public class DecoButtonGroup : MonoBehaviour
{
    public List<DecoChoiceBtn> buttons;
    public int defaultSelectedIndex = 0;
    private void Start()
    {
        // ������ �� ù ��ư�� ���� ���·� �����
        OnButtonClicked(buttons[defaultSelectedIndex]);
    }
    public void OnButtonClicked(DecoChoiceBtn selectedBtn)
    {
        foreach (var btn in buttons)
        {
            if (btn == selectedBtn) btn.ChoiceImg();
            else btn.ChangeMain();
        }
    }
}
