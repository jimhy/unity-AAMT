// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/liangbianrongjie"
{
	Properties
	{
		_rongjie_noise("rongjie_noise", 2D) = "white" {}
		_liangbian_kuan("liangbian_kuan", Float) = 0.02
		[HDR]_liangbian_col("liangbian_col", Color) = (1,1,1,1)
		_bian_col_int("bian_col_int", Float) = 1
		_yuan_tex("yuan_tex", 2D) = "white" {}
		_yuantex_int("yuantex_int", Float) = 1
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
		ZWrite On
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

			uniform sampler2D _yuan_tex;
			uniform half4 _yuan_tex_ST;
			uniform half _yuantex_int;
			uniform sampler2D _rongjie_noise;
			uniform half4 _rongjie_noise_ST;
			uniform half _liangbian_kuan;
			uniform half4 _liangbian_col;
			uniform half _bian_col_int;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
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
				float2 uv_yuan_tex = i.ase_texcoord1.xy * _yuan_tex_ST.xy + _yuan_tex_ST.zw;
				float2 uv_rongjie_noise = i.ase_texcoord1.xy * _rongjie_noise_ST.xy + _rongjie_noise_ST.zw;
				half4 tex2DNode7 = tex2D( _rongjie_noise, uv_rongjie_noise );
				half ifLocalVar4 = 0;
				if( tex2DNode7.r >= i.ase_color.a )
				ifLocalVar4 = 1.0;
				else
				ifLocalVar4 = 0.0;
				half ifLocalVar3 = 0;
				if( tex2DNode7.r >= ( i.ase_color.a + _liangbian_kuan ) )
				ifLocalVar3 = 1.0;
				else
				ifLocalVar3 = 0.0;
				half4 appendResult19 = (half4(( ( tex2D( _yuan_tex, uv_yuan_tex ) * _yuantex_int ) + ( ( ifLocalVar4 - ifLocalVar3 ) * _liangbian_col * _bian_col_int ) ).rgb , ifLocalVar4));
				
				
				finalColor = appendResult19;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
166;124;1509;871;1262.124;522.2491;1.336361;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-1373.52,59.81842;Half;False;Property;_liangbian_kuan;liangbian_kuan;1;0;Create;True;0;0;0;False;0;False;0.02;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;9;-1386.52,-204.1816;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-760.9291,264.574;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-1184.52,38.81842;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-746.8357,143.7296;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1068.915,81.09872;Inherit;True;Property;_rongjie_noise;rongjie_noise;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;3;-581.8357,192.7296;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;4;-582.8357,29.72955;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-431.4719,214.6175;Half;False;Property;_liangbian_col;liangbian_col;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0.9843137,0.8392157,0.282353,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;-403.8357,117.9461;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-374.4719,375.6175;Half;False;Property;_bian_col_int;bian_col_int;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-533.4109,-431.1453;Inherit;True;Property;_yuan_tex;yuan_tex;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-396.5378,-225.6708;Half;False;Property;_yuantex_int;yuantex_int;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-212.472,191.4176;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-188.5378,-375.6708;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-20.13731,-335.1396;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;240.257,-166.9557;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;421.8769,-164.5151;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/liangbianrongjie;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;10;0;9;4
WireConnection;10;1;11;0
WireConnection;3;0;7;1
WireConnection;3;1;10;0
WireConnection;3;2;5;0
WireConnection;3;3;5;0
WireConnection;3;4;6;0
WireConnection;4;0;7;1
WireConnection;4;1;9;4
WireConnection;4;2;5;0
WireConnection;4;3;5;0
WireConnection;4;4;6;0
WireConnection;8;0;4;0
WireConnection;8;1;3;0
WireConnection;12;0;8;0
WireConnection;12;1;13;0
WireConnection;12;2;14;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;18;0;16;0
WireConnection;18;1;12;0
WireConnection;19;0;18;0
WireConnection;19;3;4;0
WireConnection;1;0;19;0
ASEEND*/
//CHKSM=418A832F6C0AC3D81E9FDC3BE32C10E2EB44D01D