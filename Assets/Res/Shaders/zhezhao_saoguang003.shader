// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/saoguang_raodong003"
{
	Properties
	{
		_tex01("tex01", 2D) = "white" {}
		_tex01U_speed("tex01U_speed", Float) = -0.2
		_tex01v_speed("tex01v_speed", Float) = -0.35
		_tex02("tex02", 2D) = "white" {}
		_tex02U_speed("tex02U_speed", Float) = 0.1
		_tex02v_speed("tex02v_speed", Float) = 0.2
		_tex03("tex03", 2D) = "white" {}
		_tex03U_speed("tex03U_speed", Float) = 0.2
		_tex03v_speed("tex03v_speed", Float) = -0.2
		_tex04("tex04", 2D) = "white" {}
		_tex04v_speed("tex04v_speed", Float) = -0.24
		_tex04U_speed("tex04U_speed", Float) = 0.35
		[HDR]_wenli_col("wenli_col", Color) = (0,0,0,0)
		_wenli_int("wenli_int", Float) = 1
		_MainTex("MainTex", 2D) = "white" {}
		_zhezhao_int("zhezhao_int", Float) = 1
		[HDR]_zhezhao_col("zhezhao_col", Color) = (0,0,0,0)
		_U_tiling("U_tiling", Float) = 0
		_V_tiling("V_tiling", Float) = 0
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

			uniform half4 _zhezhao_col;
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half _zhezhao_int;
			uniform sampler2D _tex03;
			uniform half _tex03U_speed;
			uniform half _tex03v_speed;
			uniform half _U_tiling;
			uniform half _V_tiling;
			uniform sampler2D _tex01;
			uniform half _tex01U_speed;
			uniform half _tex01v_speed;
			uniform sampler2D _tex02;
			uniform half _tex02U_speed;
			uniform half _tex02v_speed;
			uniform sampler2D _tex04;
			uniform half _tex04U_speed;
			uniform half _tex04v_speed;
			uniform half4 _wenli_col;
			uniform half _wenli_int;

			
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
				half4 color3 = IsGammaSpace() ? half4(3.690377,1.014165,4.215746,1) : half4(17.68299,1.031428,23.69866,1);
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				half temp_output_7_0 = ( color3.a * tex2D( _MainTex, uv_MainTex ).a * _zhezhao_int );
				half2 appendResult35 = (half2(_tex03U_speed , _tex03v_speed));
				half2 appendResult53 = (half2(_U_tiling , _V_tiling));
				half2 texCoord10 = i.ase_texcoord1.xy * appendResult53 + float2( 0,0 );
				half2 appendResult12 = (half2(_tex01U_speed , _tex01v_speed));
				half2 panner11 = ( 1.0 * _Time.y * appendResult12 + texCoord10);
				half2 appendResult29 = (half2(_tex02U_speed , _tex02v_speed));
				half2 panner31 = ( 1.0 * _Time.y * appendResult29 + texCoord10);
				half2 panner24 = ( 1.0 * _Time.y * appendResult35 + ( texCoord10 + ( tex2D( _tex01, panner11 ).r * tex2D( _tex02, panner31 ).r ) ));
				half2 appendResult38 = (half2(_tex04U_speed , _tex04v_speed));
				half2 panner41 = ( 1.0 * _Time.y * appendResult38 + texCoord10);
				half4 appendResult6 = (half4(( ( _zhezhao_col * temp_output_7_0 ) + ( tex2D( _tex03, panner24 ).r * tex2D( _tex04, panner41 ).r * color3 * _wenli_col * _wenli_int ) ).rgb , temp_output_7_0));
				
				
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
7;277;1920;742;818.8757;56.83105;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;56;-1610.763,14.45041;Half;False;Property;_V_tiling;V_tiling;18;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1608.763,-52.54959;Half;False;Property;_U_tiling;U_tiling;17;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;53;-1443.763,-34.54959;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1353.517,83.70811;Half;False;Property;_tex01U_speed;tex01U_speed;1;0;Create;True;0;0;0;False;0;False;-0.2;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1359.158,317.8248;Half;False;Property;_tex02v_speed;tex02v_speed;5;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1358.517,151.7081;Half;False;Property;_tex01v_speed;tex01v_speed;2;0;Create;True;0;0;0;False;0;False;-0.35;-0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1363.158,232.8247;Half;False;Property;_tex02U_speed;tex02U_speed;4;0;Create;True;0;0;0;False;0;False;0.1;0.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-1194.66,110.4356;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1324.365,-58.4119;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;29;-1190.301,285.5523;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;11;-1060.175,3.745924;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;31;-1049.469,183.1119;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-878.1822,152.5677;Inherit;True;Property;_tex02;tex02;3;0;Create;True;0;0;0;False;0;False;-1;c3a8d1a5f5f0a02478046365010bca11;c3a8d1a5f5f0a02478046365010bca11;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-875.0853,-27.90955;Inherit;True;Property;_tex01;tex01;0;0;Create;True;0;0;0;False;0;False;-1;7dcf0cef871cea4488a22b7c698f5e79;7dcf0cef871cea4488a22b7c698f5e79;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-528,-80;Half;False;Property;_tex03v_speed;tex03v_speed;8;0;Create;True;0;0;0;False;0;False;-0.2;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-578.6274,29.33068;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-352.6997,165.6489;Half;False;Property;_tex04v_speed;tex04v_speed;10;0;Create;True;0;0;0;False;0;False;-0.24;-0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-353.6997,84.64886;Half;False;Property;_tex04U_speed;tex04U_speed;11;0;Create;True;0;0;0;False;0;False;0.35;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-528,-160;Half;False;Property;_tex03U_speed;tex03U_speed;7;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;35;-368,-112;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;38;-185.6997,112.6489;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-526.8871,-278.1514;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;24;-272,-272;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;310.4144,687.4734;Half;False;Property;_zhezhao_int;zhezhao_int;15;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;160.1147,507.0256;Inherit;True;Property;_MainTex;MainTex;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;190.3557,139.4146;Half;False;Constant;_tex01_col;tex01_col;1;1;[HDR];Create;True;0;0;0;False;0;False;3.690377,1.014165,4.215746,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;41;-82.69971,-39.35114;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;49;166.7332,-623.3743;Half;False;Property;_zhezhao_col;zhezhao_col;16;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.617682,2.506763,0.5102261,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;522.8138,248.9113;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;93.15633,-63.05698;Inherit;True;Property;_tex04;tex04;9;0;Create;True;0;0;0;False;0;False;-1;eb2478d449368824a91628d8d0f6fcc1;eb2478d449368824a91628d8d0f6fcc1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;51;162.3203,-437.9785;Half;False;Property;_wenli_col;wenli_col;12;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.7290888,1.674433,2.619777,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;108.855,-264.2151;Inherit;True;Property;_tex03;tex03;6;0;Create;True;0;0;0;False;0;False;-1;58809025f378e9348a5070ee2cd85b5e;58809025f378e9348a5070ee2cd85b5e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;216.2622,316.7991;Half;False;Property;_wenli_int;wenli_int;13;0;Create;True;0;0;0;False;0;False;1;16.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;589.9727,-188.95;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;568.6307,-76.4178;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;765.0051,-52.7359;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;1000.472,87.41473;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1395.581,56.13565;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/saoguang_raodong003;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;53;0;54;0
WireConnection;53;1;56;0
WireConnection;12;0;14;0
WireConnection;12;1;15;0
WireConnection;10;0;53;0
WireConnection;29;0;25;0
WireConnection;29;1;26;0
WireConnection;11;0;10;0
WireConnection;11;2;12;0
WireConnection;31;0;10;0
WireConnection;31;2;29;0
WireConnection;17;1;31;0
WireConnection;2;1;11;0
WireConnection;34;0;2;1
WireConnection;34;1;17;1
WireConnection;35;0;36;0
WireConnection;35;1;37;0
WireConnection;38;0;40;0
WireConnection;38;1;39;0
WireConnection;22;0;10;0
WireConnection;22;1;34;0
WireConnection;24;0;22;0
WireConnection;24;2;35;0
WireConnection;41;0;10;0
WireConnection;41;2;38;0
WireConnection;7;0;3;4
WireConnection;7;1;42;4
WireConnection;7;2;43;0
WireConnection;16;1;41;0
WireConnection;18;1;24;0
WireConnection;47;0;49;0
WireConnection;47;1;7;0
WireConnection;5;0;18;1
WireConnection;5;1;16;1
WireConnection;5;2;3;0
WireConnection;5;3;51;0
WireConnection;5;4;52;0
WireConnection;50;0;47;0
WireConnection;50;1;5;0
WireConnection;6;0;50;0
WireConnection;6;3;7;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=48C6EF63406F33AEA9540D9536AB131065278850