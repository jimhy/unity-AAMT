// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/moxing_fresnel"
{
	Properties
	{
		_mod_tex("mod_tex", 2D) = "white" {}
		_moxing_col("moxing_col", Color) = (1,1,1,1)
		_fre("fre", Range( 0 , 1)) = 0
		[HDR]_fre_col("fre_col", Color) = (0,0,0,0)
		_fre_int("fre_int", Float) = 0
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
				half3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half4 _fre_col;
			uniform half _fre_int;
			uniform half _fre;
			uniform half4 _moxing_col;
			uniform sampler2D _mod_tex;
			uniform half4 _mod_tex_ST;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				half3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				half dotResult132 = dot( ase_worldViewDir , ase_worldNormal );
				half temp_output_7_0_g1 = _fre;
				half vertexToFrag170 = saturate( ( ( ( 1.0 - max( dotResult132 , 0.0 ) ) - temp_output_7_0_g1 ) / ( 1.0 - temp_output_7_0_g1 ) ) );
				o.ase_texcoord1.x = vertexToFrag170;
				
				o.ase_texcoord1.yz = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
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
				half vertexToFrag170 = i.ase_texcoord1.x;
				float2 uv_mod_tex = i.ase_texcoord1.yz * _mod_tex_ST.xy + _mod_tex_ST.zw;
				half4 tex2DNode167 = tex2D( _mod_tex, uv_mod_tex );
				half4 appendResult172 = (half4(( ( _fre_col * _fre_int * vertexToFrag170 ) + ( _moxing_col * tex2DNode167 * i.ase_color ) ).rgb , ( _moxing_col.b * tex2DNode167.a * i.ase_color.a * vertexToFrag170 )));
				
				
				finalColor = appendResult172;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;232;1920;741;3026.548;460.8231;2.715993;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;135;-1050.294,440.4473;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;134;-1073.705,586.364;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;132;-863.4971,464.9103;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;162;-652.787,457.6486;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;163;-525.7269,458.2043;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-618.277,661.2334;Half;False;Property;_fre;fre;2;0;Create;True;0;0;0;False;0;False;0;0.475;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;164;-348.277,465.2334;Inherit;True;smoothstep_simple;-1;;1;7661721d9dbbde54b80cc573d1a19442;0;3;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-194.0343,-50.74646;Half;False;Property;_fre_int;fre_int;4;0;Create;True;0;0;0;False;0;False;0;19.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;119;-609.8248,-76.31695;Half;False;Property;_moxing_col;moxing_col;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,0.4198113,0.4776148,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;167;-678.9675,90.72021;Inherit;True;Property;_mod_tex;mod_tex;0;0;Create;True;0;0;0;False;0;False;-1;None;57d592934155d8249a606ccfee262c1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;168;-555.4675,274.0204;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;175;-302.4753,-305.6534;Half;False;Property;_fre_col;fre_col;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,0.09019607,0.1740915,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexToFragmentNode;170;-123.6931,468.5835;Inherit;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;-364.3677,64.72029;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;21.43945,-43.70486;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;141.1973,367.7259;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;169;161.6982,63.85364;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;172;388.3535,270.1933;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;566.189,276.5918;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/moxing_fresnel;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;False;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;132;0;135;0
WireConnection;132;1;134;0
WireConnection;162;0;132;0
WireConnection;163;0;162;0
WireConnection;164;6;163;0
WireConnection;164;7;165;0
WireConnection;170;0;164;0
WireConnection;166;0;119;0
WireConnection;166;1;167;0
WireConnection;166;2;168;0
WireConnection;173;0;175;0
WireConnection;173;1;176;0
WireConnection;173;2;170;0
WireConnection;171;0;119;3
WireConnection;171;1;167;4
WireConnection;171;2;168;4
WireConnection;171;3;170;0
WireConnection;169;0;173;0
WireConnection;169;1;166;0
WireConnection;172;0;169;0
WireConnection;172;3;171;0
WireConnection;1;0;172;0
ASEEND*/
//CHKSM=80024B2405E1F6AAE8297D6E98CF4974F422EF9A