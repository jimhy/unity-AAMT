// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/raodongrongjie"
{
	Properties
	{
		_fre("fre", Vector) = (1,5,0,0)
		[HDR]_fre_col("fre_col", Color) = (1,1,1,1)
		_noise01("noise01", 2D) = "white" {}
		_noise01UV_tilling("noise01UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise01UV_speed("noise01UV_speed", Vector) = (0.25,-0.1,0,0)
		_noise02("noise02", 2D) = "white" {}
		_noise02UV_tilling("noise02UV_tilling", Vector) = (0.5,0.5,0,0)
		_noise02UV_speed("noise02UV_speed", Vector) = (-0.1,0.1,0,0)
		_MainTex("MainTex", 2D) = "white" {}
		[Enum(RGBzifaguang,0,dantongdao,1)]_zifaguang_dantongdao_panding("zifaguang_dantongdao_panding", Float) = 0
		_zhezhao_int("zhezhao_int", Float) = 0.2
		[HDR]_zhezhao_col("zhezhao_col", Color) = (1,1,1,1)
		_noise_shuchu_col("noise_shuchu_col", Color) = (0.4056604,0.4056604,0.4056604,1)
		_noise_shuchu_int("noise_shuchu_int", Float) = 1.5
		_rongjie_noise("rongjie_noise", 2D) = "white" {}
		_liangbian_kuan("liangbian_kuan", Float) = 0.023
		_rongjie("rongjie", Range( 0 , 1)) = 0.3319695
		[HDR]_bian_col("bian_col", Color) = (1,0.9229986,0.4292453,1)
		[Enum(vertex col,1,rongjie,0)]_lizi_Kzhen_kaiguan("lizi_Kzhen_kaiguan", Float) = 1
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
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				half3 ase_normal : NORMAL;
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
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half _zifaguang_dantongdao_panding;
			uniform half _zhezhao_int;
			uniform half4 _zhezhao_col;
			uniform sampler2D _noise02;
			uniform half2 _noise02UV_speed;
			uniform half2 _noise02UV_tilling;
			uniform sampler2D _noise01;
			uniform half2 _noise01UV_speed;
			uniform half2 _noise01UV_tilling;
			uniform half4 _noise_shuchu_col;
			uniform half _noise_shuchu_int;
			uniform sampler2D _rongjie_noise;
			uniform half4 _rongjie_noise_ST;
			uniform half _rongjie;
			uniform half _lizi_Kzhen_kaiguan;
			uniform half _liangbian_kuan;
			uniform half4 _bian_col;
			uniform half2 _fre;
			uniform half4 _fre_col;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				half3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
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
				half4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
				half4 temp_cast_0 = (tex2DNode2.r).xxxx;
				half4 lerpResult65 = lerp( tex2DNode2 , temp_cast_0 , _zifaguang_dantongdao_panding);
				half2 texCoord48 = i.ase_texcoord1.xy * _noise02UV_tilling + float2( 0,0 );
				half2 panner50 = ( 1.0 * _Time.y * _noise02UV_speed + texCoord48);
				half2 texCoord36 = i.ase_texcoord1.xy * _noise01UV_tilling + float2( 0,0 );
				half2 panner37 = ( 1.0 * _Time.y * _noise01UV_speed + texCoord36);
				float2 uv_rongjie_noise = i.ase_texcoord1.xy * _rongjie_noise_ST.xy + _rongjie_noise_ST.zw;
				half4 tex2DNode17 = tex2D( _rongjie_noise, uv_rongjie_noise );
				half lerpResult31 = lerp( i.ase_color.a , _rongjie , _lizi_Kzhen_kaiguan);
				half ifLocalVar10 = 0;
				if( tex2DNode17.r >= lerpResult31 )
				ifLocalVar10 = 1.0;
				else
				ifLocalVar10 = 0.0;
				half lerpResult32 = lerp( ( i.ase_color.a + _liangbian_kuan ) , ( _rongjie + _liangbian_kuan ) , _lizi_Kzhen_kaiguan);
				half ifLocalVar12 = 0;
				if( tex2DNode17.r >= lerpResult32 )
				ifLocalVar12 = 1.0;
				else
				ifLocalVar12 = 0.0;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				half3 ase_worldNormal = i.ase_texcoord2.xyz;
				half fresnelNdotV24 = dot( ase_worldNormal, ase_worldViewDir );
				half fresnelNode24 = ( 0.0 + _fre.x * pow( 1.0 - fresnelNdotV24, _fre.y ) );
				
				
				finalColor = ( ( ( ( lerpResult65 * _zhezhao_int * _zhezhao_col ) + ( ( tex2D( _noise02, panner50 ).r * tex2DNode2.r * tex2D( _noise01, panner37 ).r ) * _noise_shuchu_col.r * _noise_shuchu_int ) ) + ( ( ifLocalVar10 - ifLocalVar12 ) * _bian_col ) + ( fresnelNode24 * _fre_col ) ) * ifLocalVar10 * tex2DNode2.r );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
7;290;1920;729;3544.552;1232.744;2.108285;True;False
Node;AmplifyShaderEditor.Vector2Node;49;-3061.809,-786.4097;Inherit;False;Property;_noise02UV_tilling;noise02UV_tilling;6;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;43;-2987.943,-324.8163;Inherit;False;Property;_noise01UV_tilling;noise01UV_tilling;3;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;16;-1845.149,379.6985;Half;False;Property;_liangbian_kuan;liangbian_kuan;15;0;Create;True;0;0;0;False;0;False;0.023;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1915.049,119.4984;Half;False;Property;_rongjie;rongjie;16;0;Create;True;0;0;0;False;0;False;0.3319695;-1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;52;-2856.368,-692.8646;Half;False;Property;_noise02UV_speed;noise02UV_speed;7;0;Create;True;0;0;0;False;0;False;-0.1,0.1;-0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;38;-2782.502,-231.2712;Half;False;Property;_noise01UV_speed;noise01UV_speed;4;0;Create;True;0;0;0;False;0;False;0.25,-0.1;0.25,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2804.819,-343.9371;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-2878.685,-805.5305;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;9;-1816.516,-68.5742;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-1840.861,497.1337;Inherit;False;Property;_lizi_Kzhen_kaiguan;lizi_Kzhen_kaiguan;18;1;[Enum];Create;True;2;;;2;vertex col;1;rongjie;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-1553.063,318.5374;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1548.348,215.9985;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;50;-2645.419,-805.2988;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;37;-2571.553,-343.7054;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-2270.16,-997.9009;Half;False;Property;_zifaguang_dantongdao_panding;zifaguang_dantongdao_panding;9;1;[Enum];Create;True;0;2;RGBzifaguang;0;dantongdao;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1174.225,555.2448;Inherit;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1177.626,438.2917;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;-2404.502,-372.2712;Inherit;True;Property;_noise01;noise01;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;51;-2478.368,-833.8646;Inherit;True;Property;_noise02;noise02;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-1362.861,69.43372;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2442.987,-608.1863;Inherit;True;Property;_MainTex;MainTex;8;0;Create;True;0;0;0;False;0;False;-1;None;db4acfe39c07a874482dee42655170bd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;32;-1361.962,295.2339;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-1538.149,444.6985;Inherit;True;Property;_rongjie_noise;rongjie_noise;14;0;Create;True;0;0;0;False;0;False;-1;None;52acb306a801f0941abcc89a16827579;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-1854.867,-1188.85;Inherit;False;Property;_zhezhao_int;zhezhao_int;10;0;Create;True;0;0;0;False;0;False;0.2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;64;-1831.598,-698.9102;Half;False;Property;_zhezhao_col;zhezhao_col;11;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;10;-986.6299,271.7008;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-1066.092,-847.5441;Half;False;Property;_fre;fre;0;0;Create;True;0;0;0;False;0;False;1,5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2105.354,-632.8807;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-2069.888,-176.6645;Half;False;Property;_noise_shuchu_int;noise_shuchu_int;13;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-2098.616,-366.7213;Half;False;Property;_noise_shuchu_col;noise_shuchu_col;12;0;Create;True;0;0;0;False;0;False;0.4056604,0.4056604,0.4056604,1;0.4056603,0.4056603,0.4056603,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ConditionalIfNode;12;-990.5264,487.2447;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;65;-1826.6,-1010.919;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;28;-810.0991,-681.7206;Half;False;Property;_fre_col;fre_col;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;-707.436,369.6918;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1777.453,-473.2607;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1569.974,-1059.993;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;21;-718.064,602.1138;Half;False;Property;_bian_col;bian_col;17;1;[HDR];Create;True;0;0;0;False;0;False;1,0.9229986,0.4292453,1;0.8586388,1.193661,2,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;24;-907.9472,-891.4946;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-448.8649,425.9132;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-1336.082,-441.474;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-509.451,-770.9435;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-310.3424,-384.7613;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-55.79069,-29.55391;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;222.0483,-13.48809;Half;False;True;-1;2;ASEMaterialInspector;100;1;Unlit/raodongrongjie;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;2;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;36;0;43;0
WireConnection;48;0;49;0
WireConnection;59;0;18;0
WireConnection;59;1;16;0
WireConnection;19;0;9;4
WireConnection;19;1;16;0
WireConnection;50;0;48;0
WireConnection;50;2;52;0
WireConnection;37;0;36;0
WireConnection;37;2;38;0
WireConnection;39;1;37;0
WireConnection;51;1;50;0
WireConnection;31;0;9;4
WireConnection;31;1;18;0
WireConnection;31;2;33;0
WireConnection;32;0;19;0
WireConnection;32;1;59;0
WireConnection;32;2;33;0
WireConnection;10;0;17;1
WireConnection;10;1;31;0
WireConnection;10;2;13;0
WireConnection;10;3;13;0
WireConnection;10;4;14;0
WireConnection;58;0;51;1
WireConnection;58;1;2;1
WireConnection;58;2;39;1
WireConnection;12;0;17;1
WireConnection;12;1;32;0
WireConnection;12;2;13;0
WireConnection;12;3;13;0
WireConnection;12;4;14;0
WireConnection;65;0;2;0
WireConnection;65;1;2;1
WireConnection;65;2;66;0
WireConnection;15;0;10;0
WireConnection;15;1;12;0
WireConnection;57;0;58;0
WireConnection;57;1;42;1
WireConnection;57;2;44;0
WireConnection;60;0;65;0
WireConnection;60;1;61;0
WireConnection;60;2;64;0
WireConnection;24;2;26;1
WireConnection;24;3;26;2
WireConnection;20;0;15;0
WireConnection;20;1;21;0
WireConnection;62;0;60;0
WireConnection;62;1;57;0
WireConnection;27;0;24;0
WireConnection;27;1;28;0
WireConnection;22;0;62;0
WireConnection;22;1;20;0
WireConnection;22;2;27;0
WireConnection;35;0;22;0
WireConnection;35;1;10;0
WireConnection;35;2;2;1
WireConnection;1;0;35;0
ASEEND*/
//CHKSM=705B889DE3842A8976FEEB2F7E5BE44C20C57C9F