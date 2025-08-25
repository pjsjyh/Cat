using UnityEngine;

public class GameStartBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        GameManager.Instance.StartGameBtn();
    }
}
