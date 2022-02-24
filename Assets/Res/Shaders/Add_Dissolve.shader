Shader "Custom/Effects/Add_Dissolve"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1, 1, 1, 1)
        _DissolveTex("DissolveTex", 2D) = "white" {}
        _Dissolve("Dissolve", Range(0, 1)) = 0
        _Brightness("Bright", Range(0, 2)) = 1
	}
	SubShader
	{
		Tags { "IgnoreProjector"="True"
                "Queue"="Transparent"
                "RenderType"="Transparent" }
		

		Pass
		{
			Cull Off
			ZWrite off
            Lighting Off
            Blend SrcAlpha One

			CGPROGRAM
           
			#pragma vertex vert
			#pragma fragment frag
			
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				half2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD1;
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _DissolveTex;
			uniform fixed4 _MainTex_ST;
			uniform fixed4 _DissolveTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _DissolveTex);
				o.color = v.color;
				return o;
			}
			
            uniform fixed4 _TintColor;
            uniform fixed _Dissolve;
            uniform fixed _Brightness;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 disov = tex2D(_DissolveTex, i.uv2);
				disov.r += i.color.a - 1;
				clip(disov.r - _Dissolve);
				fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
				col.rgb *= i.color.rgb * _Brightness;
				return col;
			}
			ENDCG
		}
	}
}
