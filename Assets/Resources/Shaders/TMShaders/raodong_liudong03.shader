// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/raodong_liudong03"
{
	Properties
	{
		_noise01("noise01", 2D) = "white" {}
		_noise01UV_tilling("noise01UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise01UV_speed("noise01UV_speed", Vector) = (0.25,-0.1,0,0)
		_noise02("noise02", 2D) = "white" {}
		_noise02UV_tilling("noise02UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise02UV_speed("noise02UV_speed", Vector) = (-0.1,0.1,0,0)
		[HDR]_noise_shuchu_col("noise_shuchu_col", Color) = (0.4056604,0.4056604,0.4056604,1)
		_noise_shuchu_int("noise_shuchu_int", Float) = 1
		_zhezhao("zhezhao", 2D) = "white" {}
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

			uniform sampler2D _noise02;
			uniform half2 _noise02UV_speed;
			uniform half2 _noise02UV_tilling;
			uniform sampler2D _noise01;
			uniform half2 _noise01UV_speed;
			uniform half2 _noise01UV_tilling;
			uniform half4 _noise_shuchu_col;
			uniform half _noise_shuchu_int;
			uniform sampler2D _zhezhao;
			uniform half4 _zhezhao_ST;

			
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
				half2 texCoord14 = i.ase_texcoord1.xy * _noise02UV_tilling + float2( 0,0 );
				half2 panner16 = ( 1.0 * _Time.y * _noise02UV_speed + texCoord14);
				half2 texCoord13 = i.ase_texcoord1.xy * _noise01UV_tilling + float2( 0,0 );
				half2 panner17 = ( 1.0 * _Time.y * _noise01UV_speed + texCoord13);
				float2 uv_zhezhao = i.ase_texcoord1.xy * _zhezhao_ST.xy + _zhezhao_ST.zw;
				half4 appendResult28 = (half4(( tex2D( _noise02, panner16 ).r * tex2D( _noise01, panner17 ).r * _noise_shuchu_col * _noise_shuchu_int ).rgb , tex2D( _zhezhao, uv_zhezhao ).r));
				
				
				finalColor = appendResult28;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
-163;252;1599;629;3142.498;227.6481;1.732358;True;False
Node;AmplifyShaderEditor.Vector2Node;10;-2414.95,-68.23244;Inherit;False;Property;_noise02UV_tilling;noise02UV_tilling;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-2403.801,174.3583;Inherit;False;Property;_noise01UV_tilling;noise01UV_tilling;1;0;Create;True;0;0;0;False;0;False;0.5,0.5;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2222.95,-84.23242;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;12;-2206.95,27.76759;Half;False;Property;_noise02UV_speed;noise02UV_speed;5;0;Create;True;0;0;0;False;0;False;-0.1,0.1;-0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-2211.801,174.3583;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;15;-2195.801,286.3582;Half;False;Property;_noise01UV_speed;noise01UV_speed;2;0;Create;True;0;0;0;False;0;False;0.25,-0.1;0.5,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;16;-1998.951,-84.23242;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;17;-1971.803,174.3583;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;20;-1822.951,-116.2324;Inherit;True;Property;_noise02;noise02;3;0;Create;True;0;0;0;False;0;False;-1;9a5e528b267ca6e4f8142b0e51a681b9;9a5e528b267ca6e4f8142b0e51a681b9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1811.803,142.3583;Inherit;True;Property;_noise01;noise01;0;0;Create;True;0;0;0;False;0;False;-1;9a5e528b267ca6e4f8142b0e51a681b9;eb2478d449368824a91628d8d0f6fcc1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-1737.136,345.0085;Half;False;Property;_noise_shuchu_col;noise_shuchu_col;6;1;[HDR];Create;True;0;0;0;False;0;False;0.4056604,0.4056604,0.4056604,1;0.145098,0.3921569,0.7490196,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1713.302,516.3262;Half;False;Property;_noise_shuchu_int;noise_shuchu_int;7;0;Create;True;0;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1460.057,0.3335917;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-1405.107,276.5679;Inherit;True;Property;_zhezhao;zhezhao;8;0;Create;True;0;0;0;False;0;False;-1;7071e07fa23648d419aea21e368c17ec;e7b9d7b13b8b97047a68d892bc3d5332;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;28;-946.8619,3.489485;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-799.602,-2.55593;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/raodong_liudong03;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;14;0;10;0
WireConnection;13;0;11;0
WireConnection;16;0;14;0
WireConnection;16;2;12;0
WireConnection;17;0;13;0
WireConnection;17;2;15;0
WireConnection;20;1;16;0
WireConnection;18;1;17;0
WireConnection;23;0;20;1
WireConnection;23;1;18;1
WireConnection;23;2;22;0
WireConnection;23;3;25;0
WireConnection;28;0;23;0
WireConnection;28;3;19;1
WireConnection;1;0;28;0
ASEEND*/
//CHKSM=D225B47838257600AF26AD47286F49EBB3A025B0