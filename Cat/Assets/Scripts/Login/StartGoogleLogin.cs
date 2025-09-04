using UnityEngine;

public class StartGoogleLogin : MonoBehaviour
{
    private GoogleLogin googleLogin;

    private void Start()
    {
        googleLogin = GameObject.Find("GoogleLogin").GetComponent<GoogleLogin>();
    }

    public void OnClickLoginBtn()
    {
        googleLogin.OnGoogleLoginClick();
    }
}
