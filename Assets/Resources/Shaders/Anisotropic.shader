Shader "Custom/Anisotropic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss("Gloss", Range(0,300)) = 1
        _Specular("Specular", Range(0,9)) = 1
		_speColor("speColor", Color) = (1,1,1,1)
        _Diffuse("Diffuse", Range(0,9)) = 1
        [NOScaleOffset]_SpecTex("Spec Tex", 2D) = "white" {}
		
		[NOScaleOffset]_CubeLightMap("CubeLightMap", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimRange("Rim Offset", Range(0.3,16)) = 4
		_CapIntensity("Rim Num", Range(0,8)) = 0


       // _CubeTex ("Cube Tex", 2D) = ""{}

       	[Header(_____________Shadow____________)]
		_ShadowColor("ShadowColor",color)=(0,0,0,0)
		_ShadowFalloff("ShadowFalloff",Range(0,1))=1
		_LightDir("LightDir",vector)=(1,1,1,1)
    }
    SubShader
    {
        Tags { "LightMode"="ForwardBase" }
       

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
     
            #include "Lighting.cginc"
            #include "UnityCG.cginc"

           

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 world_pos : TEXCOORD1;
                float3 world_normal : TEXCOORD2;
                float3 world_tangent : TEXCOORD3;
				fixed2 cap : COLOR;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Gloss;
            float _Diffuse;
            float _Specular;
            uniform sampler2D _CubeTex;
            uniform sampler2D _SpecTex;
			uniform half4 _speColor;
			
			uniform half _RimRange;	
			uniform half _CapIntensity;
			uniform sampler2D _CubeLightMap;
			uniform fixed4 _RimColor;

			

            v2f vert (appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy =v.texcoord.xy;
				
				o.uv.z =v.texcoord.x + _Time.y*0.01;
				o.uv.w =v.texcoord.y + _Time.y*0.1;



                o.world_pos=mul(unity_ObjectToWorld,v.vertex);
                o.world_normal=UnityObjectToWorldNormal(v.normal);
                o.world_tangent=UnityObjectToWorldDir(v.tangent);

				half2 capCoord;
				capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);
				capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);
				o.cap = capCoord * 0.5 + 0.5;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv.xy) ;
                
                
                float3 N = normalize(i.world_normal);
                float3 T = normalize(i.world_tangent);
                float3 V = normalize(UnityWorldSpaceViewDir(i.world_pos));

               fixed3 reflectionDir = normalize(reflect(-V, N));
             
                fixed4 L = tex2D(_CubeTex,reflectionDir);

                float3 dl= dot(T,L);
                float3 de = dot(T,V);

                fixed4 specTex = tex2D(_SpecTex, i.uv.xy);

				fixed4 cap = tex2D(_CubeLightMap, i.cap);
				fixed4 rim = pow(cap,_RimRange);	
				rim.rgb*=_CapIntensity * _RimColor -1.5;

				//fixed4 lightTexA=tex2D(_Flow,i.uv.zw).r * _Power  * specTex; 

                float3 specular_col =  saturate( pow((dl*de+sqrt(1-dl*dl)*sqrt(1-de*de)),_Gloss));
                float3 fcol = col.rgb*_Diffuse + rim.rgb + specular_col*_Specular * specTex*_speColor ;

               
                return fixed4(fcol,1);
            }
            ENDCG
        }
    
		 Pass
	{
		
		Tags{"Queue"="Transparent+1" }
		//用使用模板测试以保证alpha显示正确
		Stencil
		{
			Ref 0
			Comp equal
			Pass incrWrap
			Fail keep
			ZFail keep
		}

		//透明混合模式
		Blend SrcAlpha OneMinusSrcAlpha
	
		//关闭深度写入
		ZWrite off

		//深度稍微偏移防止阴影与地面穿插
		Offset -1 , 0

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		struct appdata
		{
			float4 vertex : POSITION;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
		};

		uniform half4 _LightDir;
		uniform half4 _ShadowColor;
		uniform fixed _ShadowFalloff;

		float3 ShadowProjectPos(float4 vertPos)
		{
			float3 shadowPos;

			//得到顶点的世界空间坐标
			float3 worldPos = mul(unity_ObjectToWorld , vertPos).xyz;

			//灯光方向
			float3 lightDir = normalize(_LightDir.xyz);

			//阴影的世界空间坐标（低于地面的部分不做改变）
			shadowPos.y = min(worldPos .y , _LightDir.w);
			shadowPos.xz = worldPos .xz - lightDir.xz * max(0 , worldPos .y - _LightDir.w) / lightDir.y; 

			return shadowPos;
		}

		v2f vert (appdata v)
		{
			v2f o;

			//得到阴影的世界空间坐标
			float3 shadowPos = ShadowProjectPos(v.vertex);

			//转换到裁切空间
			o.vertex = UnityWorldToClipPos(shadowPos);

			//得到中心点世界坐标
			float3 center =float3( unity_ObjectToWorld[0].w , _LightDir.w , unity_ObjectToWorld[2].w);
			//计算阴影衰减
			float falloff = 1-saturate(distance(shadowPos , center) * _ShadowFalloff);

			//阴影颜色
			o.color = _ShadowColor; 
			o.color.a *= falloff;

			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			return i.color;
		}
		ENDCG
	}
	}
}
