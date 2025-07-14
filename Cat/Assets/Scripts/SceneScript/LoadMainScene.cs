using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FadeInOut;
public class LoadMainScene : MonoBehaviour
{
    [SerializeField]
    private string nextSceneAdress= "MainRoomScene";

    public FadeInOutScene fadeManager;

    public void OnStartButtonClick()
    {
        fadeManager.FadeToScene("LoadingScene");
    }
}
