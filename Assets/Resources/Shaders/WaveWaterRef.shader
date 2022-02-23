Shader "Custom/Map/WaveWaterRef" {
	Properties {
		_BumpTex ("Bump Tex", 2D) = "bump" {} 
		_BumpPower1("BumpPower1",Range(0,1))=1
		_BumpPower2("BumpPower2",Range(0,1))=1


		_MaskTex ("MaskTex", 2D) = "white" {} //海水渐变
		_AlphaPower("AlphaPower",Range(-1,5))=1
		_WaveXSpeed ("WaveXSpeed",Range(-1,1))=0.05
		_WaveYSpeed ("WaveYSpeed",Range(-1,1))=0.05
		

		_Reflection("Reflection", 2D) = "black" {}
		_ReflectPower("ReflectPower",Range(0,1))=0
		
		_DistortionPower("DistortionPower",Range(0,0.2))=0

		 _Specular("Specular" , range(0,5)) = 1
		 _SpecularColor ( "Specular Color " , color ) = (1,1,1,1)
        _Glossiness( "Gloss Siness" , float ) =1
        _SpecularShiness("Specular Shiness " , float ) = 1
		_SpecularShinessColor( "Specular Shiness Color " , color ) = (1,1,1,1)

		_FresnelScale("Fresnel Scale", Float) = 0.5
		_CubeMap("CubeMap", CUBE) = ""{}
	}

	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		Pass
		{
			Tags{"LightMode"="ForwardBase"}
			Cull Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
		CGPROGRAM
		#pragma multi_compile_fwdbase
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"


		sampler2D _BumpTex;
		float4 _BumpTex_ST;
		sampler2D _MaskTex;
		fixed _MaskTex_ST;
		
		fixed _AlphaPower;
		fixed _WaveXSpeed;
		fixed _WaveYSpeed;
		fixed _BumpPower1;
		fixed _BumpPower2;
		    float _Specular;
            fixed _Glossiness;
            fixed4 _SpecularColor;
            float _SpecularShiness;
            float4 _SpecularShinessColor;


			float _FresnelScale;
			samplerCUBE _CubeMap;

		
		float _Gloss;
		uniform sampler2D _Reflection;
		float4 _Reflection_ST;
		float _ReflectPower;

		struct a2v {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float4 texcoord : TEXCOORD0;
		};
		struct v2f {
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
			float4 TtoW0 : TEXCOORD1;
			float4 TtoW1 : TEXCOORD2;
			float4 TtoW2 : TEXCOORD3;
			fixed3 worldRefr : TEXCOORD4;	
		};

		v2f vert (a2v v){
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = v.texcoord.xy;
			o.uv.zw = v.texcoord.xy * _BumpTex_ST.xy + _BumpTex_ST.zw;

			TANGENT_SPACE_ROTATION;
			float3 worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
			fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
			fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
			fixed3 worldBinormal = cross(worldNormal,worldTangent) * v.tangent.w;

			o.TtoW0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x,worldPos.x);
			o.TtoW1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y,worldPos.y);
			o.TtoW2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z,worldPos.z);
			
			return o;
			}
		fixed4 frag (v2f i) : SV_Target{
			float3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);
			fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
			fixed3 viewDir  = normalize(UnityWorldSpaceViewDir(worldPos));
			fixed4 texMask  = tex2D(_MaskTex,i.uv.xy);

			float2 speed = _Time.y * float2(_WaveXSpeed,_WaveYSpeed);
			fixed3 bump1 = UnpackNormal(tex2D(_BumpTex,i.uv.zw+speed)).rgb * _BumpPower1;
			fixed3 bump2 = UnpackNormal(tex2D(_BumpTex,i.uv.zw-speed)).rgb * (1- _BumpPower1);
			fixed3 bump  = normalize(bump1+bump2);
			bump = normalize(half3(dot(i.TtoW0.xyz,bump),dot(i.TtoW1.xyz,bump),dot(i.TtoW2.xyz,bump)))*_BumpPower2;
			//------------------------------------
			float4 ViewDirection = normalize(float4(viewDir.x,viewDir.y,viewDir.z*10,0));
			float2 ReflectUV = float2( (ViewDirection.x+1) * 0.5 ,(ViewDirection.y+1) * 0.5);
			float4 ReflectTex = tex2D(_Reflection,ReflectUV.xy + bump);
			float3 Fresnel = float3(0,0,0.5);
			float3 FresnelR = (1.0-dot(viewDir.xyz,Fresnel)) * _ReflectPower * ReflectTex;

			float3 worldReflect = reflect(-ViewDirection,normalize(bump));
			float4 fresnelReflectFactor = _FresnelScale + (1 - _FresnelScale)*pow(1-dot(ViewDirection,bump), 5);
            fixed4 colReflect = texCUBE(_CubeMap, normalize(worldReflect))* fresnelReflectFactor;
			

			float3 normalDetail = normalize( bump  * float3(i.TtoW0.z,i.TtoW1.z,i.TtoW2.z) );
			float3 halfDirection = normalize( lightDir + viewDir);	
            float nh = max(0, dot (bump, halfDirection));
            float nh1 = max(0, dot (normalDetail, halfDirection));
            float spec = pow (nh, _Specular*128.0);
            float shine = pow(nh1,_SpecularShiness*128.0) * _Glossiness ;
			float3 specular1 =spec*_SpecularColor*_LightColor0 + shine *_SpecularShinessColor*_LightColor0;
			float3 finalcolor =lerp(FresnelR+colReflect,specular1,0.7);


			return fixed4 (finalcolor,pow((1-texMask.r),_AlphaPower));
			}
		
		ENDCG
		} 
	}
	//FallBack "Diffuse"
}