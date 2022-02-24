// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/liudong_zhezhaoUValp"
{
	Properties
	{
		_NOISE01("NOISE01", 2D) = "white" {}
		_noise01uv_speed("noise01uv_speed", Vector) = (0,0,0,0)
		_NOISE02("NOISE02", 2D) = "white" {}
		_noise02uv_speed("noise02uv_speed", Vector) = (0,0,0,0)
		[HDR]_NOISE1_2COL("NOISE1_2COL", Color) = (1,0.9119223,0.2971698,1)
		_NOISE1_2INT("NOISE1_2INT", Float) = 0
		_NOISE03("NOISE03", 2D) = "white" {}
		_noise03uv_speed("noise03uv_speed", Vector) = (0,0,0,0)
		[HDR]_NOISE3COL("NOISE3COL", Color) = (0.8018868,0.3386145,0.2534265,1)
		_NOISE03INT("NOISE03INT", Float) = 1
		_liudong_zhezhao01("liudong_zhezhao01", 2D) = "white" {}
		_liudongzhezhao01uv_speed("liudongzhezhao01uv_speed", Vector) = (0,0,0,0)
		_liudong_zhezhao02("liudong_zhezhao02", 2D) = "white" {}
		_liudongzhezhao02uv_speed("liudongzhezhao02uv_speed", Vector) = (0,0,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		_zhezhao_int("zhezhao_int", Float) = 0
		[Toggle(_SHIFOU_ZHEZHAOZIFAGUANG_ON)] _shifou_zhezhaozifaguang("shifou_zhezhaozifaguang", Float) = 0
		_zifaguang("zifaguang", 2D) = "white" {}
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
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _SHIFOU_ZHEZHAOZIFAGUANG_ON


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

			uniform sampler2D _NOISE01;
			uniform half2 _noise01uv_speed;
			uniform sampler2D _NOISE02;
			uniform half2 _noise02uv_speed;
			uniform half4 _NOISE1_2COL;
			uniform half _NOISE1_2INT;
			uniform sampler2D _NOISE03;
			uniform half2 _noise03uv_speed;
			uniform half4 _NOISE3COL;
			uniform half _NOISE03INT;
			uniform sampler2D _zifaguang;
			uniform half4 _zifaguang_ST;
			uniform sampler2D _liudong_zhezhao01;
			uniform half2 _liudongzhezhao01uv_speed;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D _liudong_zhezhao02;
			uniform half2 _liudongzhezhao02uv_speed;
			uniform half _zhezhao_int;

			
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
				half2 texCoord10 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				half2 panner11 = ( 1.0 * _Time.y * _noise01uv_speed + texCoord10);
				half2 panner12 = ( 1.0 * _Time.y * _noise02uv_speed + texCoord10);
				half2 panner13 = ( 1.0 * _Time.y * _noise03uv_speed + texCoord10);
				half4 temp_output_26_0 = ( ( tex2D( _NOISE01, panner11 ).r * tex2D( _NOISE02, panner12 ).r * _NOISE1_2COL * _NOISE1_2INT ) + ( tex2D( _NOISE03, panner13 ).r * _NOISE3COL * _NOISE03INT ) );
				float2 uv_zifaguang = i.ase_texcoord1.xy * _zifaguang_ST.xy + _zifaguang_ST.zw;
				#ifdef _SHIFOU_ZHEZHAOZIFAGUANG_ON
				half4 staticSwitch50 = ( temp_output_26_0 + tex2D( _zifaguang, uv_zifaguang ) );
				#else
				half4 staticSwitch50 = temp_output_26_0;
				#endif
				half2 texCoord27 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				half2 panner15 = ( 1.0 * _Time.y * _liudongzhezhao01uv_speed + texCoord27);
				half4 tex2DNode34 = tex2D( _liudong_zhezhao01, panner15 );
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				half2 panner14 = ( 1.0 * _Time.y * _liudongzhezhao02uv_speed + texCoord27);
				half4 appendResult6 = (half4(( staticSwitch50 * i.ase_color ).rgb , ( ( tex2DNode34.r + tex2DNode34.g ) * tex2D( _MainTex, uv_MainTex ).r * tex2D( _liudong_zhezhao02, panner14 ).r * _zhezhao_int * i.ase_color.a )));
				
				
				finalColor = appendResult6;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;212;1920;807;2260.573;-971.285;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2114.771,180.2537;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;30;-2049.946,730.4375;Inherit;False;Property;_noise03uv_speed;noise03uv_speed;7;0;Create;True;0;0;0;False;0;False;0,0;0,-0.12;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;28;-2094.946,58.43747;Inherit;False;Property;_noise01uv_speed;noise01uv_speed;1;0;Create;True;0;0;0;False;0;False;0,0;0,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;29;-2093.946,293.4375;Inherit;False;Property;_noise02uv_speed;noise02uv_speed;3;0;Create;True;0;0;0;False;0;False;0,0;0,-0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;13;-1846.769,710.9539;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;11;-1866.47,110.0537;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;12;-1863.871,254.3539;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-1676.102,258.1031;Inherit;True;Property;_NOISE02;NOISE02;2;0;Create;True;0;0;0;False;0;False;-1;None;a4382eba8239e1642915a57b3b12be98;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-1545.102,611.1031;Half;False;Property;_NOISE1_2INT;NOISE1_2INT;5;0;Create;True;0;0;0;False;0;False;0;2.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-1589.214,879.9991;Half;False;Property;_NOISE3COL;NOISE3COL;8;1;[HDR];Create;True;0;0;0;False;0;False;0.8018868,0.3386145,0.2534265,1;0.1219063,0.3018868,0.05838376,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-1598.102,446.1031;Half;False;Property;_NOISE1_2COL;NOISE1_2COL;4;1;[HDR];Create;True;0;0;0;False;0;False;1,0.9119223,0.2971698,1;1.684618,4.868123,1.033328,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-1670.989,687.1653;Inherit;True;Property;_NOISE03;NOISE03;6;0;Create;True;0;0;0;False;0;False;-1;None;8635b715adca8d845b42382f07e65a91;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1535.811,1046.941;Half;False;Property;_NOISE03INT;NOISE03INT;9;0;Create;True;0;0;0;False;0;False;1;4.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-1675.102,71.10315;Inherit;True;Property;_NOISE01;NOISE01;0;0;Create;True;0;0;0;False;0;False;-1;None;07cd3df624a560e4881fb9174bdb489f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-2040.776,1410.684;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;31;-2089.163,1287.499;Inherit;False;Property;_liudongzhezhao01uv_speed;liudongzhezhao01uv_speed;11;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1362.889,717.0649;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1376.102,185.1031;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1273.501,463.3282;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;51;-1243.028,825.3612;Inherit;True;Property;_zifaguang;zifaguang;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;32;-2086.163,1525.499;Inherit;False;Property;_liudongzhezhao02uv_speed;liudongzhezhao02uv_speed;13;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;15;-1797.529,1278.329;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;14;-1790.12,1518.114;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-1607.163,1179.499;Inherit;True;Property;_liudong_zhezhao01;liudong_zhezhao01;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-952.0254,676.717;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;44;-1165.499,1195.101;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;50;-771.4822,433.2383;Inherit;False;Property;_shifou_zhezhaozifaguang;shifou_zhezhaozifaguang;16;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1276.108,1623.556;Half;False;Property;_zhezhao_int;zhezhao_int;15;0;Create;True;0;0;0;False;0;False;0;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-1604.163,1549.499;Inherit;True;Property;_liudong_zhezhao02;liudong_zhezhao02;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-1306.776,1207.354;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-1607.163,1364.499;Inherit;True;Property;_MainTex;MainTex;14;0;Create;True;0;0;0;False;0;False;-1;None;ff9c99880acd95c41a3afdbca37c6d91;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1096.109,1402.556;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-617.1001,815.3018;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-547.5777,992.4052;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;-405.3469,993.9099;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/liudong_zhezhaoUValp;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;13;0;10;0
WireConnection;13;2;30;0
WireConnection;11;0;10;0
WireConnection;11;2;28;0
WireConnection;12;0;10;0
WireConnection;12;2;29;0
WireConnection;18;1;12;0
WireConnection;22;1;13;0
WireConnection;16;1;11;0
WireConnection;23;0;22;1
WireConnection;23;1;24;0
WireConnection;23;2;25;0
WireConnection;19;0;16;1
WireConnection;19;1;18;1
WireConnection;19;2;20;0
WireConnection;19;3;21;0
WireConnection;26;0;19;0
WireConnection;26;1;23;0
WireConnection;15;0;27;0
WireConnection;15;2;31;0
WireConnection;14;0;27;0
WireConnection;14;2;32;0
WireConnection;34;1;15;0
WireConnection;47;0;26;0
WireConnection;47;1;51;0
WireConnection;50;1;26;0
WireConnection;50;0;47;0
WireConnection;35;1;14;0
WireConnection;36;0;34;1
WireConnection;36;1;34;2
WireConnection;37;0;36;0
WireConnection;37;1;33;1
WireConnection;37;2;35;1
WireConnection;37;3;38;0
WireConnection;37;4;44;4
WireConnection;46;0;50;0
WireConnection;46;1;44;0
WireConnection;6;0;46;0
WireConnection;6;3;37;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=B749864D73B0AC0845DCC36B50053EB3D523C6C1