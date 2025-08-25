using Firebase;
using Firebase.Auth;
using Google;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
            }
        });

        // 구글 로그인 설정 (Web Client ID는 Firebase 콘솔 → 인증 → 구글 로그인 설정에 있음)
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = "YOUR_WEB_CLIENT_ID.apps.googleusercontent.com",
            RequestIdToken = true
        };
    }

    public void OnGoogleLoginClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        #if UNITY_EDITOR
    Debug.Log("구글 로그인은 에디터에서는 실행 불가. (모바일 빌드 필요)");
        #else
       if (task.IsFaulted)
    {
        Debug.LogError("Google Sign-In Failed: " + task.Exception);
        return;
    }
    if (task.IsCanceled)
    {
        Debug.LogWarning("Google Sign-In Canceled");
        return;
    }

    // 성공
    GoogleSignInUser gUser = task.Result;
    Debug.Log($"✅ Google Sign-In Success: {gUser.DisplayName}, {gUser.Email}");

    // Firebase Auth로 연동
    Credential credential = GoogleAuthProvider.GetCredential(gUser.IdToken, null);
    auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
    {
        if (authTask.IsFaulted || authTask.IsCanceled)
        {
            Debug.LogError("Firebase Sign-In Failed: " + authTask.Exception);
        }
        else
        {
            FirebaseUser newUser = authTask.Result;
            Debug.Log($"🎉 Firebase Sign-In Success: {newUser.DisplayName}, {newUser.Email}, UID: {newUser.UserId}");
        }
    });
        #endif
    }
}
