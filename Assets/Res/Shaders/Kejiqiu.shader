// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effectrs/ASE/Kejiqiu"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		[HDR]_Tint("Tint", Color) = (1,1,1,1)
		[Toggle(_ISFRESNEL_ON)] _Isfresnel("Isfresnel", Float) = 0
		[HDR]_Fresnel_Tint("Fresnel_Tint", Color) = (0,0,0,0)
		_Fresnel_Power("Fresnel_Power", Range( 0 , 1)) = 0
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_NoisospeedUV("NoisospeedUV", Float) = 1
		_VertexOffets("VertexOffets", Float) = 0.2
		_Opacity_Intensity("Opacity_Intensity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _ISFRESNEL_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _NoiseTex;
		uniform float _NoisospeedUV;
		uniform float _VertexOffets;
		uniform float4 _Tint;
		uniform sampler2D _Main_Tex;
		uniform float4 _Main_Tex_ST;
		uniform float4 _Fresnel_Tint;
		uniform float _Fresnel_Power;
		uniform float _Opacity_Intensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_12_0 = ( _NoisospeedUV * _Time.x );
			float3 ase_vertexNormal = v.normal.xyz;
			float cos7 = cos( temp_output_12_0 );
			float sin7 = sin( temp_output_12_0 );
			float2 rotator7 = mul( (float3( 0,0,0 ) + ((ase_vertexNormal).xyz - float3( -1,-1,-1 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( -1,-1,-1 ))).xy - float2( 0,0 ) , float2x2( cos7 , -sin7 , sin7 , cos7 )) + float2( 0,0 );
			float2 panner8 = ( ( temp_output_12_0 * 0.5 ) * float2( 0,0 ) + rotator7);
			float4 tex2DNode15 = tex2Dlod( _NoiseTex, float4( panner8, 0, 0.0) );
			float4 Ve20 = ( tex2DNode15 * _VertexOffets * float4( ase_vertexNormal , 0.0 ) );
			v.vertex.xyz += Ve20.rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float4 tex2DNode27 = tex2D( _Main_Tex, uv_Main_Tex );
			float temp_output_12_0 = ( _NoisospeedUV * _Time.x );
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float cos7 = cos( temp_output_12_0 );
			float sin7 = sin( temp_output_12_0 );
			float2 rotator7 = mul( (float3( 0,0,0 ) + ((ase_vertexNormal).xyz - float3( -1,-1,-1 )) * (float3( 1,1,1 ) - float3( 0,0,0 )) / (float3( 1,1,1 ) - float3( -1,-1,-1 ))).xy - float2( 0,0 ) , float2x2( cos7 , -sin7 , sin7 , cos7 )) + float2( 0,0 );
			float2 panner8 = ( ( temp_output_12_0 * 0.5 ) * float2( 0,0 ) + rotator7);
			float4 tex2DNode15 = tex2D( _NoiseTex, panner8 );
			float4 NoiseTex_Temp31 = tex2DNode15;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV34 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode34 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV34, _Fresnel_Power ) );
			#ifdef _ISFRESNEL_ON
				float staticSwitch37 = fresnelNode34;
			#else
				float staticSwitch37 = 0.0;
			#endif
			float4 temp_output_40_0 = ( staticSwitch37 + ( tex2DNode27.r * NoiseTex_Temp31 ) );
			o.Emission = ( ( ( _Tint * tex2DNode27.r * NoiseTex_Temp31 ) + ( _Fresnel_Tint * temp_output_40_0 ) ) * i.vertexColor ).rgb;
			float grayscale45 = Luminance(temp_output_40_0.rgb);
			float clampResult46 = clamp( grayscale45 , 0.0 , 1.0 );
			o.Alpha = ( i.vertexColor.a * clampResult46 * _Opacity_Intensity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				half4 color : COLOR0;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
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
Version=17500
180;156;1740;694;1552.49;793.1214;1.3;True;True
Node;AmplifyShaderEditor.CommentaryNode;19;-2744.201,-327.8734;Inherit;False;1842.882;545.7136;Vect;15;8;7;9;11;12;14;13;15;2;6;16;17;18;1;31;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NormalVertexDataNode;1;-2694.201,-274.997;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;2;-2329.073,-277.8734;Inherit;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2428.003,-90.2447;Inherit;False;Property;_NoisospeedUV;NoisospeedUV;6;0;Create;True;0;0;False;0;1;0.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;11;-2445.929,-5.522922;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-2228.073,-39.87342;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2140.1,83.98917;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;6;-2139.073,-238.8734;Inherit;False;5;0;FLOAT3;0,0,0;False;1;FLOAT3;-1,-1,-1;False;2;FLOAT3;1,1,1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1926.099,-3.010818;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;7;-1958.073,-219.8734;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-1705.073,-169.8734;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;15;-1388.566,-192.9021;Inherit;True;Property;_NoiseTex;NoiseTex;5;0;Create;True;0;0;False;0;-1;a9f953c7353804247b8c3ed6e1c46a2e;bdbe94d7623ec3940947b62544306f1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-1099.104,-228.499;Inherit;False;NoiseTex_Temp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2430.705,-594.2028;Inherit;False;Property;_Fresnel_Power;Fresnel_Power;4;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;34;-2068.265,-703.9024;Inherit;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;27;-2016.664,-1198.074;Inherit;True;Property;_Main_Tex;Main_Tex;0;0;Create;True;0;0;False;0;-1;None;f0642b7757ccbb94e923c0be8643c483;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-1751.051,-753.7071;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-1723.526,-507.3513;Inherit;False;31;NoiseTex_Temp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;37;-1507.586,-726.0347;Inherit;False;Property;_Isfresnel;Isfresnel;2;0;Create;True;0;0;False;0;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1485.989,-613.4884;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;18;-2505.915,135.8402;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1467.459,48.52021;Inherit;False;Property;_VertexOffets;VertexOffets;7;0;Create;True;0;0;False;0;0.2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-1732.282,-1012.706;Inherit;False;31;NoiseTex_Temp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;-1668.023,-1357.377;Inherit;False;Property;_Tint;Tint;1;1;[HDR];Create;True;0;0;False;0;1,1,1,1;4.237095,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1250.562,-671.2726;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;24;-1617.08,-898.5743;Inherit;False;Property;_Fresnel_Tint;Fresnel_Tint;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,1.505882,4,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-1017.511,-840.4557;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1394.642,-1213.683;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1063.319,31.68778;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;45;-939.1296,-596.5458;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-797.7601,-1051.716;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;43;-805.387,-828.855;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;46;-718.251,-596.4851;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-798.353,167.3427;Inherit;False;Ve;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-701.251,-406.4851;Inherit;False;Property;_Opacity_Intensity;Opacity_Intensity;8;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;23;-561.6418,-1097.266;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;21;-378.1606,47.64625;Inherit;False;20;Ve;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-491.8236,-781.1315;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-436.251,-554.4851;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;22.09063,-488.002;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Effectrs/ASE/Kejiqiu;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;12;0;9;0
WireConnection;12;1;11;1
WireConnection;6;0;2;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;7;0;6;0
WireConnection;7;2;12;0
WireConnection;8;0;7;0
WireConnection;8;1;14;0
WireConnection;15;1;8;0
WireConnection;31;0;15;0
WireConnection;34;3;35;0
WireConnection;37;1;36;0
WireConnection;37;0;34;0
WireConnection;38;0;27;1
WireConnection;38;1;39;0
WireConnection;18;0;1;0
WireConnection;40;0;37;0
WireConnection;40;1;38;0
WireConnection;42;0;24;0
WireConnection;42;1;40;0
WireConnection;28;0;22;0
WireConnection;28;1;27;1
WireConnection;28;2;33;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;16;2;18;0
WireConnection;45;0;40;0
WireConnection;41;0;28;0
WireConnection;41;1;42;0
WireConnection;46;0;45;0
WireConnection;20;0;16;0
WireConnection;44;0;41;0
WireConnection;44;1;43;0
WireConnection;47;0;43;4
WireConnection;47;1;46;0
WireConnection;47;2;48;0
WireConnection;0;2;44;0
WireConnection;0;9;47;0
WireConnection;0;11;21;0
ASEEND*/
//CHKSM=8EF6442ED08A703D6C42E173F90F64C340170A3C