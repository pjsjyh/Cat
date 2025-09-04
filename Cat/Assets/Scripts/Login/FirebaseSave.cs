using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static PlayerDataFrame;

public class FirebaseSave : MonoBehaviour
{
    public static FirebaseSave Instance { get; private set; }
    FirebaseAuth auth;
    DatabaseReference dbRef;
    private GoogleLogin googleLogin;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 안 됨
        }
        else
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

    }
    void Start()
    {
        googleLogin = GameObject.Find("GoogleLogin").GetComponent<GoogleLogin>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Auth + DB 초기화 완료");
                googleLogin.SetAuth(auth);
                // 👉 앱 첫 실행 시 무조건 익명 로그인
                if (auth.CurrentUser != null)
                {
                    Debug.Log($"기존 로그인 유지됨: UID={auth.CurrentUser.UserId}, Provider={auth.CurrentUser.ProviderId}");
                }
                else
                {
                    // 👉 계정이 전혀 없는 경우에만 익명 로그인
                    auth.SignInAnonymouslyAsync().ContinueWith(loginTask =>
                    {
                        if (loginTask.IsCompleted && !loginTask.IsFaulted)
                        {
                            Debug.Log("익명 로그인 성공: " + auth.CurrentUser.UserId);
                        }
                        else
                        {
                            Debug.LogError("익명 로그인 실패: " + loginTask.Exception);
                        }
                    });
                }
            }
        });
    }

    public void SaveUserData(string playerData)
    {
        Debug.Log(auth);
        FirebaseUser user = auth.CurrentUser;
        if (user == null)
        {
            Debug.LogError("로그인된 유저 없음");
            return;
        }

        string uid = user.UserId;

        string json = playerData;

        // users/uid 경로에 저장
        dbRef.Child("users").Child(uid).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("유저 데이터 저장 성공: " + json);
            }
            else
            {
                Debug.LogError("저장 실패: " + task.Exception);
            }
        });
    }
    // 유저 데이터 불러오기
    public async Task<PlayerData> LoadUserDataAsync()
    {
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.Log("로그인된 유저 없음");
            return DataManager.LoadData(); // fallback
        }

        string uid = user.UserId;
        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot snapshot = await dbRef.Child("users").Child(uid).GetValueAsync();

        if (snapshot.Exists)
        {
            Debug.Log("로그인");
            string json = snapshot.GetRawJsonValue();
            Debug.Log(json);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            DataManager.SaveLocalData(data); // 로컬에도 캐싱
            return data;
        }
        else
        {
            return DataManager.LoadData();
        }
    }

}
