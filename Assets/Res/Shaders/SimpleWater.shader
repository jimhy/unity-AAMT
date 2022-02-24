Shader "Custom/Map/SimpleWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalTex ("Normal", 2D) = "bump" {}
        _SamplerNoise ("SamplerNoise", 2D) = "white" {}
        //x:WaveSpeed y:Reflection z:NoiseStrength w:DistortSpeed
        _WaveControl ("WaveControl", vector) = (0,0,0,0)
        
        _wavecolor("Wavecolor",color)=(1,1,1,1)
        _WaveIntensity ("WaveIntensity",Range(1,10))=1
        _WaveTex ("WaveTexture", 2D) = "black" {}
        _Vertexcol("Vertexcol",2D)= "black"{}
        
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True" 
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 color : COLOR;
            };

            struct v2f
            {    
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float4 color : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _NormalTex; float4 _NormalTex_ST;;
            sampler2D _SamplerNoise; float4 _SamplerNoise_ST;
            sampler2D _WaveTex; float4 _WaveTex_ST;
            sampler2D _Vertexcol; float4 _Vertexcol_ST;
            fixed4 _WaveControl;
            fixed4 _wavecolor;
            half _WaveIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                TANGENT_SPACE_ROTATION;
                o.viewDir = normalize(mul(rotation, ObjSpaceViewDir(v.vertex)));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 samplerNoise = fixed2(tex2D(_SamplerNoise, i.uv * _SamplerNoise_ST.xy + float2(_WaveControl.w * _Time.y, 0)).r * _WaveControl.z, 0);
                float2 samplerUV = float2(_Time.y * _WaveControl.x, 0);

                float3 normalDir = normalize(UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(i.uv, _NormalTex) + samplerUV * 0.25 + samplerNoise)));
                float vdn = saturate(pow(dot(i.viewDir, normalDir), _WaveControl.y));

                fixed distortNoise = tex2D(_SamplerNoise, i.uv * _SamplerNoise_ST.xy + samplerUV + samplerNoise).g;
                fixed3 vertexcol = tex2D(_Vertexcol,i.uv);
                fixed waveMask = saturate((vertexcol.g - distortNoise) * 30);
                fixed waveTex = tex2D(_WaveTex, i.uv * _WaveTex_ST.xy + samplerUV + samplerNoise).r * waveMask;

                fixed4 col = tex2D(_MainTex, i.uv + samplerNoise.x) * (2 - vdn);
              

                 col.rgb += waveTex*_wavecolor*_WaveIntensity;
                col.a =vertexcol.r;

                
                return col;
            }
            ENDCG
        }
    }
}