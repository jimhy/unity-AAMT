Shader "Custom/Map/Grass"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Noise("Noise", 2D) = "black" {}

        _WindControl("WindControl(x:XSpeed y:YSpeed z:ZSpeed w:windMagnitude)",vector) = (1,0,1,0.5)
        //前面几个分量表示在各个轴向上自身摆动的速度, w表示摆动的强度
        _WaveControl("WaveControl(x:XSpeed y:YSpeed z:ZSpeed w:worldSize)",vector) = (1,0,1,1)
        //前面几个分量表示在各个轴向上风浪的速度, w用来模拟地图的大小,值越小草摆动的越凌乱，越大摆动的越整体
        _ForIntensity ("For Intensity", Range(0,5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Blend SrcAlpha OneMinusSrcAlpha
        //Blend One One
            Cull Off
            ZWrite Off
            Lighting Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #pragma multi_compile_fog

            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON ///// LightMap打开或者关闭
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 texUV2 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(1)
                #ifdef LIGHTMAP_ON       
                float2 uv2 : TEXCOORD2;  ///// 如果有烘焙图的话，定义lightMapUV
                #endif  
                
                //float3 tempCol : TEXCOORD1;//用来测试传递noise贴图采样的结果
            };

            uniform sampler2D _MainTex;
            uniform half4 _MainTex_ST;
            uniform sampler2D _Noise;
            uniform half4 _WindControl;
            uniform half4 _WaveControl;
            uniform half4 _TintColor;
            uniform half _ForIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                half4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                half2 samplePos = worldPos.xz / _WaveControl.w;
                samplePos += _Time.x * -_WaveControl.xz;
                fixed waveSample = tex2Dlod(_Noise, float4(samplePos, 0, 0)).r;
                worldPos.x += sin(waveSample * _WindControl.x) * _WaveControl.x * _WindControl.w * v.uv.y;
                worldPos.z += sin(waveSample * _WindControl.z) * _WaveControl.z * _WindControl.w * v.uv.y;
                o.pos = mul(UNITY_MATRIX_VP, worldPos);
                o.uv =v.uv;
                UNITY_TRANSFER_FOG(o,o.pos);
                #ifdef LIGHTMAP_ON 
                o.uv2 = v.texUV2.xy * unity_LightmapST.xy + unity_LightmapST.zw;/////如果有红配图，算UV
                #endif
              //  o.worlduv = 
                //o.tempCol = waveSample;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
                #ifdef LIGHTMAP_ON
                fixed3 lm = ( DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv2)));/////
                col.rgb *= lm  ;/////如果有烘焙图，将烘焙图参与计算
                #endif
               UNITY_APPLY_FOG(pow(i.fogCoord,_ForIntensity), col);
                return col;
            }
            ENDCG
        }
    }
}