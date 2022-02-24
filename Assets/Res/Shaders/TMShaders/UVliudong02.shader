// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/UVliudong02"
{
	Properties
	{
		[Enum(particle,0,material,1)]_liudong_fangshi("liudong_fangshi", Float) = 0
		_U_apeed("U_apeed", Float) = 0
		_V_speed("V_speed", Float) = 0
		_liudongxiaosan_weizhi("liudongxiaosan_weizhi", Range( -1 , 1)) = -1
		_tex01("tex01", 2D) = "white" {}
		[HDR]_tex01_col("tex01_col", Color) = (1,1,1,1)
		_tex01_int("tex01_int", Float) = 1
		_zhezhao("zhezhao", 2D) = "white" {}
		_zhezhao_int("zhezhao_int", Float) = 1
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
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half4 _tex01_col;
			uniform sampler2D _tex01;
			uniform half _liudongxiaosan_weizhi;
			uniform half _U_apeed;
			uniform half _V_speed;
			uniform half _liudong_fangshi;
			uniform half _tex01_int;
			uniform sampler2D _zhezhao;
			uniform half4 _zhezhao_ST;
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
				half2 appendResult35 = (half2(( i.ase_color.a + _liudongxiaosan_weizhi ) , 0.0));
				half2 appendResult16 = (half2(_U_apeed , _V_speed));
				half2 lerpResult22 = lerp( appendResult35 , ( appendResult16 * _Time.y ) , _liudong_fangshi);
				half2 texCoord10 = i.ase_texcoord1.xy * float2( 1,1 ) + lerpResult22;
				half2 panner11 = ( 1.0 * _Time.y * float2( 0,0 ) + texCoord10);
				half4 tex2DNode2 = tex2D( _tex01, panner11 );
				float2 uv_zhezhao = i.ase_texcoord1.xy * _zhezhao_ST.xy + _zhezhao_ST.zw;
				half4 appendResult6 = (half4(( _tex01_col * tex2DNode2.r * _tex01_int ).rgb , ( _tex01_col.a * tex2DNode2.a * ( tex2D( _zhezhao, uv_zhezhao ).r * _zhezhao_int ) )));
				
				
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
7;378;1920;641;2751.277;259.4916;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-1714.919,241.4825;Half;False;Property;_V_speed;V_speed;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;24;-2227.165,-217.6781;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-2314.349,-12.57501;Half;False;Property;_liudongxiaosan_weizhi;liudongxiaosan_weizhi;3;0;Create;True;0;0;0;False;0;False;-1;-0.31;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1714.919,179.4823;Half;False;Property;_U_apeed;U_apeed;1;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1569.919,211.4824;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1953.04,-92.19588;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;27;-1642.51,334.4684;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-1435.26,655.5939;Half;False;Property;_liudong_fangshi;liudong_fangshi;0;1;[Enum];Create;True;0;2;particle;0;material;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1399.858,228.315;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-1466.277,-42.49158;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;22;-1228.972,213.5822;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1147.069,2.446442;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-410.639,618.8316;Half;False;Property;_zhezhao_int;zhezhao_int;8;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-897.1462,10.44644;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;12;-540.639,426.8316;Inherit;True;Property;_zhezhao;zhezhao;7;0;Create;True;0;0;0;False;0;False;-1;None;115e0a1b8c2d002468cf2dd23764fb54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-630.2305,-24.50464;Inherit;True;Property;_tex01;tex01;4;0;Create;True;0;0;0;False;0;False;-1;None;ad11e65dbac278a48ada2f1b82946cbf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-243.639,546.8316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-561.2304,-216.3046;Half;False;Property;_tex01_col;tex01_col;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0.282353,0.3647059,0.7490196,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-530.6308,180.2953;Half;False;Property;_tex01_int;tex01_int;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-260.2305,-31.50464;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-239.2305,244.4954;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-87.23047,-26.50464;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;162.7554,-27;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/UVliudong02;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;31;0;24;4
WireConnection;31;1;30;0
WireConnection;25;0;16;0
WireConnection;25;1;27;2
WireConnection;35;0;31;0
WireConnection;22;0;35;0
WireConnection;22;1;25;0
WireConnection;22;2;23;0
WireConnection;10;1;22;0
WireConnection;11;0;10;0
WireConnection;2;1;11;0
WireConnection;14;0;12;1
WireConnection;14;1;13;0
WireConnection;5;0;3;0
WireConnection;5;1;2;1
WireConnection;5;2;4;0
WireConnection;7;0;3;4
WireConnection;7;1;2;4
WireConnection;7;2;14;0
WireConnection;6;0;5;0
WireConnection;6;3;7;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=8079ED259331F4B979E5FDF6C3AAD08F77F50176