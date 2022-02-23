Shader "Custom/CubeReflecFlowOutLine"
{
	//属性
	Properties{

		_Color("Diff Color", Color) = (1,1,1,1)
		_Diffuse("Diffuse",float )=1
		[NOScaleOffset]_MainTex ("MainTex", 2D) = "white" {}
		
		_Environ ("Environ", Cube) = ""{}
		_Specular("Specular", Range(0,9)) = 1
		//[NOScaleOffset]_SpecTex("Spec Tex", 2D) = "white" {}

		[NOScaleOffset]_CubeLightMap("CubeLightMap", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimRange("Rim Offset", Range(0.3,16)) = 4
		_CapIntensity("Rim Num", Range(0,8)) = 0

		[Header(_____________Flow____________)]
		[Toggle] _FlowOn("Flow On", Float) = 0
		_Flow("Flow Texture",2D)="white"{}
		_Power ("Power", Range(0, 10)) = 1
		_FlowColor("Flow Color", Color) = (1,1,1,1)
        _U_SP ("U_SP", Float ) = 0
        _U_VP ("V_SP", Float ) = 0

		[Header(_____________Outline____________)]
		_OutlineCol("OutlineCol", Color) = (1,1,1,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.06
	}
 
	//子着色器	
	SubShader
	{
		//让渲染队列靠后，并且渲染顺序为从后向前，保证描边效果不被其他对象遮挡。
		Tags{"Queue" = "Transparent"}
		//描边使用两个Pass，第一个pass沿法线挤出一点，只输出描边的颜色
		Pass //描边
		{
			//剔除正面，只渲染背面
			Cull Front
			//关闭深度写入
			ZWrite Off
			
			CGPROGRAM
			#include "UnityCG.cginc"
			fixed4 _OutlineCol;
			float _OutlineFactor;
			
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			
			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				//将法线方向转换到视空间
				float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				//将视空间法线xy坐标转化到投影空间
				float2 offset = TransformViewToProjection(vnormal.xy);
				//在最终投影阶段输出进行偏移操作
				o.pos.xy += offset * _OutlineFactor;
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				//这个Pass直接输出描边颜色
				return _OutlineCol;
			}
			
			//使用vert函数和frag函数
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		
		//正常着色的Pass
		
			//ColorMask RGBA
			UsePass "Custom/CubeReflecFlow/CUBE"

		
	}
	//前面的Shader失效的话，使用默认的Diffuse
	FallBack "Diffuse"
}