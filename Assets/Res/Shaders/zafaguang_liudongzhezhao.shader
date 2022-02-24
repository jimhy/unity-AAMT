// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/zafaguang_liudongzhezhao"
{
	Properties
	{
		_zifaguang("zifaguang", 2D) = "white" {}
		[HDR]_zifaguang_col("zifaguang_col", Color) = (1,1,1,1)
		[Enum(shiyongRtongdao,0,alp_tongdao,1)]_panding("panding", Float) = 0
		_liudong_mask("liudong_mask", 2D) = "white" {}
		_scale_offset("scale_offset", Vector) = (1,1,0,0)
		[Enum(Particle,0,Material,1)]_liudong_panding("liudong_panding", Float) = 0
		[Toggle(_SHIFOUZILIUDONG_ON)] _shifouziliudong("shifouziliudong", Float) = 0
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
			#pragma shader_feature_local _SHIFOUZILIUDONG_ON


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

			uniform half4 _zifaguang_col;
			uniform sampler2D _zifaguang;
			uniform float4 _zifaguang_ST;
			uniform half _panding;
			uniform sampler2D _liudong_mask;
			uniform half4 _scale_offset;
			uniform half _liudong_panding;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1 = v.ase_texcoord;
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
				float2 uv_zifaguang = i.ase_texcoord1.xy * _zifaguang_ST.xy + _zifaguang_ST.zw;
				float4 tex2DNode2 = tex2D( _zifaguang, uv_zifaguang );
				float lerpResult15 = lerp( tex2DNode2.r , tex2DNode2.a , _panding);
				float2 appendResult11 = (float2(_scale_offset.x , _scale_offset.y));
				float4 texCoord8 = i.ase_texcoord1;
				texCoord8.xy = i.ase_texcoord1.xy * appendResult11 + float2( 0,0 );
				float2 appendResult25 = (float2(texCoord8.x , texCoord8.y));
				float2 appendResult19 = (float2(texCoord8.z , texCoord8.w));
				float2 appendResult12 = (float2(_scale_offset.z , _scale_offset.w));
				float2 lerpResult17 = lerp( appendResult19 , appendResult12 , _liudong_panding);
				#ifdef _SHIFOUZILIUDONG_ON
				float2 staticSwitch26 = ( lerpResult17 * _Time.y );
				#else
				float2 staticSwitch26 = lerpResult17;
				#endif
				float4 appendResult6 = (float4(( _zifaguang_col * tex2DNode2 ).rgb , ( lerpResult15 * tex2D( _liudong_mask, ( appendResult25 + staticSwitch26 ) ).r )));
				
				
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
7;54;1920;965;1745.926;179.2355;1;True;False
Node;AmplifyShaderEditor.Vector4Node;10;-1355.238,57.2589;Half;False;Property;_scale_offset;scale_offset;4;0;Create;True;0;0;0;False;0;False;1,1,0,0;1,1,5.91,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;11;-1189.303,61.74361;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1071.043,41.1974;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;12;-958.8531,270.8011;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1023.089,371.2399;Half;False;Property;_liudong_panding;liudong_panding;5;1;[Enum];Create;True;0;2;Particle;0;Material;1;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;-851.0887,157.2399;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;17;-817.8148,269.0128;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-848.0677,450.8059;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-649.3174,381.5334;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-732.478,84.21011;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;26;-653.478,201.2101;Inherit;False;Property;_shifouziliudong;shifouziliudong;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-400.2621,103.6923;Half;False;Property;_panding;panding;2;1;[Enum];Create;True;0;2;shiyongRtongdao;0;alp_tongdao;1;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-418.478,182.2101;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-603.1332,-89.09235;Inherit;True;Property;_zifaguang;zifaguang;0;0;Create;True;0;0;0;False;0;False;-1;None;adae8ec17a0f05f46915804145712dba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-527.1332,-255.0923;Half;False;Property;_zifaguang_col;zifaguang_col;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2,1.498039,0.6509804,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-377.9389,314.4187;Inherit;True;Property;_liudong_mask;liudong_mask;3;0;Create;True;0;0;0;False;0;False;-1;None;8332b3e6e33bc1344b17b1a83fd9e83f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-262.2221,9.297318;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-295.1332,-105.0923;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-85.657,53.85924;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;76.07001,-58.108;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;231,-58;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/zafaguang_liudongzhezhao;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;11;0;10;1
WireConnection;11;1;10;2
WireConnection;8;0;11;0
WireConnection;12;0;10;3
WireConnection;12;1;10;4
WireConnection;19;0;8;3
WireConnection;19;1;8;4
WireConnection;17;0;19;0
WireConnection;17;1;12;0
WireConnection;17;2;20;0
WireConnection;21;0;17;0
WireConnection;21;1;22;0
WireConnection;25;0;8;1
WireConnection;25;1;8;2
WireConnection;26;1;17;0
WireConnection;26;0;21;0
WireConnection;24;0;25;0
WireConnection;24;1;26;0
WireConnection;7;1;24;0
WireConnection;15;0;2;1
WireConnection;15;1;2;4
WireConnection;15;2;16;0
WireConnection;3;0;5;0
WireConnection;3;1;2;0
WireConnection;13;0;15;0
WireConnection;13;1;7;1
WireConnection;6;0;3;0
WireConnection;6;3;13;0
WireConnection;1;0;6;0
ASEEND*/
//CHKSM=F11FD0D3A563F649C4D26D96BF69FADDB79734E7