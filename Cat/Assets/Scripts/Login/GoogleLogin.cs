using Firebase;
using Firebase.Auth;
using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
public class GoogleLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    private static GoogleLogin instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.name = "GoogleLogin";
            Debug.Log("GoogleLogin GameObject 생성 및 DontDestroyOnLoad 설정");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    if (task.Result == DependencyStatus.Available)
        //    {
        //        auth = FirebaseAuth.DefaultInstance;
        //        Debug.Log("Firebase 초기화 완료");
        //    }
        //});
    }
    public void SetAuth(FirebaseAuth getAuth)
    {
        auth = getAuth;
    }
    // Java Plugin 호출
    public void OnGoogleLoginClick()
    {
        using (AndroidJavaClass plugin = new AndroidJavaClass("com.catroom.google.GoogleSignInPlugin"))
        {
            plugin.CallStatic("Init", "241940281632-7m88qaebgqlbldfu288a8oplbnr86psj.apps.googleusercontent.com");
            plugin.CallStatic("SignIn");
        }
    }

    // Java → Unity로 전달되는 idToken 처리
    public void OnGoogleIdToken(string idToken)
    {
        if (idToken.StartsWith("ERROR:"))
        {
            Debug.LogError("Google Sign-In 실패: " + idToken);
            return;
        }

        Credential googleCred = GoogleAuthProvider.GetCredential(idToken, null);

        FirebaseUser currentUser = auth.CurrentUser;

        if (currentUser != null && currentUser.IsAnonymous)
        {
            // ✅ 익명 계정 → 구글 계정으로 업그레이드
            currentUser.LinkWithCredentialAsync(googleCred).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("계정 연동 실패: " + task.Exception);
                }
                else
                {
                    FirebaseUser linkedUser = task.Result.User;
                    Debug.Log($"구글 계정으로 연동 완료! UID 유지: {linkedUser.UserId}");
                }
            });
        }
        else
        {
            // ✅ 이미 구글 로그인 한 경우 → 그냥 로그인
            auth.SignInWithCredentialAsync(googleCred).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Firebase 구글 로그인 실패: " + task.Exception);
                }
                else
                {
                    FirebaseUser newUser = task.Result;
                    Debug.Log($"구글 로그인 성공: {newUser.DisplayName}, UID: {newUser.UserId}");
                }
            });
        }
    }
}
