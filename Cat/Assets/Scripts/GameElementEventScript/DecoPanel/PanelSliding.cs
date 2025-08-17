using UnityEngine;

public class PanelSliding : MonoBehaviour
{
    //���� �ǳ� �����̵�. ��ư�� ����� �ݾ����� �ִϸ��̼�.
    [SerializeField] RectTransform container;
    [SerializeField] RectTransform bottomPanel;

    public bool isOn = true;
    private float slideAmount = 0f;
    private Vector2 originalPosition;

    private void OnEnable()
    {
        originalPosition = container.anchoredPosition; // Ȱ��ȭ �� ���� ��ġ ����
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
        slideAmount = height - 200f;  // �󸶸�ŭ ���ȴ��� ����
        container.anchoredPosition = new Vector2(0, -(height-200)); // �Ʒ��� �����̵�
        isOn = false;
    }

    public void SlideUp()
    {
        //container.anchoredPosition = Vector2.zero; // �ٽ� ����
        container.anchoredPosition = new Vector2(0, container.anchoredPosition.y + slideAmount);
        isOn = true;
    }
    public void OnDisable()
    {
        container.anchoredPosition = originalPosition;
        isOn = true;
    }

}
