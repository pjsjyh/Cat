using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BottomPanelBtn : MonoBehaviour
{
    public GameObject thisPanel;
    public void OnClick()
    {
        Image img = GetComponent<Image>();
        BottomPanelControl.ShowPanel(thisPanel);
        BottomPanelControl.ChangeIcon(img);
    }
    public void OnClickInCat()
    {
        CatRoomBottomBtn.ShowPanel(thisPanel);
    }
    public void OnClickInStore()
    {
        StoreRoomBottomBtn.ShowPanel(thisPanel);
    }
}
