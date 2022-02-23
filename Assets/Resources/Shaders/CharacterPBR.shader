
Shader "Custom/CharacterPBR" {
	Properties{
		//_Roughness("Roughness", Range(0, 1)) = 0
		//_Metallic("Metallic", Range(0, 1)) = 0
		//_AlbedoMap("Albedo Map", 2D) = "white" {}
		_AlphaValue("Alpha Value", Range(0.0,1.0)) = 1.0
		_MainTex("Main_Tex", 2D) = "white" {}
		_NormMap("Normal Map", 2D) = "white" {}
		_BumpScale("BumpScale",Range(0,2))=1
		_EnvMap("Environment Map", Cube) = "white" {}
		_MaskMap("Mask Map", 2D) = "white" {}
		_SSSIntensity("SSS Intensity", Range(0, 2)) = 1
		_SSSMap("SSS Map", 2D) = "white" {}
		[Toggle] _Metal("Metal On", Float) = 1
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode("Cull Mode", Float) = 2
		[Toggle] _Clip("Clip On", Float) = 0
		_Cutoff("Clip cutoff", Range(0,.9)) = .5
		[Toggle] _Emissive("Emissive On", Float) = 0
		[HDR]_EmissiveColor("Emissive Color", Color) = (1, 1, 1, 1)
		[KeywordEnum(None, ViewDiffuse, ViewDiffuseSpecular)] _VirtualLightMode("Virtual Light Mode", Float) = 0
		_VirtualLightColor("Virtual Light Color", Color) = (0.4, 0.4, 0.4, 0.4)

			[Header(_____________Shadow____________)]
		_ShadowColor("ShadowColor",color)=(0,0,0,0)
		_ShadowFalloff("ShadowFalloff",Range(0,1))=1
		_LightDir("LightDir",vector)=(1,1,1,1)
	}

	SubShader{
		Tags{
			"RenderType" = "Opaque"
		}
		Blend Off
		Cull[_CullMode]

		Pass{
			Name "ForwardBase"
			Tags{"Queue"="Transparent" "RenderType" = "Transparent" "LightMode" = "ForwardBase"}
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#pragma fragmentoption ARB_precision_hint_fastest	
			#pragma multi_compile _DUMMY _METAL_ON
			#pragma multi_compile _DUMMY _CLIP_ON
			#pragma multi_compile _DUMMY _EMISSIVE_ON
			#pragma shader_feature _VIRTUALLIGHTMODE_NONE _VIRTUALLIGHTMODE_VIEWDIFFUSE _VIRTUALLIGHTMODE_VIEWDIFFUSESPECULAR

			#include "AutoLight.cginc"
			#include "UnityCG.cginc"
			#include "BRDF.cginc"

			//uniform float _Roughness;
			//uniform float _Metallic;
			uniform float4 _LightColor0;
			uniform float _Cutoff;
			//uniform sampler2D _AlbedoMap;
			uniform sampler2D _MainTex;
			uniform sampler2D _NormMap;
			uniform float _BumpScale;
			uniform sampler2D _MaskMap;
			uniform sampler2D _SSSMap;
			UNITY_DECLARE_TEXCUBE(_EnvMap);
			uniform float _SSSIntensity;
			uniform float3 _VirtualLightColor;
			uniform float4 _EmissiveColor;
			float _AlphaValue;
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 space0 : TEXCOORD1;
				float4 space1 : TEXCOORD2;
				float4 space2 : TEXCOORD3;
				float4 ambient_or_lightmap_uv : TEXCOORD4;
				UNITY_SHADOW_COORDS(5)
			};

			v2f vert(appdata_full v)
			{
				v2f o = (v2f)0;
				o.uv = v.texcoord.xy;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 world_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 world_normal = UnityObjectToWorldNormal(v.normal);
				float3 world_tangent = UnityObjectToWorldDir(v.tangent.xyz);
				float tangent_sign = v.tangent.w * unity_WorldTransformParams.w;
				float3 world_binormal = cross(world_normal, world_tangent) * tangent_sign;
				o.space0 = float4(world_tangent.x, world_binormal.x, world_normal.x, world_pos.x);
				o.space1 = float4(world_tangent.y, world_binormal.y, world_normal.y, world_pos.y);
				o.space2 = float4(world_tangent.z, world_binormal.z, world_normal.z, world_pos.z);
				o.ambient_or_lightmap_uv = 0;
				o.ambient_or_lightmap_uv.rgb = GetSHEvalLinearL0L1(world_normal);
				TRANSFER_SHADOW(o)
				return o;
			}

			half3 GetSSSColor(half3 n, half3 l, half curvature)
			{
				half nol = dot(n, l);
				half3 color = tex2D(_SSSMap, half2(clamp(((nol * 0.5) + 0.5), 0.0, 1.0), curvature)).rgb;
				color = GammaToLinear(color);
				return color * _SSSIntensity;
			}

			half3 GetVirtualLight(half3 n, half3 v, half3 diffuse_color, half3 specular_color, half roughness, half occlusion)
			{
				half3 color = 0;
#if _VIRTUALLIGHTMODE_NONE
				return color;
#elif _VIRTUALLIGHTMODE_VIEWDIFFUSE
				half3 l = v;
				half nol = saturate(dot(l, n));
				half3 lit_color = nol * _VirtualLightColor;
				color = lit_color * diffuse_color;
#elif _VIRTUALLIGHTMODE_VIEWDIFFUSESPECULAR
				half3 l = v;
				half nol = saturate(dot(l, n));
				half3 lit_color = nol * _VirtualLightColor;
				half3 h = normalize(v + l);
				half noh = max(0, dot(n, h));
				half specular_term = GGX_Mobile(roughness, noh, h, n);
				color = (diffuse_color + specular_term * specular_color) * lit_color;
#endif
				return color;
			}

			half4 frag(v2f i) : COLOR
			{
				half4 albedo = tex2D(_MainTex, i.uv);
				half4 mask = tex2D(_MaskMap, i.uv);
				half3 base_color = albedo.rgb;
				base_color = GammaToLinear(base_color);

#if _CLIP_ON
				clip(albedo.a - _Cutoff);
#endif
				//half roughness = _Roughness;
				//half metallic = _Metallic;
				half roughness = mask.r;
				half metallic = mask.g;
				half occlusion = mask.b;

				half3 packed_normal = UnpackNormal(tex2D(_NormMap, i.uv)).rgb;
				half3 normal;
				normal.xy =packed_normal*_BumpScale;
				normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
				half sss_mask = 1.0 - packed_normal.z;

#if _METAL_ON
				half3 specular_color = lerp(0.04, base_color.rgb, metallic);
				half3 diffuse_color = (base_color - base_color * metallic) * 0.96;
#else
				half3 diffuse_color = base_color;
				half3 specular_color = 0.04;
#endif
				half3 world_pos = half3(i.space0.w, i.space1.w, i.space2.w);
				half3 v = normalize(_WorldSpaceCameraPos - world_pos);

				half3 n;
				n.x = dot(i.space0.xyz, normal);
				n.y = dot(i.space1.xyz, normal);
				n.z = dot(i.space2.xyz, normal);

				n = normalize(n);

				half nov = saturate(dot(n, v));
#if _METAL_ON
				half3 env_spec = EnvBRDFApprox(specular_color, roughness, nov);
#else
				half3 env_spec = EnvBRDFApproxNonmetal(roughness, nov);
#endif

				half3 color = 0;
				half3 r = reflect(-v, n);
				half3 l = normalize(_WorldSpaceLightPos0.xyz);
				half rol = saturate(dot(r, l));
				half nol = saturate(dot(n, l));
				//return nol.xxxx;
				half3 h = normalize(v + l);
				half noh = max(0, dot(n, h));
				half3 sh = i.ambient_or_lightmap_uv.rgb;
				half shadow = SHADOW_ATTENUATION(i);
				//return shadow.xxxx;
				half3 atten_color = _LightColor0.rgb * shadow;

				half3 smooth_normal = lerp(n, half3(i.space0.z, i.space1.z, i.space2.z), 0.5f);
				//smooth_normal = n;
				half3 sss = GetSSSColor(smooth_normal, l, sss_mask);

				half3 direct_diffuse = sss * atten_color;

				half3 indirect_diffuse = sh * occlusion;
				half3 diffuse = (direct_diffuse + indirect_diffuse) * diffuse_color;
				//return half4(diffuse, 1);
				half3 env_color = GetEnvMapCube(roughness, r, UNITY_PASS_TEXCUBE(_EnvMap));
				half3 indirect_specular = env_color * env_spec;

				//half specular_term = MobileGGX(n, v, l, roughness);
				half specular_term = GGX_Mobile(roughness, noh, h, n);
				//half specular_term = PhongApprox(roughness, rol);

				half3 direct_specular = atten_color * nol * specular_term * specular_color;
				//return half4(sqrt(direct_specular), 1);
				half3 specular = direct_specular + indirect_specular;
#if _EMISSIVE_ON
				half3 emissive = (mask.a) * _EmissiveColor.rgb;
#else
				half3 emissive = 0;
#endif
				//return half4(emissive, 1.0);
				color = diffuse + specular + emissive;
				color += GetVirtualLight(n, v, diffuse_color, specular_color, roughness, occlusion);
				color = LinearToGamma(color);
				return half4(color, _AlphaValue);
			}
		ENDCG
		}
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			Offset 1, 1
			ZWrite On
			Cull Off
			Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#define UNITY_PASS_SHADOWCASTER
			#pragma multi_compile _DUMMY _CLIP_ON
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			uniform float _Cutoff;
			uniform sampler2D _MainTex;

			struct v2f {
				V2F_SHADOW_CASTER;
				float2 uv : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o = (v2f)0;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = v.texcoord;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				half4 albedo = tex2D(_MainTex, i.uv);
#if _CLIP_ON
				clip(albedo.a - _Cutoff);
#endif
				SHADOW_CASTER_FRAGMENT(i)

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
		//ZWrite off

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
	//Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
}