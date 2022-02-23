Shader "Custom/ice"
{
    Properties
    {
        
        [Header(___________Base Albedo)]
        
        [NoScaleOffset]_MainTex ("MainTex", 2D) = "white" {}
		[NoScaleOffset]_iceTex("iceTextuer", 2D) = "white" {}
        [Header(___________ice)]
        _icevalue("ice Value", Range( 0 , 1)) = 0
		_icelength("iceLength", Range( 0 , 1)) = 0.3
        [NoScaleOffset]_iceMask("ice Mask", 2D) = "white" {}
        _iceMaskTile("ice Mask Tile", Range( 0 , 1)) = 1
        [Header(___________Fresne)]
        _fresnelBase("FresnelBase", Range(0, 1)) = 1
        _fresnelScale("FresnelScale", Range(0, 5)) = 1
        _fresnelIndensity("FresnelIndensity", Range(0, 5)) = 1
        _fresnelCol("_FresnelCol", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+1" }
        Cull Back

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            //#include "Lighting.cginc"



            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 L : TEXCOORD1;
                float3 N : TEXCOORD2;
                float3 V : TEXCOORD3;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
			uniform sampler2D _iceTex;
			uniform float4 _iceTex_ST;
			uniform float _icevalue;
			uniform sampler2D _iceMask;
			uniform float _iceMaskTile;
			uniform float _icelength;
            uniform float _fresnelBase;
            uniform float _fresnelScale;
            uniform float _fresnelIndensity;
            uniform float4 _fresnelCol;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord * _iceTex_ST.xy + _iceTex_ST.zw;
                o.N =UnityObjectToWorldNormal(v.normal);
                o.V = WorldSpaceViewDir(v.vertex);
                float4 wPos = mul(unity_ObjectToWorld,v.vertex);
                half3 wNormal = UnityObjectToWorldNormal(v.normal);
                fixed NdotD = max(0,dot(wNormal,_icevalue));
                float4 _offsetMask = tex2Dlod( _iceMask, float4( (( _iceMaskTile * wNormal.xy )*1.0 + 0.5), 0, 0.0) );
                wPos.xyz +=float3 (0,-_icevalue*_icelength,0) * _offsetMask * NdotD;
                o.vertex=mul(UNITY_MATRIX_VP,wPos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv.xy);
				fixed4 ice = tex2D(_iceTex,i.uv.zw);

                
                float3 N = normalize(i.N);
                float3 L = normalize(float3(0.5,0.3,0.8));
                float3 V = normalize(i.V);
                col.rgb *= saturate(dot(N, L)) *0.5+0.5;

				col.rgb =lerp(col.rgb*0.7,ice.rgb*0.45,_icevalue) ;
                //菲尼尔公式
                float fresnel =_fresnelBase+ _fresnelScale*pow(1 - dot(N, V), _fresnelIndensity+1);

                col.rgb +=lerp(col.rgb, _fresnelCol, fresnel) * _fresnelCol.a;

                return col;
            }

            ENDCG
        }
    }
}