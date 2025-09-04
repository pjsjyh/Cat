package com.catroom.google;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class MyUnityPlayerActivity extends com.unity3d.player.UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Log.d("MyUnityPlayerActivity", "✅ onCreate 실행됨");

        String webClientId = "241940281632-7m88qaebgqlbldfu288a8oplbnr86psj.apps.googleusercontent.com";
        GoogleSignInPlugin.Init(webClientId);
        Log.d("MyUnityPlayerActivity", "✅ GoogleSignInPlugin Init 완료");
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Log.d("MyUnityPlayerActivity", "📌 onActivityResult 전달됨: request=" + requestCode);
        GoogleSignInPlugin.OnActivityResult(requestCode, resultCode, data);
    }
}
