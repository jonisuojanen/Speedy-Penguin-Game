Shader "Unlit/Water"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _WaveMultiplier("Wave multiplier",Float) = 1.0
        _SubwaveScale("Subwave scale",Float) = 1.0
        _SubwaveMultiplier("Subwave multiplier",Float) = 1.0
        _PositionScalar("Position scalar",Float) = 1.0
        _DeepPower("Deep power",Float) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Unlit" }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"


            float _WaveMultiplier;
            float _SubwaveScale;
            float _SubwaveMultiplier;
            float _PositionScalar;
            float _DeepPower;
            sampler2D _MainTex;
            float4 _MainTex_ST;

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

            //Unity gradient noise methods
            //-----------------------------------------------
            float2 unity_gradientNoise_dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            {
                Out = unity_gradientNoise(UV * Scale) + 0.5;
            }
            //-----------------------------------------------
            v2f vert(appdata v)
            {
                float noise = 0.0;
                float noiseOffset = _Time.y * _SubwaveMultiplier;
                float2 noiseTilingOffset = { (v.uv.x * _MainTex_ST.x) + noiseOffset, (v.uv.y * _MainTex_ST.y) + noiseOffset };
                Unity_GradientNoise_float(noiseTilingOffset, _SubwaveScale, noise);

                v.vertex.y = noise * _PositionScalar;

                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float noise = 0.0;
                float noiseOffset = _Time.y * _SubwaveMultiplier;
                float2 noiseTilingOffset = {i.uv.x + noiseOffset, i.uv.y + noiseOffset};
                Unity_GradientNoise_float(noiseTilingOffset, _SubwaveScale, noise);

                float2 offset = { sin(_Time.y) * _WaveMultiplier, cos(_Time.y) * _WaveMultiplier };
                float2 textureUV = { i.uv.x + offset.x, i.uv.y + offset.y };

                fixed4 color = tex2D(_MainTex, textureUV);
                fixed4 deep = pow(color, _DeepPower);
                return lerp(deep,color,noise);
            }
            ENDCG
        }
        }
}
