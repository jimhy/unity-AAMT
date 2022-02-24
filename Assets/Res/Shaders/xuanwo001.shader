// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/xuanwo"
{
	Properties
	{
		_tex01("tex01", 2D) = "white" {}
		_tex01_int("tex01_int", Float) = 0
		[HDR]_tex01_col("tex01_col", Color) = (1,1,1,1)
		_UVscale("UVscale", Vector) = (1,1,0,0)
		_UV_offset("UV_offset", Vector) = (0.1,0,0,0)
		_Power_xiangweijiya("Power_xiangweijiya", Float) = 1
		_dingweixuanwo_zhongxindian("dingweixuanwo_zhongxindian", Vector) = (0.5,0.5,0,0)
		_xuanwo_midu("xuanwo_midu", Float) = 1
		_MainTex("MainTex", 2D) = "white" {}
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

			uniform half4 _tex01_col;
			uniform sampler2D _tex01;
			uniform half2 _dingweixuanwo_zhongxindian;
			uniform half _xuanwo_midu;
			uniform half _Power_xiangweijiya;
			uniform half2 _UVscale;
			uniform half2 _UV_offset;
			uniform half _tex01_int;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;

			
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
				float cos40 = cos( ( ( ( 1.0 - length( (texCoord10*2.0 + -1.0) ) ) * 2.0 * _xuanwo_midu ) * UNITY_PI ) );
				float sin40 = sin( ( ( ( 1.0 - length( (texCoord10*2.0 + -1.0) ) ) * 2.0 * _xuanwo_midu ) * UNITY_PI ) );
				half2 rotator40 = mul( texCoord10 - _dingweixuanwo_zhongxindian , float2x2( cos40 , -sin40 , sin40 , cos40 )) + _dingweixuanwo_zhongxindian;
				half2 temp_output_44_0 = (rotator40*2.0 + -1.0);
				half2 break15 = (temp_output_44_0*2.0 + -1.0);
				half2 appendResult28 = (half2(pow( length( temp_output_44_0 ) , _Power_xiangweijiya ) , ( ( atan2( break15.y , break15.x ) / ( 2.0 * UNITY_PI ) ) + 0.5 )));
				half2 appendResult32 = (half2(( _UV_offset.x * _Time.y ) , ( _UV_offset.y * _Time.y )));
				half4 tex2DNode2 = tex2D( _tex01, (appendResult28*_UVscale + appendResult32) );
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				half4 appendResult6 = (half4(( _tex01_col * tex2DNode2.r * _tex01_int * i.ase_color ).rgb , ( _tex01_col.a * tex2DNode2.b * i.ase_color.a * tex2D( _MainTex, uv_MainTex ).r * tex2DNode2.g )));
				
				
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
7;265;1920;754;1291.277;-133.2812;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-3120.37,28.34384;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;45;-3043.336,366.9268;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;46;-2794.514,337.2701;Inherit;False;166.2416;244.7018;输出一个长度;1;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;50;-2601.851,327.8077;Inherit;False;173.9452;259.7729;1-,（黑的白、白的黑）;1;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LengthOpNode;47;-2783.665,375.1825;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2583.618,595.129;Half;False;Property;_xuanwo_midu;xuanwo_midu;7;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;-2592.992,377.8076;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-2398.426,380.2653;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;54;-2263.286,381.9358;Inherit;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;43;-2639.306,101.7666;Inherit;False;Property;_dingweixuanwo_zhongxindian;dingweixuanwo_zhongxindian;6;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;40;-2265.877,36.05824;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;21;-1965.141,374.9773;Inherit;False;238;251;先乘后加，也是重复偏移;1;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;44;-2012.744,40.65134;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;29;-1719.029,319.8564;Inherit;False;885.4617;743.2486;此处主要是解决重铺的问题;6;26;15;17;24;25;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;13;-1954.141,410.9772;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;15;-1703.029,411.856;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.CommentaryNode;17;-1558.029,369.8562;Inherit;False;200;242;交叉链接;1;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;25;-1568.029,636.8545;Inherit;False;191;121;相当于Π乘一个数再输出;1;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;-1658.035,-9.160271;Inherit;False;166.2416;244.7018;输出一个长度;1;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-1338.029,388.8562;Inherit;False;204;246;相除;1;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ATan2OpNode;16;-1552.029,404.8559;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;22;-1561.029,686.8544;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;34;-770.4705,371.4597;Half;False;Property;_UV_offset;UV_offset;4;0;Create;True;0;0;0;False;0;False;0.1,0;0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;35;-814.5279,498.05;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-1652.988,243.3904;Half;False;Property;_Power_xiangweijiya;Power_xiangweijiya;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;11;-1647.186,28.75222;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;23;-1328.029,423.856;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-610.5279,499.05;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-1107.029,424.856;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;38;-1433.988,29.39038;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-609.5279,396.05;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;33;-765.4705,240.4595;Half;False;Property;_UVscale;UVscale;3;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;32;-473.0685,441.1267;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-880.7495,31.48492;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;30;-647.22,26.66699;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-466.2333,-0.8285446;Inherit;True;Property;_tex01;tex01;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;9;-337.8647,187.46;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-374.1647,-174.5399;Half;False;Property;_tex01_col;tex01_col;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-161.2648,79.4601;Half;False;Property;_tex01_int;tex01_int;1;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;-393.3295,505.9464;Inherit;True;Property;_MainTex;MainTex;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-132.6649,215.9599;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-10.0649,17.05999;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;201.8353,5.559951;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;342.8655,6.66459;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/xuanwo;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
Node;AmplifyShaderEditor.CommentaryNode;27;-1335.029,641.8545;Inherit;False;425.457;393.8713;下边之所以是黑的，是因为此时它的输出是0.5——-0.5之间;0;;1,1,1,1;0;0
WireConnection;45;0;10;0
WireConnection;47;0;45;0
WireConnection;48;0;47;0
WireConnection;51;0;48;0
WireConnection;51;2;53;0
WireConnection;54;0;51;0
WireConnection;40;0;10;0
WireConnection;40;1;43;0
WireConnection;40;2;54;0
WireConnection;44;0;40;0
WireConnection;13;0;44;0
WireConnection;15;0;13;0
WireConnection;16;0;15;1
WireConnection;16;1;15;0
WireConnection;11;0;44;0
WireConnection;23;0;16;0
WireConnection;23;1;22;0
WireConnection;37;0;34;2
WireConnection;37;1;35;2
WireConnection;26;0;23;0
WireConnection;38;0;11;0
WireConnection;38;1;39;0
WireConnection;36;0;34;1
WireConnection;36;1;35;2
WireConnection;32;0;36;0
WireConnection;32;1;37;0
WireConnection;28;0;38;0
WireConnection;28;1;26;0
WireConnection;30;0;28;0
WireConnection;30;1;33;0
WireConnection;30;2;32;0
WireConnection;2;1;30;0
WireConnection;7;0;3;4
WireConnection;7;1;2;3
WireConnection;7;2;9;4
WireConnection;7;3;55;1
WireConnection;7;4;2;2
WireConnection;5;0;3;0
WireConnection;5;1;2;1
WireConnection;5;2;4;0
WireConnection;5;3;9;0
WireConnection;6;0;5;0
WireConnection;6;3;7;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=FCA3E67363E05077AF9C03D57DDEDB8369CBF2DE