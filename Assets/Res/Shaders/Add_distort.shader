Shader "Custom/Effects/Add_distort" 
{
    Properties 
    {
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Maintex ("Maintex", 2D) = "white" {}
        _niuqu ("niuqu_tex", 2D) = "white" {}
        _QD ("QD", Float ) = 0.05
        _GLOW ("GLOW", Float ) = 1
        _V ("V", Float ) = 0
        _U ("U", Float ) = 0
        _VM ("VM", Float ) = 0
        _UM ("UM", Float ) = 0
    }
    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True"}
       
        Pass {
            Blend One One
            Cull Off
            ZWrite Off
            Lighting Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            uniform sampler2D _Maintex; 
            uniform float4 _Maintex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _niuqu; 
            uniform float4 _niuqu_ST;
            uniform float _QD;
            uniform float _GLOW;
            uniform float _V;
            uniform float _U;
            uniform float _VM;
            uniform float _UM;
            struct a2v {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            v2f vert (a2v v) {
                v2f o;
                o.uv = v.texcoord;

                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(v2f i, float facing : VFACE) : COLOR 
            {

                float2 timeUV = float2(( _U *_Time.y),( _V *_Time.y)) + i.uv;
                float4 timeniuqu = tex2D(_niuqu,TRANSFORM_TEX(timeUV, _niuqu));
                float2 timeUVMain = float2(( _UM *_Time.y),( _VM *_Time.y)) + i.uv;
                float2 timeniuquR = ((timeniuqu.r * _QD) + timeUVMain);
                float4 _Maintex_var = tex2D(_Maintex,TRANSFORM_TEX(timeniuquR, _Maintex));
                float3 emissive = (((2.0*timeniuqu.rgb)*_Maintex_var.rgb)*(_Maintex_var.rgb*i.vertexColor.rgb*(_TintColor.rgb*_GLOW*_TintColor.a)*_Maintex_var.a*i.vertexColor.a));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
}
}
}