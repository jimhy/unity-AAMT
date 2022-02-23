// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/4_1tex"
{
	Properties
	{
		_main_tex("main_tex", 2D) = "white" {}
		[HDR]_maintex_col("maintex_col", Color) = (0,0,0,0)

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
		ZWrite On
		ZTest Always
		
		
		
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

			uniform half4 _maintex_col;
			uniform sampler2D _main_tex;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float2 texCoord10 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_11_0 = (texCoord10*2.0 + -1.0);
				float2 vertexToFrag29 = abs( temp_output_11_0 );
				o.ase_texcoord1.xy = vertexToFrag29;
				
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
				float2 vertexToFrag29 = i.ase_texcoord1.xy;
				
				
				finalColor = ( _maintex_col * tex2D( _main_tex, vertexToFrag29 ).r * i.ase_color );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;168;1682;851;2573.683;175.8813;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2086.278,136.2719;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;11;-1878.384,136.2788;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;28;-1706.07,86.2356;Inherit;False;303.9;261.1;绝对值，正数不变，负数的负号去掉输出;1;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.AbsOpNode;16;-1656.07,136.2356;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;19;-933.5992,-237.8918;Inherit;False;283;262;Comment;1;7;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexToFragmentNode;29;-1318.154,141.5649;Inherit;False;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;20;-945.7242,56.80344;Inherit;False;312;241;贴图是四分之一的圆，并且是clamp模式下;1;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;5;-932.7242,106.8035;Inherit;True;Property;_main_tex;main_tex;0;0;Create;True;0;0;0;False;0;False;-1;None;9ef065a8da3154342b422dc66e20d8ca;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;27;-1795.842,360.7046;Inherit;False;227.2001;539.8;从1到0到1;2;22;23;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;7;-883.5993,-187.892;Half;False;Property;_maintex_col;maintex_col;1;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;26;-2143.595,385.3453;Inherit;False;329.7;469.8;从-1到0到1;2;21;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;18;-811.4415,302.6993;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;24;-2066.595,658.3458;Inherit;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;23;-1770.343,677.2049;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1464.595,558.5463;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;22;-1745.842,410.7045;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-575.1826,119.1925;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;21;-2081.895,435.3453;Inherit;True;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;9;-275.073,118.9476;Float;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/4_1tex;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;7;False;-1;True;False;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;11;0;10;0
WireConnection;16;0;11;0
WireConnection;29;0;16;0
WireConnection;5;1;29;0
WireConnection;24;0;11;0
WireConnection;23;0;24;0
WireConnection;25;0;22;0
WireConnection;25;1;23;0
WireConnection;22;0;21;0
WireConnection;17;0;7;0
WireConnection;17;1;5;1
WireConnection;17;2;18;0
WireConnection;21;0;11;0
WireConnection;9;0;17;0
ASEEND*/
//CHKSM=65B8CB08FFD216716F0ABA2A9A39B8A8BDC17064