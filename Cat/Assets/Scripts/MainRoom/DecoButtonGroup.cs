using System.Collections.Generic;
using UnityEngine;

public class DecoButtonGroup : MonoBehaviour
{
    public List<DecoChoiceBtn> buttons;
    public int defaultSelectedIndex = 0;
    private void Start()
    {
        // 시작할 때 첫 버튼을 선택 상태로 만들기
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
