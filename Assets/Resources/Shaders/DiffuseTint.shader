Shader "Custom/Map/DiffuseTint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor("TintColor",color)=(1,1,1,1)
        _ForIntensity ("ForIntensity", float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Geometry"}
 
    Pass
    {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #pragma multi_compile_fog
//------------------add-------------------
        #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON ///// LightMap打开或者关闭
//------------------add-------------------
 
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
//------------------add-------------------
            float2 uvLM:TEXCOORD1;///// 第二套UV
//------------------add-------------------
 
        };
 
        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex: SV_POSITION;
            UNITY_FOG_COORDS(1)
            
//------------------add-------------------
            #ifdef LIGHTMAP_ON       
            half2 uvLM : TEXCOORD2;  ///// 如果有烘焙图的话，定义lightMapUV
            #endif  
//------------------add-------------------
                 
        };
 
        uniform sampler2D _MainTex;
        uniform float4 _MainTex_ST;
        uniform float4 _TintColor;
        uniform half _ForIntensity;
 
 
        v2f vert (appdata v)
        {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv,_MainTex);
            UNITY_TRANSFER_FOG(o,o.vertex);
 
//------------------add-------------------
            #ifdef LIGHTMAP_ON 
            o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;/////如果有红配图，算UV
            #endif
//------------------add-------------------
 
            return o;
        }
 
 
        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);
            
 
//------------------add-------------------
            #ifdef LIGHTMAP_ON
            fixed3 lm = ( DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));/////
            col.rgb *= lm * _TintColor;/////如果有烘焙图，将烘焙图参与计算
            #endif  
//------------------add-------------------
            UNITY_APPLY_FOG(i.fogCoord*_ForIntensity, col);
            return col;
        }
        ENDCG
      }
    }
}