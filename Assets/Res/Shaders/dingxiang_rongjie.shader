// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/dingxingrongjie"
{
	Properties
	{
		_tex01("tex01", 2D) = "white" {}
		[HDR]_rongjie_col("rongjie_col", Color) = (1,1,1,1)
		_rongjie("rongjie", Range( -0.5 , 1)) = -0.5
		_noise_tex("noise_tex", 2D) = "white" {}
		_UVpianyi("UVpianyi", Vector) = (0,-0.6,0,0)
		_UV_tilling("UV_tilling", Vector) = (3,3,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_noise_int("noise_int", Float) = 0.2
		_tex01_int("tex01_int", Float) = 1
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
			#include "UnityShaderVariables.cginc"


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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half4 _rongjie_col;
			uniform sampler2D _tex01;
			uniform half4 _tex01_ST;
			uniform half _tex01_int;
			uniform half _noise_int;
			uniform sampler2D _noise_tex;
			uniform half2 _UV_tilling;
			uniform half2 _UVpianyi;
			uniform sampler2D _MainTex;
			uniform half _rongjie;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
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
				float2 uv_tex01 = i.ase_texcoord1.xy * _tex01_ST.xy + _tex01_ST.zw;
				half2 texCoord32 = i.ase_texcoord1.xy * _UV_tilling + ( _UVpianyi * _Time.y );
				half2 panner33 = ( 1.0 * _Time.y * float2( 0,0 ) + texCoord32);
				half2 temp_cast_0 = (_rongjie).xx;
				half2 texCoord38 = i.ase_texcoord1.xy * float2( 1,1 ) + temp_cast_0;
				half2 panner37 = ( 1.0 * _Time.y * float2( 0,0 ) + texCoord38);
				
				
				finalColor = ( ( ( _rongjie_col * tex2D( _tex01, uv_tex01 ) * _tex01_int ) + ( _noise_int * tex2D( _noise_tex, panner33 ).r ) ) * tex2D( _MainTex, panner37 ).r );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;194;1920;825;2547.231;259.034;1.3;True;False
Node;AmplifyShaderEditor.TimeNode;36;-2325.783,268.8565;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;34;-2288.484,143.9565;Half;False;Property;_UVpianyi;UVpianyi;4;0;Create;True;0;0;0;False;0;False;0,-0.6;0,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;50;-2226.555,13.06027;Half;False;Property;_UV_tilling;UV_tilling;5;0;Create;True;0;0;0;False;0;False;3,3;0,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-2140.783,167.8565;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;-2000.217,84.73844;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-2203.188,449.1585;Half;False;Property;_rongjie;rongjie;2;0;Create;True;0;0;0;False;0;False;-0.5;0.3127933;-0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;33;-1790.783,86.85654;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-2009.274,281.9624;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1253.547,-186.6817;Inherit;True;Property;_tex01;tex01;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-1506.753,-7.465384;Half;False;Property;_noise_int;noise_int;7;0;Create;True;0;0;0;False;0;False;0.2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-1176.102,-373.9359;Half;False;Property;_rongjie_col;rongjie_col;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,0.8293912,0.2216981,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-1609.071,64.57623;Inherit;True;Property;_noise_tex;noise_tex;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;51;-1153.346,-1.409965;Half;False;Property;_tex01_int;tex01_int;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-938.2535,-205.7884;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1271.307,104.4198;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-1798.634,280.4619;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-713.2676,-103.8276;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;39;-1618.21,253.2882;Inherit;True;Property;_MainTex;MainTex;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-678.8856,195.909;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-417.4589,261.9348;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/dingxingrongjie;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;35;0;34;0
WireConnection;35;1;36;2
WireConnection;32;0;50;0
WireConnection;32;1;35;0
WireConnection;33;0;32;0
WireConnection;38;1;18;0
WireConnection;19;1;33;0
WireConnection;29;0;3;0
WireConnection;29;1;2;0
WireConnection;29;2;51;0
WireConnection;48;0;43;0
WireConnection;48;1;19;1
WireConnection;37;0;38;0
WireConnection;49;0;29;0
WireConnection;49;1;48;0
WireConnection;39;1;37;0
WireConnection;47;0;49;0
WireConnection;47;1;39;1
WireConnection;1;0;47;0
ASEEND*/
//CHKSM=D594E1BA3598865B08C05AB118050BBBF90A41FA