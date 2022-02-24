// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/raodongzhezhao"
{
	Properties
	{
		_noise2("noise01", 2D) = "white" {}
		_noise01UV_tilling1("noise01UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise01UV_speed1("noise01UV_speed", Vector) = (0.25,-0.1,0,0)
		_noise3("noise02", 2D) = "white" {}
		_noise02UV_tilling1("noise02UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise02UV_speed1("noise02UV_speed", Vector) = (-0.1,0.1,0,0)
		[HDR]_noise_shuchu_col1("noise_shuchu_col", Color) = (0.4056604,0.4056604,0.4056604,1)
		_noise_shuchu_int("noise_shuchu_int", Float) = 1
		_MainTex("MainTex", 2D) = "white" {}
		[HDR]_zhezhao_col1("zhezhao_col", Color) = (0.3370862,0.6460618,0.9528302,1)
		_zhezhao_int1("zhezhao_int", Float) = 0.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha One
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

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half _zhezhao_int1;
			uniform half4 _zhezhao_col1;
			uniform sampler2D _noise3;
			uniform half2 _noise02UV_speed1;
			uniform half2 _noise02UV_tilling1;
			uniform sampler2D _noise2;
			uniform half2 _noise01UV_speed1;
			uniform half2 _noise01UV_tilling1;
			uniform half4 _noise_shuchu_col1;
			uniform half _noise_shuchu_int;

			
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
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				half4 tex2DNode18 = tex2D( _MainTex, uv_MainTex );
				half2 texCoord14 = i.ase_texcoord1.xy * _noise02UV_tilling1 + float2( 0,0 );
				half2 panner15 = ( 1.0 * _Time.y * _noise02UV_speed1 + texCoord14);
				half2 texCoord13 = i.ase_texcoord1.xy * _noise01UV_tilling1 + float2( 0,0 );
				half2 panner16 = ( 1.0 * _Time.y * _noise01UV_speed1 + texCoord13);
				
				
				finalColor = ( ( ( tex2DNode18.r * _zhezhao_int1 * _zhezhao_col1 ) + ( ( tex2D( _noise3, panner15 ).r * tex2DNode18.r * tex2D( _noise2, panner16 ).r ) * _noise_shuchu_col1 * _noise_shuchu_int ) ) * tex2DNode18.r );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;236;1920;783;2550.071;584.6492;1.651457;True;False
Node;AmplifyShaderEditor.Vector2Node;10;-2104.183,-266.7806;Inherit;False;Property;_noise02UV_tilling1;noise02UV_tilling;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-2045.183,179.2194;Inherit;False;Property;_noise01UV_tilling1;noise01UV_tilling;1;0;Create;True;0;0;0;False;0;False;0.5,0.5;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-1896.183,-170.7806;Half;False;Property;_noise02UV_speed1;noise02UV_speed;5;0;Create;True;0;0;0;False;0;False;-0.1,0.1;-0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1848.183,181.2194;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1912.183,-282.7806;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;30;-1827.391,293.2645;Half;False;Property;_noise01UV_speed1;noise01UV_speed;2;0;Create;True;0;0;0;False;0;False;0.25,-0.1;0.25,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;15;-1688.183,-282.7806;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;16;-1608.183,181.2194;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1448.183,149.2194;Inherit;True;Property;_noise2;noise01;0;0;Create;True;0;0;0;False;0;False;-1;None;f26dd9d29b673dc4ba27d12e5ace0ebe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1480.183,-89.12912;Inherit;True;Property;_MainTex;MainTex;8;0;Create;True;0;0;0;False;0;False;-1;None;8083a08e443c66c46b626c9b8158f1f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;19;-1512.183,-314.7806;Inherit;True;Property;_noise3;noise02;3;0;Create;True;0;0;0;False;0;False;-1;None;a4382eba8239e1642915a57b3b12be98;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;-1144.183,149.2194;Half;False;Property;_noise_shuchu_col1;noise_shuchu_col;6;1;[HDR];Create;True;0;0;0;False;0;False;0.4056604,0.4056604,0.4056604,1;4.88689,1.207483,0.8529007,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1144.183,-106.7806;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-808.1831,-90.78058;Inherit;False;Property;_zhezhao_int1;zhezhao_int;10;0;Create;True;0;0;0;False;0;False;0.2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1106.645,330.4849;Half;False;Property;_noise_shuchu_int;noise_shuchu_int;7;0;Create;True;0;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-840.1831,-349.8275;Inherit;False;Property;_zhezhao_col1;zhezhao_col;9;1;[HDR];Create;True;0;0;0;False;0;False;0.3370862,0.6460618,0.9528302,1;0.05780543,0.04373504,0.02624122,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-600.1831,-186.7806;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-808.1831,53.21942;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-376.1637,38.68389;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-141.418,-52.24545;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;119.8555,-67.03908;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/raodongzhezhao;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;13;0;11;0
WireConnection;14;0;10;0
WireConnection;15;0;14;0
WireConnection;15;2;12;0
WireConnection;16;0;13;0
WireConnection;16;2;30;0
WireConnection;17;1;16;0
WireConnection;19;1;15;0
WireConnection;23;0;19;1
WireConnection;23;1;18;1
WireConnection;23;2;17;1
WireConnection;24;0;18;1
WireConnection;24;1;22;0
WireConnection;24;2;20;0
WireConnection;25;0;23;0
WireConnection;25;1;21;0
WireConnection;25;2;29;0
WireConnection;26;0;24;0
WireConnection;26;1;25;0
WireConnection;28;0;26;0
WireConnection;28;1;18;1
WireConnection;1;0;28;0
ASEEND*/
//CHKSM=962D1BD09E16ACB3139028FA14B3FF0823B016E0