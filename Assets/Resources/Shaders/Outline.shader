Shader "Custom/Outline"
{
	//属性
	Properties{
		[Header(_____________Outline____________)]
		_OutlineCol("OutlineCol", Color) = (0.005,0.9,0,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.03
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
			#pragma vertex vert
			#pragma fragment frag
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
			
			ENDCG
		}	
	}
	//前面的Shader失效的话，使用默认的Diffuse
	FallBack "Diffuse"
}