using UnityEngine;

public class PauseBtnEvent : MonoBehaviour
{
    public GameObject PausePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseBtnClick()
    {
        //게임 멈추는 (시간과 )
        PausePanel.SetActive(true);
    }

    public void ExitBtnClick()
    {
        //메인씬으로
    }

    public void RestartBtnClick()
    {
        //게임새로시작
    }
    public void ContinueBtnClick()
    {
        PausePanel.SetActive(false);
    }
}
