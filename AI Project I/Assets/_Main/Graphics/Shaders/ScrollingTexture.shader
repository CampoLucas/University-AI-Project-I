Shader "Unlit/ScrollingTextureDoubleSidedTrans" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Range(-10, 10)) = 1
    }
 
    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 100
 
        // Disable backface culling for double-sided rendering
        Cull Off
 
        // Enable transparency with alpha blending
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _Speed;
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                float2 uv = i.uv;
                uv.x -= _Time.x * _Speed;
                fixed4 col = tex2D(_MainTex, uv);
                col.a *= tex2D(_MainTex, uv).r;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
