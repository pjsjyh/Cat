package com.catroom.google;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log; 
import com.unity3d.player.UnityPlayerActivity;

public class UnityGoogleActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        // 🔑 GoogleSignInPlugin에 전달
         Log.d("UnityGoogleActivity", "onActivityResult called, request=" + requestCode);

        GoogleSignInPlugin.OnActivityResult(requestCode, resultCode, data);
    }
}
