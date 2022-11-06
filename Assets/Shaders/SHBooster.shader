Shader "Unlit/SHBooster"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Speed", Float) = 0.0
        _Color("Color (RGBA)",Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Unlit" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 scrolledUV = i.uv;
                fixed offset = _Speed * _Time;
                scrolledUV += fixed2(0, offset);
                half4 smp = tex2D(_MainTex, scrolledUV);
                fixed4 o;
                o = smp*_Color;
                o.a = smp.a;
                return o;
            }
            ENDCG
        }
    }
}
