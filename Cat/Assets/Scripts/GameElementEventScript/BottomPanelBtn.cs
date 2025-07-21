using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BottomPanelBtn : MonoBehaviour
{
    public GameObject thisPanel;

    public void OnClick()
    {
        BottomPanelControl.ShowPanel(thisPanel);
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
