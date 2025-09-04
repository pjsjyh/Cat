Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Range(0,100)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.texcoord);

                // 아웃라인 검사
                float alpha = c.a;
                if (alpha == 0) {
                    // 주변 픽셀 확인
                    for (int x = -1; x <= 1; x++) {
                        for (int y = -1; y <= 1; y++) {
                            float2 uv = i.texcoord + float2(x, y) * _MainTex_TexelSize.xy * _OutlineSize;
                            alpha = max(alpha, tex2D(_MainTex, uv).a);
                        }
                    }
                    if (alpha > 0) return _OutlineColor; // 아웃라인
                }

                return c;
            }
            ENDCG
        }
    }
}
