Shader "Custom/UIGlassShatter"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}         // UI用の画像テクスチャ
        _BreakMask ("Break Mask", 2D) = "white" {}          // 破壊エリアを示すマスク（白い部分が破壊される）
        _TimeFactor ("Time Factor", Range(0,1)) = 0         // 破壊進行度（0:初期、1:完全破壊）
        _Displacement ("Displacement Strength", Float) = 0.1 // 破片の移動量
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        // UI向けの設定：アルファブレンド、裏面描画無効、ライティングなし、Z書き込みオフ
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BreakMask;
            float _TimeFactor;
            float _Displacement;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            // 画素ごとに乱数を生成する簡易関数
            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // マスクで破壊エリアを判定
                float mask = tex2D(_BreakMask, IN.texcoord).r;

                // UV座標に基づいて各画素でランダムな角度を生成（0〜2π）
                float angle = rand(IN.texcoord * 100.0) * 6.2831853;
                // マスクがある部分のみ、_TimeFactorに応じてシフトさせる
                float displacement = mask * _TimeFactor * _Displacement;
                float2 offset = float2(cos(angle), sin(angle)) * displacement;
                float2 uv = IN.texcoord + offset;

                fixed4 col = tex2D(_MainTex, uv);
                col *= IN.color;  // UIでは頂点カラーが掛かっている場合があるので乗算
                return col;
            }
            ENDCG
        }
    }
}
