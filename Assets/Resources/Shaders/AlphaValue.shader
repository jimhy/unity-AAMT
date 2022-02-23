Shader "Custom/AlphaValue"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "blanck" {}
        _center("Center", range(-1,1)) = 0.6              //中心点y坐标值
		_range("AlphaValue", range(0,1)) = 0.2
    }
    SubShader
    {
       
       //ColorMask[RGB]

        Pass
        {
            Tags { "RenderType"="Transparent" "RenderType"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
             
                float4 vertex : SV_POSITION;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed _center;
	        uniform fixed _range;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_MainTex, i.uv);
                float y = i.uv.y;
		        float s = y - (_center - _range / 2);
		        float f = saturate( s / _range);
		        col.a = lerp(0.01, 1, f);

                return col;
            }
            ENDCG
        }
    }
}
