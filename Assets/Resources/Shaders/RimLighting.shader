Shader "Custom/Effects/RimLighting" 
{
    Properties
    {
        _MainTex("Main Tex",2D) = "black"{}
        _MainColor("Main Color", Color) = (0.5,0.5,0.5,1)
        [Toggle] _RampOn("Ramp On", Float) = 0
        _RampTex("Ramp Tex",2D) = "black"{}
        [HDR]_RimColor("rim color",Color) = (1,1,1,1)//边缘颜色
        _RimPower ("rim power",range(1,10)) = 2//边缘强度
        _Transparent ("Transparent",range(0,1)) = 0//边缘强度
        _U_SP ("U_SP", Float ) = 0
    }
 
    SubShader
    {
        Tags { "IgnoreProjector"="True"
                "Queue"="Transparent"
                "RenderType"="Transparent" 
                }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include"UnityCG.cginc"
            #pragma multi_compile __ _RAMPON_ON
 
            struct v2f
            {
                float4 vertex:POSITION;
                float4 uv:TEXCOORD0;
                float4 NdotV:COLOR;
            };
 
            uniform sampler2D _MainTex;
            uniform sampler2D _RampTex;
            uniform float4 _RampTex_ST;
            uniform float4 _MainColor;  
            uniform float4 _RimColor;
            uniform float _RimPower;
            uniform float _Transparent;
            uniform float _U_SP;
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;  
                //float3 ViewPos = mul(UNITY_MATRIX_VP,v.vertex).xyz; 
                o.uv.xy = v.texcoord.xy;
                o.uv.z = worldPos.x;
                o.uv.w = worldPos.y + _Time.y * _U_SP;
                o.uv.zw *= _RampTex_ST.xy + _RampTex_ST.zw;
                float3 V = WorldSpaceViewDir(v.vertex);
                V = mul(unity_WorldToObject,V);//视方向从世界到模型坐标系的转换
                o.NdotV.x = saturate(dot(v.normal,normalize(V)));//必须在同一坐标系才能正确做点乘运算
                return o;
            }
 
            half4 frag(v2f i):COLOR
            {
                half4 c = tex2D(_MainTex,i.uv.xy) * _MainColor;
                half4 ramp = tex2D(_RampTex,i.uv.zw);
                //用视方向和法线方向做点乘，越边缘的地方，法线和视方向越接近90度，点乘越接近0.
                //用（1- 上面点乘的结果）*颜色，来反映边缘颜色情况
#if _RAMPON_ON
                c.rgb =10 * c *((cos( 40)+3)/8.0) + pow((1-i.NdotV.x) ,_RimPower)* _RimColor.rgb * ramp ;
                c.a=(1 - _Transparent *i.NdotV.x);
#else
                c.rgb += pow((1-i.NdotV.x) ,_RimPower)* _RimColor.rgb ;
                c.a=1 - _Transparent;
#endif
                return c ;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}