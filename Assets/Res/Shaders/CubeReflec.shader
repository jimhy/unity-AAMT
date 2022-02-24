Shader "Custom/CubeReflec"
{
	Properties
	{
		_Color("Diff Color", Color) = (1,1,1,1)
		_Diffuse("Diffuse",float )=1
		[NOScaleOffset]_MainTex ("MainTex", 2D) = "white" {}
		
		_Environ ("Environ", Cube) = ""{}
		_Specular("Specular", Range(0,9)) = 1
		[NOScaleOffset]_SpecTex("Spec Tex", 2D) = "white" {}

		[NOScaleOffset]_CubeLightMap("CubeLightMap", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimRange("Rim Offset", Range(0.3,16)) = 4
		_CapIntensity("Rim Num", Range(0,8)) = 0

		[Header(_____________Flow____________)]
		[Toggle] _FlowOn("Flow On", Float) = 0
		_Flow("Flow Texture",2D)="white"{}
		_Power ("Power", Range(0, 10)) = 1
		_FlowColor("Flow Color", Color) = (1,1,1,1)
        _U_SP ("U_SP", Float ) = 0
        _U_VP ("V_SP", Float ) = 0

		
		[Header(_____________Shadow____________)]
		_ShadowColor("ShadowColor",color)=(0,0,0,0)
		_ShadowFalloff("ShadowFalloff",Range(0,1))=1
		_LightDir("LightDir",vector)=(1,1,1,1)
	}
	SubShader
	{
		Tags { "LightMode"="Always" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#pragma multi_compile _DUMMY _FLOWON_ON
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 reflectionDir : TEXCOORD0;
				float4  uv : TEXCOORD1;
				fixed2 cap : COLOR;
			};
			
			uniform half4 _Color;
			uniform half _Diffuse;
			uniform sampler2D _MainTex;	
			uniform samplerCUBE _Environ;
			uniform half _Specular;
			uniform sampler2D _SpecTex;

			uniform half _RimRange;	
			uniform half _CapIntensity;
			uniform sampler2D _CubeLightMap;
			uniform fixed4 _RimColor;

			uniform sampler2D _Flow; 
			uniform float _Power;
			uniform half4 _FlowColor;
            uniform float _U_SP;
            uniform float _U_VP;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.texcoord.xy;

				o.uv.z =v.texcoord.x + _Time.y * _U_SP;
				o.uv.w =v.texcoord.y + _Time.y * _U_VP;

				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				float3 worldViewDir = WorldSpaceViewDir(v.vertex);
				o.reflectionDir = reflect(-worldViewDir, worldNormal);

				float3 viewnormal = mul(UNITY_MATRIX_IT_MV, v.normal);
				viewnormal = normalize(viewnormal);
				o.cap = viewnormal.xy * 0.5 + 0.5;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half4 maincol = tex2D(_MainTex,i.uv)*_Color*_Diffuse;	
				fixed4 specTex = tex2D(_SpecTex, i.uv);
				fixed4 reflec = texCUBE(_Environ, i.reflectionDir) * _Specular * specTex;

				fixed4 cap = tex2D(_CubeLightMap, i.cap);
				fixed4 rim = pow(cap,_RimRange);	
				rim.rgb*=_CapIntensity * _RimColor -1.5;
#if _FLOWON_ON
				fixed4 lightTexA=tex2D(_Flow,i.uv.zw).r * _Power  * specTex; 
#else
				fixed4 lightTexA=0;
#endif
				half4 col = (maincol + reflec + rim) + (lightTexA * _FlowColor);
				
				return col;
			}
			ENDCG
		}
	

		Pass
	{
		
		Tags{"Queue"="Transparent+1" }
		//用使用模板测试以保证alpha显示正确
		Stencil
		{
			Ref 0
			Comp equal
			Pass incrWrap
			Fail keep
			ZFail keep
		}

		//透明混合模式
		Blend SrcAlpha OneMinusSrcAlpha
	
		//关闭深度写入
		ZWrite off

		//深度稍微偏移防止阴影与地面穿插
		Offset -1 , 0

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		struct appdata
		{
			float4 vertex : POSITION;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
		};

		uniform half4 _LightDir;
		uniform half4 _ShadowColor;
		uniform fixed _ShadowFalloff;

		float3 ShadowProjectPos(float4 vertPos)
		{
			float3 shadowPos;

			//得到顶点的世界空间坐标
			float3 worldPos = mul(unity_ObjectToWorld , vertPos).xyz;

			//灯光方向
			float3 lightDir = normalize(_LightDir.xyz);

			//阴影的世界空间坐标（低于地面的部分不做改变）
			shadowPos.y = min(worldPos .y , _LightDir.w);
			shadowPos.xz = worldPos .xz - lightDir.xz * max(0 , worldPos .y - _LightDir.w) / lightDir.y; 

			return shadowPos;
		}

		v2f vert (appdata v)
		{
			v2f o;

			//得到阴影的世界空间坐标
			float3 shadowPos = ShadowProjectPos(v.vertex);

			//转换到裁切空间
			o.vertex = UnityWorldToClipPos(shadowPos);

			//得到中心点世界坐标
			float3 center =float3( unity_ObjectToWorld[0].w , _LightDir.w , unity_ObjectToWorld[2].w);
			//计算阴影衰减
			float falloff = 1-saturate(distance(shadowPos , center) * _ShadowFalloff);

			//阴影颜色
			o.color = _ShadowColor; 
			o.color.a *= falloff;

			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			return i.color;
		}
		ENDCG
	}
}
}