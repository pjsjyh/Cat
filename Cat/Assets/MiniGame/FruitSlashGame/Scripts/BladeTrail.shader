Shader "Custom/BladeTrail"
{
    Properties
    {
        _Color ("Trail Color", Color) = (1,1,1,1)
        _Width ("Trail Width", Range(0.01, 0.1)) = 0.05
        _FadeSpeed ("Fade Speed", Range(0.1, 5)) = 2
        _GlowIntensity ("Glow Intensity", Range(0, 3)) = 1.5
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float3 worldPos : TEXCOORD1;
            };
            
            fixed4 _Color;
            float _Width;
            float _FadeSpeed;
            float _GlowIntensity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // UV를 중심으로부터의 거리로 계산
                float dist = abs(i.uv.y - 0.5) * 2.0;
                
                // 검날의 날카로운 가장자리 효과
                float edge = 1.0 - smoothstep(0.0, _Width, dist);
                
                // 글로우 효과
                float glow = 1.0 - smoothstep(0.0, _Width * 2.0, dist);
                glow = pow(glow, 2) * _GlowIntensity;
                
                // 시간에 따른 페이드
                float fade = sin(_Time.y * _FadeSpeed) * 0.1 + 0.9;
                
                // 최종 알파 계산
                float alpha = (edge + glow * 0.3) * i.color.a * fade;
                
                fixed4 finalColor = _Color;
                finalColor.a = alpha;
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}