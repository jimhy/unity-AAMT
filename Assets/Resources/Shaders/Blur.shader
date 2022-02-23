Shader "Custom/Blur" {
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		[HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil ("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255

		[HideInInspector]_ColorMask ("Color Mask", Float) = 15

		
		_Size ("BlurSize", Range(0, 50)) = 6
		_Center("Center", range(-1,1)) = 0.45              //中心点y坐标值
        _R("AlphaValue", range(0,1)) = 0.2 
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Geometry" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		//ZWrite Off
		//ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
			Name "FrontBlurHor"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
		
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
		
		
			fixed _Center;
            fixed _R;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Size;

			half4 GrabPixel(v2f i, float weight, float kernelx){
				if (_Size <= 1 || weight == 0){
					return tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size, i.texcoord.y)) * weight;
				}else{
					half4 sum = half4(0,0,0,0);
					sum += tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size*0.2, i.texcoord.y))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size*0.4, i.texcoord.y))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size*0.6, i.texcoord.y))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size*0.8, i.texcoord.y))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x + _MainTex_TexelSize.x*kernelx*_Size*1.0, i.texcoord.y))*0.2;
					return (sum + _TextureSampleAdd) * weight;
				}
			}



			fixed4 frag(v2f IN) : SV_Target
			{
				half4 sum = half4(0,0,0,0);
		
				 //sum += GrabPixel(IN, 0.05, -4.0);
				 //sum += GrabPixel(IN, 0.09, -3.0);
				 //sum += GrabPixel(IN, 0.12, -2.0);
				 //sum += GrabPixel(IN, 0.15, -1.0);
				 //sum += GrabPixel(IN, 0.18,  0.0);
				 //sum += GrabPixel(IN, 0.15, +1.0);
				 //sum += GrabPixel(IN, 0.12, +2.0);
				 //sum += GrabPixel(IN, 0.09, +3.0);
				 //sum += GrabPixel(IN, 0.05, +4.0);


				 sum += GrabPixel(IN, 0.01, -9.0);
				 sum += GrabPixel(IN, 0.02, -8.0);
				 sum += GrabPixel(IN, 0.03, -7.0);
				 sum += GrabPixel(IN, 0.04, -6.0);
				 sum += GrabPixel(IN, 0.05, -5.0);
				 sum += GrabPixel(IN, 0.06, -4.0);
				 sum += GrabPixel(IN, 0.07, -3.0);
				 sum += GrabPixel(IN, 0.08, -2.0);
				 sum += GrabPixel(IN, 0.09, -1.0);
				 sum += GrabPixel(IN, 0.10,  0.0);
				 sum += GrabPixel(IN, 0.09, +1.0);
				 sum += GrabPixel(IN, 0.08, +2.0);
				 sum += GrabPixel(IN, 0.07, +3.0);
				 sum += GrabPixel(IN, 0.06, +4.0);
				 sum += GrabPixel(IN, 0.05, +5.0);
				 sum += GrabPixel(IN, 0.04, +6.0);
				 sum += GrabPixel(IN, 0.03, +7.0);
				 sum += GrabPixel(IN, 0.02, +8.0);
				 sum += GrabPixel(IN, 0.01, +9.0);


				sum = sum * IN.color;
				float y = IN.texcoord.y;
                float s = y -( _Center - _R/2);
                float f = saturate(s / _R);
                sum.a = lerp(0, 1, f);
				return sum;

			}
		ENDCG
		}
		Pass
		{
			Name "FrontBlurVer"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			
			fixed _Center;
            fixed _R;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Size;

			half4 GrabPixel(v2f i, float weight, float kernely){
				if (_Size <= 1 || weight == 0){
					return tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size)) * weight;
				}else{
					half4 sum = half4(0,0,0,0);
					sum += tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size*0.2))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size*0.4))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size*0.6))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size*0.8))*0.2;
					sum += tex2D(_MainTex, half2(i.texcoord.x, i.texcoord.y + _MainTex_TexelSize.y*kernely*_Size*1.0))*0.2;
					return (sum + _TextureSampleAdd) * weight;
				}
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 sum = half4(0,0,0,0);  
				 //sum += GrabPixel(IN, 0.05, -4.0);
				 //sum += GrabPixel(IN, 0.09, -3.0);
				 //sum += GrabPixel(IN, 0.12, -2.0);
				 //sum += GrabPixel(IN, 0.15, -1.0);
				 //sum += GrabPixel(IN, 0.18,  0.0);
				 //sum += GrabPixel(IN, 0.15, +1.0);
				 //sum += GrabPixel(IN, 0.12, +2.0);
				 //sum += GrabPixel(IN, 0.09, +3.0);
				 //sum += GrabPixel(IN, 0.05, +4.0);

				 sum += GrabPixel(IN, 0.01, -9.0);
				 sum += GrabPixel(IN, 0.02, -8.0);
				 sum += GrabPixel(IN, 0.03, -7.0);
				 sum += GrabPixel(IN, 0.04, -6.0);
				 sum += GrabPixel(IN, 0.05, -5.0);
				 sum += GrabPixel(IN, 0.06, -4.0);
				 sum += GrabPixel(IN, 0.07, -3.0);
				 sum += GrabPixel(IN, 0.08, -2.0);
				 sum += GrabPixel(IN, 0.09, -1.0);
				 sum += GrabPixel(IN, 0.10,  0.0);
				 sum += GrabPixel(IN, 0.09, +1.0);
				 sum += GrabPixel(IN, 0.08, +2.0);
				 sum += GrabPixel(IN, 0.07, +3.0);
				 sum += GrabPixel(IN, 0.06, +4.0);
				 sum += GrabPixel(IN, 0.05, +5.0);
				 sum += GrabPixel(IN, 0.04, +6.0);
				 sum += GrabPixel(IN, 0.03, +7.0);
				 sum += GrabPixel(IN, 0.02, +8.0);
				 sum += GrabPixel(IN, 0.01, +9.0);



				sum = sum * IN.color;

				float y = IN.texcoord.y;
                float s = y -( _Center - _R/2);
                float f = saturate(s / _R);
                sum.a = lerp(0, 1, f);
				
				// = pow(IN.texcoord.y,_AlphaValue);
	
				return sum;


			}
		ENDCG
		}



	}
}
