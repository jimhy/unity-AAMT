// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/saoguang03"
{
	Properties
	{
		_liudong("liudong", 2D) = "white" {}
		[HDR]_liudong_col("liudong_col", Color) = (1,1,1,1)
		_liudong_int("liudong_int", Float) = 1
		_MainTex01("MainTex01", 2D) = "white" {}
		_MainTex02("MainTex02", 2D) = "white" {}
		[Enum(huakuai,0,particle,1)]_UV_kaiguan1("UV_kaiguan", Float) = 0
		_liudong_UVtilling("liudong_UVtilling", Vector) = (1,1,0,0)
		_liudong_UVspeed("liudong_UVspeed", Vector) = (0,0,0,0)
		[Enum(Ufangxiang,0,Vfangxiang,1)]_LIZI_UVpanding("LIZI_UVpanding", Float) = 0
		_U_SPEED1("U_SPEED", Range( -1 , 1)) = 0
		_V_SPEED1("V_SPEED", Range( -1 , 1)) = 0
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

			uniform half4 _liudong_col;
			uniform sampler2D _liudong;
			uniform half2 _liudong_UVtilling;
			uniform float2 _liudong_UVspeed;
			uniform half _U_SPEED1;
			uniform half _V_SPEED1;
			uniform float _LIZI_UVpanding;
			uniform float _UV_kaiguan1;
			uniform half _liudong_int;
			uniform sampler2D _MainTex02;
			uniform float4 _MainTex02_ST;
			uniform sampler2D _MainTex01;
			uniform float4 _MainTex01_ST;

			
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
				float2 appendResult20 = (float2(( _liudong_UVspeed.x * _Time.y ) , ( _liudong_UVspeed.y * _Time.y )));
				float2 appendResult36 = (float2(( _U_SPEED1 + i.ase_color.a ) , 0.0));
				float2 appendResult35 = (float2(0.0 , ( _V_SPEED1 + i.ase_color.a )));
				float2 lerpResult25 = lerp( appendResult36 , appendResult35 , _LIZI_UVpanding);
				float2 lerpResult19 = lerp( appendResult20 , lerpResult25 , _UV_kaiguan1);
				float2 texCoord3 = i.ase_texcoord1.xy * _liudong_UVtilling + lerpResult19;
				float2 panner4 = ( 1.0 * _Time.y * float2( 0,0 ) + texCoord3);
				float2 uv_MainTex02 = i.ase_texcoord1.xy * _MainTex02_ST.xy + _MainTex02_ST.zw;
				float2 uv_MainTex01 = i.ase_texcoord1.xy * _MainTex01_ST.xy + _MainTex01_ST.zw;
				float4 appendResult13 = (float4(( _liudong_col * tex2D( _liudong, panner4 ).r * _liudong_int * i.ase_color ).rgb , ( tex2D( _MainTex02, uv_MainTex02 ).r * tex2D( _MainTex01, uv_MainTex01 ).r * i.ase_color.a )));
				
				
				finalColor = appendResult13;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;314;1920;705;2483.388;371.2426;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;15;-2690.179,194.7153;Half;False;Property;_V_SPEED1;V_SPEED;10;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;14;-2605.334,260.7362;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2689.983,125.0093;Half;False;Property;_U_SPEED1;U_SPEED;9;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;6;-2269.436,-74.42532;Inherit;False;Property;_liudong_UVspeed;liudong_UVspeed;7;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-2412.134,293.337;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-2407.874,157.8904;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;22;-2265.206,41.14522;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-2583,422.6705;Inherit;False;Property;_LIZI_UVpanding;LIZI_UVpanding;8;1;[Enum];Create;True;0;2;Ufangxiang;0;Vfangxiang;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2079.206,-46.85477;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;36;-2223.55,174.2018;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-2222.124,259.7904;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2078.206,41.14522;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-2062.392,217.9035;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;-1951.347,-6.562024;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1879.651,254.5866;Inherit;False;Property;_UV_kaiguan1;UV_kaiguan;5;1;[Enum];Create;True;0;2;huakuai;0;particle;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;19;-1815.048,-4.766907;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;5;-1806.704,-151.471;Half;False;Property;_liudong_UVtilling;liudong_UVtilling;6;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1577.465,-153.4934;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;4;-1361.989,-153.5213;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-1180.998,-183.0932;Inherit;True;Property;_liudong;liudong;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-1096.158,-346.461;Half;False;Property;_liudong_col;liudong_col;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1043.958,-1.360998;Half;False;Property;_liudong_int;liudong_int;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1340.207,8.921277;Inherit;True;Property;_MainTex02;MainTex02;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-1339.916,190.8986;Inherit;True;Property;_MainTex01;MainTex01;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1051.646,117.8394;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-872.1927,-175.1238;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-617.405,-169.1784;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-458.8999,-169.0001;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/saoguang03;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;28;0;15;0
WireConnection;28;1;14;4
WireConnection;27;0;16;0
WireConnection;27;1;14;4
WireConnection;21;0;6;1
WireConnection;21;1;22;2
WireConnection;36;0;27;0
WireConnection;35;1;28;0
WireConnection;23;0;6;2
WireConnection;23;1;22;2
WireConnection;25;0;36;0
WireConnection;25;1;35;0
WireConnection;25;2;26;0
WireConnection;20;0;21;0
WireConnection;20;1;23;0
WireConnection;19;0;20;0
WireConnection;19;1;25;0
WireConnection;19;2;17;0
WireConnection;3;0;5;0
WireConnection;3;1;19;0
WireConnection;4;0;3;0
WireConnection;2;1;4;0
WireConnection;10;0;12;1
WireConnection;10;1;11;1
WireConnection;10;2;14;4
WireConnection;7;0;8;0
WireConnection;7;1;2;1
WireConnection;7;2;9;0
WireConnection;7;3;14;0
WireConnection;13;0;7;0
WireConnection;13;3;10;0
WireConnection;1;0;13;0
ASEEND*/
//CHKSM=A5D9CB6D373A5E1D63F3E9648EFE08B2F2E8D12F