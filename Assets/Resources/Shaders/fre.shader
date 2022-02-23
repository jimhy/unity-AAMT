// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/fre"
{
	Properties
	{
		_fanguang("fanguang", Float) = 0.2
		_normal("normal", 2D) = "bump" {}
		_zifaguang("zifaguang", 2D) = "white" {}
		_jinshu("jinshu", Float) = 2
		_pinghua("pinghua", Float) = 0
		_fre_scale_power("fre_scale_power", Vector) = (1,5,0,0)
		_fre_int("fre_int", Float) = 5
		[HDR]_fre_col("fre_col", Color) = (1,0.8793501,0.5613208,1)
		_noise001("noise001", 2D) = "white" {}
		_noise001_tiling_speed("noise001_tiling_speed", Vector) = (0,0,0,0)
		_noise002("noise002", 2D) = "white" {}
		_noise002_tiling_speed("noise002_tiling_speed", Vector) = (0,0,0,0)
		[HDR]_noise_col("noise_col", Color) = (1,0.8793501,0.5613208,1)
		_nosie_int("nosie_int", Float) = 0
		[Enum(frezuo_mask,0,zifaguang_alp zuo_mask,1)]_noise_mask_panding("noise_mask_panding", Float) = 0
		[Toggle(_TONGDAOKAIGUAN_ON)] _tongdaokaiguan("tongdaokaiguan", Float) = 0
		[Enum(fre,0,zafaguang_alp,1)]_panding_zhezhao("panding_zhezhao", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _TONGDAOKAIGUAN_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _normal;
		uniform float4 _normal_ST;
		uniform half _fanguang;
		uniform sampler2D _zifaguang;
		uniform float4 _zifaguang_ST;
		uniform half4 _fre_col;
		uniform half2 _fre_scale_power;
		uniform half _fre_int;
		uniform sampler2D _noise002;
		uniform half4 _noise002_tiling_speed;
		uniform sampler2D _noise001;
		uniform half4 _noise001_tiling_speed;
		uniform half4 _noise_col;
		uniform half _nosie_int;
		uniform half _noise_mask_panding;
		uniform float _jinshu;
		uniform half _pinghua;
		uniform half _panding_zhezhao;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_normal = i.uv_texcoord * _normal_ST.xy + _normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _normal, uv_normal ) );
			half3 temp_cast_0 = (_fanguang).xxx;
			o.Albedo = temp_cast_0;
			float2 uv_zifaguang = i.uv_texcoord * _zifaguang_ST.xy + _zifaguang_ST.zw;
			float4 tex2DNode16 = tex2D( _zifaguang, uv_zifaguang );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNdotV2 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode2 = ( 0.0 + _fre_scale_power.x * pow( 1.0 - fresnelNdotV2, _fre_scale_power.y ) );
			float2 appendResult39 = (float2(_noise002_tiling_speed.z , _noise002_tiling_speed.w));
			float2 appendResult36 = (float2(_noise001_tiling_speed.z , _noise001_tiling_speed.w));
			float2 appendResult35 = (float2(_noise001_tiling_speed.x , _noise001_tiling_speed.y));
			float2 uv_TexCoord20 = i.uv_texcoord * appendResult35;
			float2 panner21 = ( 1.0 * _Time.y * appendResult36 + uv_TexCoord20);
			float2 appendResult38 = (float2(_noise002_tiling_speed.x , _noise002_tiling_speed.y));
			float2 uv_TexCoord25 = i.uv_texcoord * appendResult38;
			float2 panner26 = ( 1.0 * _Time.y * appendResult39 + ( tex2D( _noise001, panner21 ) + float4( uv_TexCoord25, 0.0 , 0.0 ) ).rg);
			float4 tex2DNode23 = tex2D( _noise002, panner26 );
			float4 lerpResult45 = lerp( ( fresnelNode2 * tex2DNode23.r * _noise_col * _nosie_int ) , ( tex2DNode23.r * _noise_col * _nosie_int * tex2DNode16.a ) , _noise_mask_panding);
			o.Emission = ( ( tex2DNode16 * tex2DNode16.a ) + ( _fre_col * ( fresnelNode2 * _fre_int * tex2DNode16.a ) ) + lerpResult45 ).rgb;
			o.Metallic = _jinshu;
			o.Smoothness = _pinghua;
			float lerpResult41 = lerp( fresnelNode2 , tex2DNode16.a , _panding_zhezhao);
			#ifdef _TONGDAOKAIGUAN_ON
				float staticSwitch40 = 0.0;
			#else
				float staticSwitch40 = lerpResult41;
			#endif
			o.Alpha = staticSwitch40;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
202;159;1446;797;2701.638;728.3746;2.688204;True;False
Node;AmplifyShaderEditor.Vector4Node;34;-2196.695,455.6375;Half;False;Property;_noise001_tiling_speed;noise001_tiling_speed;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,0.05,0.08;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;35;-1969.695,418.6375;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-1836.581,393.8208;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;36;-1801.695,527.6375;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;37;-1669.396,581.6699;Half;False;Property;_noise002_tiling_speed;noise002_tiling_speed;12;0;Create;True;0;0;0;False;0;False;0,0,0,0;2,2,-0.03,0.02;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;21;-1555.991,399.8194;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-1447.396,598.6699;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;22;-1387.781,373.9625;Inherit;True;Property;_noise001;noise001;9;0;Create;True;0;0;0;False;0;False;-1;None;e205a30ce515daa48a4757e2fc5d274b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1307.781,558.9625;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-1096.781,441.9625;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;-1226.396,679.6699;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;13;-1335.415,106.8199;Half;False;Property;_fre_scale_power;fre_scale_power;6;0;Create;True;0;0;0;False;0;False;1,5;1.5,1.95;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;26;-978.8974,443.4691;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;33;-770.2938,614.7125;Half;False;Property;_noise_col;noise_col;13;1;[HDR];Create;True;0;0;0;False;0;False;1,0.8793501,0.5613208,1;15.81176,47.93726,12.04706,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-712.9021,792.8386;Half;False;Property;_nosie_int;nosie_int;14;0;Create;True;0;0;0;False;0;False;0;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-813.5812,410.8625;Inherit;True;Property;_noise002;noise002;11;0;Create;True;0;0;0;False;0;False;-1;None;050e5d4abb6a9ea409513d486ab516f7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1009.032,275.3855;Half;False;Property;_fre_int;fre_int;7;0;Create;True;0;0;0;False;0;False;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;2;-1154.861,64.44111;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-741.5912,-275.3329;Inherit;True;Property;_zifaguang;zifaguang;2;0;Create;True;0;0;0;False;0;False;-1;None;8754e3c9237e0d24f9a65638010a3a73;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-355.2735,330.3577;Half;False;Property;_panding_zhezhao;panding_zhezhao;17;1;[Enum];Create;True;0;2;fre;0;zafaguang_alp;1;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-498.9362,602.8193;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-848.6086,-90.00447;Half;False;Property;_fre_col;fre_col;8;1;[HDR];Create;True;0;0;0;False;0;False;1,0.8793501,0.5613208,1;0.1098039,0.7137255,0.04705882,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-806.5429,120.4778;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-496.3681,748.9539;Half;False;Property;_noise_mask_panding;noise_mask_panding;15;1;[Enum];Create;True;0;2;frezuo_mask;0;zifaguang_alp zuo_mask;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-507.6075,414.4263;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;45;-326.7681,510.5538;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-580.0474,51.57288;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-445.2382,-237.8823;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.2971698;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;-168.2644,268.1069;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-281.7086,20.27664;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;106.9483,121.0981;Inherit;False;Property;_jinshu;jinshu;4;0;Create;True;0;0;0;False;0;False;2;-1.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;129.729,196.8637;Half;False;Property;_pinghua;pinghua;5;0;Create;True;0;0;0;False;0;False;0;1.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;60.40199,-381.5624;Half;False;Property;_fanguang;fanguang;0;0;Create;True;0;0;0;False;0;False;0.2;-0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;40;-23.77176,261.9078;Inherit;False;Property;_tongdaokaiguan;tongdaokaiguan;16;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-297.8312,-330.408;Inherit;True;Property;_normal;normal;1;0;Create;True;0;0;0;False;0;False;-1;None;e6a9a276e47bf5b488af62e2e9d9e38b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;5;370.4093,28.70482;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Unlit/fre;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;1;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;1
WireConnection;35;1;34;2
WireConnection;20;0;35;0
WireConnection;36;0;34;3
WireConnection;36;1;34;4
WireConnection;21;0;20;0
WireConnection;21;2;36;0
WireConnection;38;0;37;1
WireConnection;38;1;37;2
WireConnection;22;1;21;0
WireConnection;25;0;38;0
WireConnection;24;0;22;0
WireConnection;24;1;25;0
WireConnection;39;0;37;3
WireConnection;39;1;37;4
WireConnection;26;0;24;0
WireConnection;26;2;39;0
WireConnection;23;1;26;0
WireConnection;2;2;13;1
WireConnection;2;3;13;2
WireConnection;44;0;23;1
WireConnection;44;1;33;0
WireConnection;44;2;43;0
WireConnection;44;3;16;4
WireConnection;11;0;2;0
WireConnection;11;1;14;0
WireConnection;11;2;16;4
WireConnection;32;0;2;0
WireConnection;32;1;23;1
WireConnection;32;2;33;0
WireConnection;32;3;43;0
WireConnection;45;0;32;0
WireConnection;45;1;44;0
WireConnection;45;2;46;0
WireConnection;15;0;12;0
WireConnection;15;1;11;0
WireConnection;18;0;16;0
WireConnection;18;1;16;4
WireConnection;41;0;2;0
WireConnection;41;1;16;4
WireConnection;41;2;42;0
WireConnection;17;0;18;0
WireConnection;17;1;15;0
WireConnection;17;2;45;0
WireConnection;40;1;41;0
WireConnection;5;0;6;0
WireConnection;5;1;19;0
WireConnection;5;2;17;0
WireConnection;5;3;8;0
WireConnection;5;4;9;0
WireConnection;5;9;40;0
ASEEND*/
//CHKSM=1A3449BA7EDD46C6D500456A93DF07EDDAE93724