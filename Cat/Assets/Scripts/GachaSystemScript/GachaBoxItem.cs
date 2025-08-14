using UnityEngine;
using UnityEngine.UI;

public class GachaBoxItem : MonoBehaviour
{
    [SerializeField]
    RawImage boxImg;

    public void ChangeImage(Sprite getImg)
    {
        boxImg.texture = getImg.texture;
    }
}
