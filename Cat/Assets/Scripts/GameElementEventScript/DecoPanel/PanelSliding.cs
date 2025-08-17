using UnityEngine;

public class PanelSliding : MonoBehaviour
{
    //가구 판넬 슬라이딩. 버튼을 남기고 닫아지는 애니메이션.
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform bottomPanel;

    public bool isOn = true;
    private float slideAmount = 0f;
    private Vector2 originalPosition;

    private void OnEnable()
    {
        originalPosition = container.anchoredPosition; // 활성화 시 최초 위치 저장
    }
    public void Sliding()
    {
        if (isOn)
        {
            SlideDown();
        }
        else
        {
            SlideUp();
        }
    }
    public void SlideDown()
    {
        float height = bottomPanel.rect.height;
        slideAmount = height - 200f;  // 얼마만큼 내렸는지 저장
        container.anchoredPosition = new Vector2(0, -(height-200)); // 아래로 슬라이드
        isOn = false;
    }

    public void SlideUp()
    {
        //container.anchoredPosition = Vector2.zero; // 다시 위로
        container.anchoredPosition = new Vector2(0, container.anchoredPosition.y + slideAmount);
        isOn = true;
    }
    public void OnDisable()
    {
        container.anchoredPosition = originalPosition;
        isOn = true;
    }

}
