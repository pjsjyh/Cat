package com.catroom.google;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;

import com.google.android.gms.auth.api.signin.GoogleSignIn;
import com.google.android.gms.auth.api.signin.GoogleSignInAccount;
import com.google.android.gms.auth.api.signin.GoogleSignInClient;
import com.google.android.gms.auth.api.signin.GoogleSignInOptions;
import com.google.android.gms.common.api.ApiException;
import com.unity3d.player.UnityPlayer;

public class GoogleSignInPlugin {
    private static final int RC_SIGN_IN = 9001;
    private static GoogleSignInClient mGoogleSignInClient;

    public static void Init(String webClientId) {
        Activity activity = UnityPlayer.currentActivity;
        GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DEFAULT_SIGN_IN)
                .requestIdToken(webClientId)
                .requestEmail()
                .build();
        mGoogleSignInClient = GoogleSignIn.getClient(activity, gso);
        Log.d("GoogleSignInPlugin", "Init 완료");
    }

    public static void SignIn() {
        Activity activity = UnityPlayer.currentActivity;
        Intent signInIntent = mGoogleSignInClient.getSignInIntent();
        activity.startActivityForResult(signInIntent, RC_SIGN_IN);
    }

    public static void OnActivityResult(int requestCode, int resultCode, Intent data) {
        Log.d("GoogleSignInPlugin", "📌 OnActivityResult 실행됨, requestCode=" + requestCode + ", resultCode=" + resultCode);

        if (requestCode == RC_SIGN_IN) {
            try {
                GoogleSignInAccount account = GoogleSignIn.getSignedInAccountFromIntent(data)
                        .getResult(ApiException.class);
                String idToken = account.getIdToken();

                UnityPlayer.UnitySendMessage("GoogleLogin", "OnGoogleIdToken", idToken);
            } catch (ApiException e) {
                UnityPlayer.UnitySendMessage("GoogleLogin", "OnGoogleIdToken", "ERROR:" + e.getMessage());
            }
        }
    }
}
