// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/ruanbianrongje"
{
	Properties
	{
		[Enum(particle,0,material,1)]_fangshipanding("fangshi-panding", Float) = 0
		_shuchu_tex("shuchu_tex", 2D) = "white" {}
		[HDR]_shuchutex_col("shuchutex_col", Color) = (1,1,1,1)
		_noise01("noise01", 2D) = "white" {}
		[HDR]_rongjie_col("rongjie_col", Color) = (2,0.4627451,0.4627451,1)
		_rongjie("rongjie", Range( 0 , 1)) = 0.1449917
		_rongjie_liangbian_kuan("rongjie_liangbian_kuan", Range( 0 , 1)) = 1
		_ruanyingdu("ruanyingdu", Range( 0 , 0.99)) = 0.7957165
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half4 _rongjie_col;
			uniform half _fangshipanding;
			uniform half4 _shuchutex_col;
			uniform sampler2D _shuchu_tex;
			uniform half4 _shuchu_tex_ST;
			uniform sampler2D _noise01;
			uniform half4 _noise01_ST;
			uniform half _rongjie;
			uniform half _rongjie_liangbian_kuan;
			uniform half _ruanyingdu;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				half4 texCoord82 = v.ase_texcoord1;
				texCoord82.xy = v.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				half3 appendResult95 = (half3(_rongjie , _rongjie_liangbian_kuan , _ruanyingdu));
				half4 lerpResult92 = lerp( texCoord82 , half4( appendResult95 , 0.0 ) , _fangshipanding);
				half3 break94 = lerpResult92.xyz;
				half temp_output_71_0 = ( break94.x * ( break94.y + 1.0 ) );
				half myVarName00176 = break94.z;
				half temp_output_57_0 = ( 1.0 - myVarName00176 );
				half vertexToFrag80 = ( temp_output_71_0 * ( 1.0 + temp_output_57_0 ) );
				o.ase_texcoord2.z = vertexToFrag80;
				half vertexToFrag81 = ( ( temp_output_71_0 - break94.y ) * ( 1.0 + temp_output_57_0 ) );
				o.ase_texcoord2.w = vertexToFrag81;
				
				o.ase_texcoord1 = v.ase_texcoord2;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				o.ase_texcoord3 = v.ase_texcoord1;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				half4 texCoord89 = i.ase_texcoord1;
				texCoord89.xy = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				half myVarName00490 = _fangshipanding;
				half4 lerpResult88 = lerp( texCoord89 , _rongjie_col , myVarName00490);
				float2 uv_shuchu_tex = i.ase_texcoord2.xy * _shuchu_tex_ST.xy + _shuchu_tex_ST.zw;
				half4 tex2DNode54 = tex2D( _shuchu_tex, uv_shuchu_tex );
				float2 uv_noise01 = i.ase_texcoord2.xy * _noise01_ST.xy + _noise01_ST.zw;
				half temp_output_65_0 = ( tex2D( _noise01, uv_noise01 ).r + 1.0 );
				half vertexToFrag80 = i.ase_texcoord2.z;
				half4 texCoord82 = i.ase_texcoord3;
				texCoord82.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				half3 appendResult95 = (half3(_rongjie , _rongjie_liangbian_kuan , _ruanyingdu));
				half4 lerpResult92 = lerp( texCoord82 , half4( appendResult95 , 0.0 ) , _fangshipanding);
				half3 break94 = lerpResult92.xyz;
				half temp_output_7_0_g16 = break94.z;
				half4 lerpResult66 = lerp( lerpResult88 , ( _shuchutex_col * tex2DNode54 * i.ase_color ) , saturate( ( ( ( temp_output_65_0 - vertexToFrag80 ) - temp_output_7_0_g16 ) / ( 1.0 - temp_output_7_0_g16 ) ) ));
				half vertexToFrag81 = i.ase_texcoord2.w;
				half temp_output_7_0_g17 = break94.z;
				half4 appendResult14 = (half4(lerpResult66.rgb , ( _shuchutex_col.a * tex2DNode54.a * i.ase_color.a * saturate( ( ( ( temp_output_65_0 - vertexToFrag81 ) - temp_output_7_0_g17 ) / ( 1.0 - temp_output_7_0_g17 ) ) ) )));
				
				
				finalColor = appendResult14;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;36;1760;983;3000.865;767.8969;1.983262;True;False
Node;AmplifyShaderEditor.RangedFloatNode;58;-2445.171,386.8116;Half;False;Property;_rongjie;rongjie;5;0;Create;True;0;0;0;False;0;False;0.1449917;0.563;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2443.372,456.6738;Half;False;Property;_rongjie_liangbian_kuan;rongjie_liangbian_kuan;6;0;Create;True;0;0;0;False;0;False;1;0.575;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-2442.955,530.501;Half;False;Property;_ruanyingdu;ruanyingdu;7;0;Create;True;0;0;0;False;0;False;0.7957165;0.662;0;0.99;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;95;-2179.108,433.9896;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-2263.587,222.9398;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;-2165.305,549.3386;Half;False;Property;_fangshipanding;fangshi-panding;0;1;[Enum];Create;True;0;2;particle;0;material;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;-2034.802,365.384;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;94;-1891.869,362.4372;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RegisterLocalVarNode;76;-829.9929,456.2585;Inherit;False;myVarName001;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;77;-1813.146,679.5145;Inherit;False;76;myVarName001;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-1741.555,552.1996;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1646.893,345.932;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;57;-1623.137,682.511;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1451.543,654.2703;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-1527.637,424.4388;Inherit;False;2;2;0;FLOAT;1;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;79;-1459.863,559.0835;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1373.718,347.7463;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-1308.63,566.7438;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-1521.584,164.4051;Inherit;True;Property;_noise01;noise01;3;0;Create;True;0;0;0;False;0;False;-1;385330324ee1d8146910db0844b31677;385330324ee1d8146910db0844b31677;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexToFragmentNode;81;-1187.775,565.6161;Inherit;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1206.384,193.8883;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;80;-1187.235,326.2925;Inherit;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;-1981.759,551.8254;Inherit;False;myVarName004;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;-695.8846,-341.9429;Half;False;Property;_rongjie_col;rongjie_col;4;1;[HDR];Create;True;0;0;0;False;0;False;2,0.4627451,0.4627451,1;1,0.4198113,0.4198113,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;55;-952.0918,-274.9565;Half;False;Property;_shuchutex_col;shuchutex_col;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;54;-1038.092,-108.9563;Inherit;True;Property;_shuchu_tex;shuchu_tex;1;0;Create;True;0;0;0;False;0;False;-1;f5adae8c01dcb404cac0203d6f77ca57;686608f8d4031af49ba335237e039b92;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;63;-967.507,233.0453;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;56;-916.673,72.50881;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;-984.5446,539.4921;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;89;-705.6129,-501.6628;Inherit;False;2;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;91;-694.1204,-177.789;Inherit;False;90;myVarName004;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;-497.2362,-363.877;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;47;-770.4442,540.9803;Inherit;True;smoothstep_simple;-1;;17;7661721d9dbbde54b80cc573d1a19442;0;3;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;62;-752.9394,236.8148;Inherit;True;smoothstep_simple;-1;;16;7661721d9dbbde54b80cc573d1a19442;0;3;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-751.0154,-125.4295;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-406.9365,308.8851;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;66;-495.5358,82.45856;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;-259.5225,83.6619;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-45.77669,82.4586;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/ruanbianrongje;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;95;0;58;0
WireConnection;95;1;24;0
WireConnection;95;2;59;0
WireConnection;92;0;82;0
WireConnection;92;1;95;0
WireConnection;92;2;87;0
WireConnection;94;0;92;0
WireConnection;76;0;94;2
WireConnection;70;0;94;1
WireConnection;71;0;94;0
WireConnection;71;1;70;0
WireConnection;57;0;77;0
WireConnection;51;1;57;0
WireConnection;60;1;57;0
WireConnection;79;0;71;0
WireConnection;79;1;94;1
WireConnection;61;0;71;0
WireConnection;61;1;60;0
WireConnection;52;0;79;0
WireConnection;52;1;51;0
WireConnection;81;0;52;0
WireConnection;65;0;64;1
WireConnection;80;0;61;0
WireConnection;90;0;87;0
WireConnection;63;0;65;0
WireConnection;63;1;80;0
WireConnection;23;0;65;0
WireConnection;23;1;81;0
WireConnection;88;0;89;0
WireConnection;88;1;67;0
WireConnection;88;2;91;0
WireConnection;47;6;23;0
WireConnection;47;7;94;2
WireConnection;62;6;63;0
WireConnection;62;7;94;2
WireConnection;12;0;55;0
WireConnection;12;1;54;0
WireConnection;12;2;56;0
WireConnection;83;0;55;4
WireConnection;83;1;54;4
WireConnection;83;2;56;4
WireConnection;83;3;47;0
WireConnection;66;0;88;0
WireConnection;66;1;12;0
WireConnection;66;2;62;0
WireConnection;14;0;66;0
WireConnection;14;3;83;0
WireConnection;1;0;14;0
ASEEND*/
//CHKSM=55DF335124516ED9E5AA5C9DFD4990B728984F04